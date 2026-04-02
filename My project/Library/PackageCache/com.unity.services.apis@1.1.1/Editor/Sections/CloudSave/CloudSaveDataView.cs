using System;
using System.Threading.Tasks;

namespace Unity.Services.Apis.Sample
{
    class CloudSaveDataView : EditorView
    {
        CloudSaveController CloudSave => App.CloudSave;

        readonly CloudSaveSectionView SectionView;

        string m_PlayerIdPrompt = "";
        string m_SetKey = "";
        string m_SetValue = "";

        UIElement DataContainer;

        public CloudSaveDataView(App app, CloudSaveSectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            CloudSave.DataChanged += RefreshData;
        }

        IUIBinding<bool> CustomDataBinding;
        IUIBinding<bool> PlayerDataBinding;

        public override void CreateGUI()
        {
            UI.H5("Admin API");
            UI.Separator();
            UI.Snippet().SetText(Snippets.CloudSaveAdmin).SetPaddingBottom(10);

            UI.H5("Trusted API");
            UI.Separator();
            UI.Snippet().SetText(Snippets.CloudSaveTrusted).SetPaddingBottom(10);

            CustomDataBinding = UI.BindReadOnly(() => CloudSave.DataType == CloudSaveDataType.Custom);
            PlayerDataBinding = UI.BindReadOnly(() => CloudSave.DataType == CloudSaveDataType.Player);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.H5("Sample");
                UI.Separator();

                using (UI.Block().SetName("LoadPlayerContainer").SetMargin(5, 0, 5, 0).Scope())
                {
                    UI.Label("Data Type").SetWidth(100);
                    UI.EnumField()
                        .Init(CloudSave.DataType)
                        .BindValue(UI.BindTarget(
                            () => (Enum)CloudSave.DataType,
                            (value) => CloudSave.SetDataType((CloudSaveDataType)value)))
                        .SetWidth(100);
                }

                using (UI.Block().SetName("LoadPlayerContainer").SetMargin(5, 0, 5, 0).Scope())
                {
                    UI.Label("Custom Id").BindVisibility(CustomDataBinding, true).SetWidth(100);
                    UI.Label("Player Id").BindVisibility(PlayerDataBinding, true).SetWidth(100);
                    UI.TextField().BindValue(UI.BindField(this, () => m_PlayerIdPrompt)).SetWidth(250);
                    UI.Button("Set", SetIdentifier).SetWidth(75);
                }

                UI.Space();

                var binding = UI.BindReadOnly(() => CloudSave.DataIdentifier != null);

                using (UI.HeaderPanel().BindVisibility(binding, true).Scope())
                {
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => CloudSave.DataIdentifier));
                    UI.Flex();
                    UI.Button("X", CloudSave.Reset).SetMargin(0).SetWidth(75);
                }
                using (UI.ContentPanel().BindVisibility(binding, true).Scope())
                {
                    CreateDataGUI();
                }
            }
        }

        void CreateDataGUI()
        {
            UI.EnumField()
                .Init(CloudSave.CustomDataType)
                .BindValue(UI.BindTarget(
                    () => (Enum)CloudSave.CustomDataType,
                    (value) => CloudSave.SetCustomDataType((CloudSaveCustomDataType)value)))
                .BindVisibility(CustomDataBinding, true)
                .SetWidth(100).SetMarginBottom(10);

            UI.EnumField()
                .Init(CloudSave.PlayerDataType)
                .BindValue(UI.BindTarget(
                    () => (Enum)CloudSave.PlayerDataType,
                    (value) => CloudSave.SetPlayerDataType((CloudSavePlayerDataType)value)))
                .BindVisibility(PlayerDataBinding, true)
                .SetWidth(100).SetMarginBottom(10);

            using (UI.HeaderPanel().Scope())
            {
                UI.H5($"Cloud Values");
                UI.Flex();
                UI.Button("Load", LoadCloudSaveValuesAsync).SetWidth(75);
            }

            DataContainer = UI.ContentPanel(false);

            using (UI.FooterPanel().Scope())
            {
                UI.TextField().BindValue(UI.BindField(this, () => m_SetKey)).SetWidth(150);
                UI.TextField().BindValue(UI.BindField(this, () => m_SetValue)).SetWidth(300);
                UI.Button($"Set", SetDataAsync).SetWidth(75);
            }
        }

        void RefreshData()
        {
            DataContainer.Clear();

            using (DataContainer.Scope())
            {
                if (CloudSave.CloudValues != null && CloudSave.CloudValues.Count > 0)
                {
                    foreach (var value in CloudSave.CloudValues)
                    {
                        using (UI.Block().Scope())
                        {
                            UI.Label(value.Key).SetWidth(150);
                            UI.Label(value.Value.ToString()).SetWidth(300).Wrap();
                            UI.Button($"Delete", () => DeleteCloudValueAsync(value.Key)).SetFlexGrow(0).SetWidth(75);
                        }
                    }
                }
                else
                {
                    UI.Label("- No values.");
                }
            }
        }

        void SetIdentifier()
        {
            if (!string.IsNullOrEmpty(m_PlayerIdPrompt))
            {
                CloudSave.SetIdentifier(m_PlayerIdPrompt);
            }

            m_PlayerIdPrompt = "";
        }

        async Task SetDataAsync()
        {
            try
            {
                await CloudSave.SetDataAsync(m_SetKey, m_SetValue);
                SetStatus(Status.Success, $"Set value '{m_SetKey}'.");

                m_SetKey = "";
                m_SetValue = "";
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Cloud Values failed to save.", e);
            }
        }

        async Task DeleteCloudValueAsync(string key)
        {
            try
            {
                await CloudSave.DeleteCloudValueAsync(key);
                SetStatus(Status.Success, $"Cloud Key '{key}' deleted.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, $"Cloud Key '{key}' failed to delete.", e);
            }
        }

        async Task LoadCloudSaveValuesAsync()
        {
            try
            {
                await CloudSave.LoadCloudSaveValuesAsync();
                SetStatus(Status.Success, "Cloud Values loaded!");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Cloud Values failed to load.", e);
            }
        }

        void SetStatus(Status status, string message, Exception exception = null)
        {
            SectionView.SetStatus(status, message, exception);
        }
    }
}
