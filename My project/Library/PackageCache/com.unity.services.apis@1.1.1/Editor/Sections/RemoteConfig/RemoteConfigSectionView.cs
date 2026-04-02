using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class RemoteConfigSectionView : SectionView
    {
        public override Section Section => Section.RemoteConfig;
        public override string Url => "https://services.docs.unity.com/remote-config-admin";
        public override string Info => "Manage your remote settings.";

        RemoteConfigController RemoteConfig => App.RemoteConfig;

        string m_RemoteConfigType = "settings";
        string m_RemoteConfigContent = "";

        public RemoteConfigSectionView(App app) : base(app)
        {
        }

        protected override void CreateGUISection()
        {
            UI.Snippet().SetText(Snippets.RemoteConfigAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                using (UI.HeaderPanel().Scope())
                {
                    UI.Label($"Settings");
                    UI.Flex();
                    UI.Button("Fetch", GetSettingsConfigAsync).SetWidth(50);
                }
                using (UI.ContentPanel().Scope())
                {
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => RemoteConfig.CurrentConfig));
                }
                /*
                UI.Space();

                using (UI.HeaderPanel().Scope())
                {
                    UI.Label($"Update");
                }
                using (UI.ContentPanel(false).Scope())
                {
                    UI.TextField()
                        .BindValue(UI.BindField(this, () => m_RemoteConfigContent))
                        .SetMultiline();
                }
                using (UI.FooterPanel().Scope())
                {
                    UI.Button("Update", UpdateRemoteConfigAsync).SetWidth(100);
                }*/
            }
        }

        Task GetSettingsConfigAsync()
        {
            m_RemoteConfigType = "settings";
            return GetRemoteConfigAsync();
        }

        async Task GetRemoteConfigAsync()
        {
            try
            {
                m_RemoteConfigContent = "";
                RemoteConfig.SetCurrentType(m_RemoteConfigType);
                SetStatus(Status.Warning, $"Getting Remote Config...");
                await RemoteConfig.GetRemoteConfigAsync();
                m_RemoteConfigContent = RemoteConfig.CurrentConfig;
                SetStatus(Status.Success, $"GetRemoteConfig Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "GetRemoteConfig Failed.", e);
            }
        }

        async Task UpdateRemoteConfigAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Updating Remote Config...");
                await RemoteConfig.UpdateRemoteConfigAsync(m_RemoteConfigContent);
                SetStatus(Status.Success, $"UpdateRemoteConfig Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "UpdateRemoteConfig Failed.", e);
            }
        }
    }
}
