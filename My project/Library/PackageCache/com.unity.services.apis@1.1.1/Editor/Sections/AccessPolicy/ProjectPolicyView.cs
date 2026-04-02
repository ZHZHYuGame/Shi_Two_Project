using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Apis.Admin.AccessPolicy;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class ProjectPolicyView : EditorView
    {
        AccessPolicyController Policy => App.AccessPolicy;
        AccessPolicySectionView SectionView { get; }

        bool ShowPolicy = true;
        bool ShowCreateForm;
        bool ShowCommonStatements;
        string StatementInput;

        UIElement PolicyContainer;

        public ProjectPolicyView(App app, AccessPolicySectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            SetStatementInput("", "", "", "");
            Policy.ProjectPolicyChanged += RefreshPolicyGUI;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.ProjectPolicyAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                CreatePolicyGUI();
                UI.Space();
                DrawCreateStatement();
                UI.Space();
                DrawCommonStatement();
            }
        }

        void CreatePolicyGUI()
        {
            var binding = UI.BindField(this, () => ShowPolicy);

            using (UI.HeaderPanel(binding).Scope())
            {
                UI.Foldout().BindValue(binding);
                UI.H5("Project Policy");
                UI.Flex();
                UI.Button("Load", RefreshProjectPolicyAsync).SetWidth(50);
            }

            PolicyContainer = UI.ContentPanel(false);

            using (UI.FooterPanel().BindVisibility(binding, true).Scope())
            {
                UI.Button("Save", UpdateProjectPolicyAsync).SetWidth(100);
            }
        }

        void RefreshPolicyGUI()
        {
            PolicyContainer.Clear();

            if (Policy.ProjectPolicy == null)
                return;

            using (PolicyContainer.Scope())
            {
                foreach (var statement in Policy.ProjectPolicy.Statements)
                {
                    ProjectStatementGUI.DrawView(UI, statement);

                    using (UI.FooterPanel().Scope())
                    {
                        UI.Button("Delete", () => DeleteProjectPolicyStatementAsync(statement)).SetWidth(100);
                    }

                    UI.Space();
                }
            }
        }

        void DrawCreateStatement()
        {
            var binding = UI.BindField(this, () => ShowCreateForm);

            using (UI.HeaderPanel(binding).Scope())
            {
                UI.Foldout().BindValue(UI.BindField(this, () => ShowCreateForm));
                UI.H5("Create Project Statement");
            }

            using (UI.ContentPanel(false).BindVisibility(binding, true).Scope())
            {
                UI.TextField().BindValue(UI.BindField(this, () => StatementInput));
            }
            using (UI.FooterPanel().BindVisibility(binding, true).Scope())
            {
                UI.Button("Create", CreateStatementAsync).SetWidth(100);
            }
        }

        void DrawCommonStatement()
        {
            var binding = UI.BindField(this, () => ShowCommonStatements);

            using (UI.HeaderPanel(binding).Scope())
            {
                UI.Foldout().BindValue(binding);
                UI.H5("Common Project Statements");
            }

            using (UI.ContentPanel().BindVisibility(binding, true).Scope())
            {
                UI.Button("Block CloudSave Write", () => SetDenyStatementInput("BlockCloudSaveWrite", "urn:ugs:cloud-save:*")).SetWidth(300);
                UI.Button("Block Economy Write", () => SetDenyStatementInput("BlockEconomyWrite", "urn:ugs:economy:*")).SetWidth(300);
                UI.Button("Block Leaderboards Write", () => SetDenyStatementInput("BlockLeaderboardsWrite", "urn:ugs:leaderboards:*")).SetWidth(300);
            }
        }

        async Task RefreshProjectPolicyAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Refreshing project policy...");
                await Policy.RefreshProjectPolicyAsync();
                SetStatus(Status.Success, $"Project policy refreshed.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Failed to refresh project policy.", e);
            }
        }

        async Task CreateStatementAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Creating project policy statement...");
                var statement = JsonConvert.DeserializeObject<ProjectStatement>(StatementInput);
                await Policy.CreateProjectPolicyStatementAsync(statement);
                SetStatus(Status.Success, $"Project policy statement created.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Failed to create project policy statement.", e);
            }
        }

        async Task UpdateProjectPolicyAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Updating project policy...");
                await Policy.UpdateProjectPolicyAsync();
                SetStatus(Status.Success, $"Project policy updated.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Failed to update project policy.", e);
            }
        }

        async Task DeleteProjectPolicyStatementAsync(ProjectStatement statement)
        {
            try
            {
                SetStatus(Status.Warning, $"Deleting project policy statement...");
                await Policy.DeleteProjectPolicyAsync(statement.Sid);
                SetStatus(Status.Success, $"Project policy deleted.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Failed to delete project policy.", e);
            }
        }

        void SetStatus(Status status, string message, Exception exception = null)
        {
            SectionView.SetStatus(status, message, exception);
        }

        void SetStatementInput(string name, string resource, string action, string effect)
        {
            StatementInput = "{\n" +
                                $"\t\"Sid\": \"{name}\",\n" +
                                $"\t\"Action\": [\"{action}\"],\n" +
                                $"\t\"Effect\": \"{effect}\",\n" +
                                "\t\"Principal\": \"Player\",\n" +
                                $"\t\"Resource\": \"{resource}\",\n" +
                                "\t\"ExpiresAt\": \"2025-01-01T00:00:00.000Z\",\n" +
                                "\t\"Version\": \"1\"\n" +
                            "}";
        }

        void SetDenyStatementInput(string name, string resource)
        {
            SetStatementInput(name, resource, "Write", "Deny");
        }
    }
}
