using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.CloudCode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class CloudCodeModulesView : EditorView
    {
        CloudCodeController CloudCode => App.CloudCode;

        readonly CloudCodeSectionView SectionView;
        UIElement Container;

        public CloudCodeModulesView(App app, CloudCodeSectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            CloudCode.ModulesChanged += CreateGUIListModules;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.CloudCodeModulesAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Load Modules", GetModulesAsync).SetWidth(200);
                UI.Space();
                Container = UI.Element();
            }
        }

        void CreateGUIListModules()
        {
            Container.Clear();

            if (CloudCode.CloudCodeModules == null)
                return;

            using (Container.Scope())
            {
                foreach (var module in CloudCode.CloudCodeModules)
                {
                    using (UI.HeaderPanel().Scope())
                    {
                        UI.Label(module.Name).SetWidth(200);
                    }
                    using (UI.ContentPanel().Scope())
                    {
                        using (UI.Block().Scope())
                        {
                            UI.Label("DateCreated").SetWidth(100);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => module.DateCreated.ToString())).SetWidth(200);
                        }
                        using (UI.Block().Scope())
                        {
                            UI.Label("DateModified").SetWidth(100);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => module.DateModified.ToString())).SetWidth(200);
                        }
                        using (UI.Block().Scope())
                        {
                            UI.Label("Language").SetWidth(100);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => module.Language.ToString())).SetWidth(50);
                        }

                        UI.Button("Delete", () => DeleteCloudCodeModuleAsync(module))
                            .SetWidth(100)
                            .SetBackgroundImageTint(Color.red);
                    }

                    UI.Space();
                }
            }
        }

        async Task GetModulesAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Getting CloudCode Modules...");
                await CloudCode.GetCloudCodeModulesAsync();
                SetStatus(Status.Success, $"GetCloudCodeModules Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "GetCloudCodeModules Failed.", e);
            }
        }

        async Task DeleteCloudCodeModuleAsync(ModuleMetadata module)
        {
            try
            {
                SetStatus(Status.Warning, $"Deleting CloudCode Module '{module.Name}'...");
                await CloudCode.DeleteCloudCodeModuleAsync(module);
                SetStatus(Status.Success, $"DeleteCloudCodeModule Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "DeleteCloudCodeModule Failed.", e);
            }
        }

        void SetStatus(Status status, string message, Exception exception = null)
        {
            SectionView.SetStatus(status, message, exception);
        }
    }
}
