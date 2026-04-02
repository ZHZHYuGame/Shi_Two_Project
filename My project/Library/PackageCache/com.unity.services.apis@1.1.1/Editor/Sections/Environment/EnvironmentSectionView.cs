using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.Environment;

namespace Unity.Services.Apis.Sample
{
    class EnvironmentSectionView : SectionView
    {
        public override Section Section => Section.Environment;
        public override string Url => "https://services.docs.unity.com/unity/";
        public override string Info => "Configure your environments";

        EnvironmentsController Environments => App.Environments;
        string m_NewEnvironmentName;

        UIElement Container;

        public EnvironmentSectionView(App app) : base(app)
        {
            Environments.EnvironmentsChange += RefreshContainer;
        }

        protected override void CreateGUISection()
        {
            UI.Snippet().SetText(Snippets.EnvironmentAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Load Environments", GetEnvironmentsAsync).SetWidth(200);

                UI.Space();

                Container = UI.Element();
                RefreshContainer();

                UI.Space();

                using (UI.HeaderPanel().Scope())
                {
                    UI.H3("Create Environment");
                }
                using (UI.ContentPanel(false).Scope())
                {
                    using (UI.Block().Scope())
                    {
                        UI.Label("Name").SetWidth(75);
                        UI.TextField().BindValue(UI.BindField(this, () => m_NewEnvironmentName)).SetWidth(200);
                    }
                }
                using (UI.FooterPanel().Scope())
                {
                    UI.Button("Create", CreateAsync).SetWidth(100);
                }
            }
        }

        void RefreshContainer()
        {
            Container.Clear();

            if (Environments.Environments != null)
            {
                using (Container.Scope())
                {
                    foreach (var environment in Environments.Environments)
                    {
                        using (UI.HeaderPanel().Scope())
                        {
                            UI.H5(environment.Name);
                            UI.Flex();

                            if (environment.IsDefault)
                            {
                                UI.Label("[Default]");
                            }
                        }
                        using (UI.ContentPanel(false).Scope())
                        {
                            using (UI.Block().Scope())
                            {
                                UI.Label("Id").SetWidth(150);
                                UI.SelectableLabel().SetText(environment.Id);
                            }

                            using (UI.Block().Scope())
                            {
                                UI.Label("CreatedAt").SetWidth(150);
                                UI.SelectableLabel().SetText(environment.CreatedAt.ToString());
                            }

                            using (UI.Block().Scope())
                            {
                                UI.Label("UpdatedAt").SetWidth(150);
                                UI.SelectableLabel().SetText(environment.UpdatedAt.ToString());
                            }

                            if (environment.ArchivedAt != null)
                            {
                                using (UI.Block().Scope())
                                {
                                    UI.Label("ArchivedAt").SetWidth(150);
                                    UI.SelectableLabel().SetText(environment.ArchivedAt.ToString());
                                }
                            }
                        }
                        using (UI.FooterPanel().SetMarginBottom(5).Scope())
                        {
                        }
                    }
                }
            }
        }

        async Task GetEnvironmentsAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Getting Environments...");
                await Environments.GetEnvironmentsAsync();
                SetStatus(Status.Success, $"GetEnvironments Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "GetEnvironments Failed.", e);
            }
        }

        async Task CreateAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Creating Environment '{m_NewEnvironmentName}'...");
                await Environments.CreateAsync(m_NewEnvironmentName);
                m_NewEnvironmentName = "";
                SetStatus(Status.Success, $"Creation Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Creation Failed.", e);
            }
        }

        async Task DeleteAsync(EnvironmentResponse environment)
        {
            try
            {
                SetStatus(Status.Warning, $"Deleting Environment '{environment.Id}'...");
                await Environments.DeleteAsync(environment);
                SetStatus(Status.Success, $"Delete Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Delete Failed.", e);
            }
        }
    }
}
