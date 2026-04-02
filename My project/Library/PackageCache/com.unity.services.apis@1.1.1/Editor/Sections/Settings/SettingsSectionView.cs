using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class SettingsSectionView : SectionView
    {
        public override Section Section => Section.Settings;
        public override string Url => "https://services.docs.unity.com/docs/service-account-auth/";
        public override string Info => "Configure the project, environment and service account for the samples.";

        SettingsController Settings => App.Settings;

        CloudSettings CloudSettings => Settings.CloudSettings;

        string m_CloudProjectId;
        string m_EnvironmentName;
        string m_EnvironmentId;
        string m_AdminKeyIdentifier;
        string m_AdminKeySecret;
        string m_TrustedKeyIdentifier;
        string m_TrustedKeySecret;

        bool m_UpdateFoldout;

        public SettingsSectionView(App app) : base(app)
        {
        }

        public override void Initialize()
        {
            m_CloudProjectId = CloudSettings.CloudProjectId;
            m_EnvironmentName = CloudSettings.GameEnvironmentName;
            m_EnvironmentId = CloudSettings.GameEnvironmentId;
            m_AdminKeyIdentifier = CloudSettings.AdminKeyIdentifier;
            m_AdminKeySecret = CloudSettings.AdminKeySecret;
            m_TrustedKeyIdentifier = CloudSettings.TrustedKeyIdentifier;
            m_TrustedKeySecret = CloudSettings.TrustedKeySecret;
        }

        protected override void CreateGUISection()
        {
            using (UI.HorizontalElement().SetMargin(5, 0, 5, 0).Scope())
            {
                UI.Button("Project", OpenGeneral).SetWidth(150);
                UI.Space();
                UI.Button("Environments", OpenEnvironments).SetWidth(150);
                UI.Space();
                UI.Button("Service Accounts", OpenServiceAccounts).SetWidth(150);
            }

            UI.HelpBox()
                .SetText("Settings are stored in EditorPrefs. Service Accounts must be configured with the proper permissions.")
                .SetMessageType(HelpBoxMessageType.Info).SetMargin(5, 0, 10, 0);

            using (UI.HeaderPanel().Scope())
            {
                UI.H2($"Current");
            }
            using (UI.ContentPanel().SetMarginBottom(10).Scope())
            {
                using (UI.Block().Scope())
                {
                    UI.Label("CloudProjectId").SetWidth(150);
                    UI.SelectableLabel().SetWidth(300).BindValue(UI.BindReadOnly(() => CloudSettings.CloudProjectId));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("EnvironmentName").SetWidth(150);
                    UI.SelectableLabel().SetWidth(300).BindValue(UI.BindProperty(() => CloudSettings.GameEnvironmentName));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("EnvironmentId").SetWidth(150);
                    UI.SelectableLabel().SetWidth(300).BindValue(UI.BindProperty(() => CloudSettings.GameEnvironmentId));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("AdminKeyIdentifier").SetWidth(150);
                    UI.SelectableLabel().SetWidth(300).BindValue(UI.BindProperty(() => CloudSettings.AdminKeyIdentifier));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("AdminKeySecret").SetWidth(150);
                    UI.SelectableLabel().SetWidth(300).BindValue(UI.BindProperty(() => CloudSettings.AdminKeySecret));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("TrustedKeyIdentifier").SetWidth(150);
                    UI.SelectableLabel().SetWidth(300).BindValue(UI.BindProperty(() => CloudSettings.TrustedKeyIdentifier));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("TrustedKeySecret").SetWidth(150);
                    UI.SelectableLabel().SetWidth(300).BindValue(UI.BindProperty(() => CloudSettings.TrustedKeySecret));
                }
            }

            var binding = UI.BindReadOnly(() => m_UpdateFoldout);

            using (UI.HeaderPanel(binding).Scope())
            {
                UI.Foldout().BindValue(UI.BindField(this, () => m_UpdateFoldout));
                UI.H2($"Update");
            }

            using (UI.ContentPanel(false)
                .BindVisibility(binding, true)
                .Scope())
            {
                using (UI.Block().Scope())
                {
                    UI.Label("CloudProjectId").SetWidth(150);
                    UI.TextField().SetWidth(300).BindValue(UI.BindField(this, () => m_CloudProjectId));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("EnvironmentName").SetWidth(150);
                    UI.TextField().SetWidth(300).BindValue(UI.BindField(this, () => m_EnvironmentName));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("EnvironmentId").SetWidth(150);
                    UI.TextField().SetWidth(300).BindValue(UI.BindField(this, () => m_EnvironmentId));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("AdminKeyIdentifier").SetWidth(150);
                    UI.TextField().SetWidth(300).BindValue(UI.BindField(this, () => m_AdminKeyIdentifier));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("AdminKeySecret").SetWidth(150);
                    UI.TextField().SetWidth(300).BindValue(UI.BindField(this, () => m_AdminKeySecret));
                }
                using (UI.Block().Scope())
                {
                    UI.Label("TrustedKeyIdentifier").SetWidth(150);
                    UI.TextField().SetWidth(300).BindValue(UI.BindField(this, () => m_TrustedKeyIdentifier));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("TrustedKeySecret").SetWidth(150);
                    UI.TextField().SetWidth(300).BindValue(UI.BindField(this, () => m_TrustedKeySecret));
                }
            }
            using (UI.FooterPanel()
                .BindVisibility(binding, true)
                .Scope())
            {
                UI.Button("Save", Save).SetWidth(100);
            }

        }

        void Save()
        {
            Settings.Save(m_CloudProjectId, m_EnvironmentName, m_EnvironmentId, m_AdminKeyIdentifier, m_AdminKeySecret, m_TrustedKeyIdentifier, m_TrustedKeySecret);
            SetStatus(Status.Success, "Settings Set!");
        }

        void OpenGeneral()
        {
            Application.OpenURL("https://dashboard.unity3d.com/admin-portal/organizations/default/projects/default/settings/general");
        }

        void OpenEnvironments()
        {
            Application.OpenURL("https://dashboard.unity3d.com/admin-portal/organizations/default/projects/default/settings/environments");
        }

        void OpenServiceAccounts()
        {
            Application.OpenURL("https://dashboard.unity3d.com/admin-portal/organizations/default/settings/service-accounts");
        }
    }
}
