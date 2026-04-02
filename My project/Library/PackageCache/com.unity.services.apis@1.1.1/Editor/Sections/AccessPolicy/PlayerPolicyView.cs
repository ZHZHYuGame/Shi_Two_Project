using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Apis.Admin.AccessPolicy;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class PlayerPolicyView : EditorView
    {
        AccessPolicyController Policy => App.AccessPolicy;
        AccessPolicySectionView SectionView { get; }

        string m_PlayerIdInput;
        bool ShowPolicies = true;
        bool ShowCreateForm;
        bool ShowCommonStatements;
        string StatementInput;

        UIElement PolicyContainer;

        public PlayerPolicyView(App app, AccessPolicySectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            SetStatementInput("", "", "", "");
            Policy.PlayerPoliciesChanged += RefreshPolicyGUI;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.PlayerPolicyAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                CreatePlayerPolicies();
                UI.Space();
                CreateStatementCreation();
                UI.Space();
                CreateCommonStatement();
            }
        }

        void CreatePlayerPolicies()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.Foldout().BindValue(UI.BindField(this, () => ShowPolicies));
                UI.H5("Player Policies");
                UI.Flex();
                UI.Button("Load", RefreshPlayerPoliciesAsync).SetWidth(50);
            }

            PolicyContainer = UI.ContentPanel().BindVisibility(UI.BindReadOnly(() => ShowPolicies));
        }

        void RefreshPolicyGUI()
        {
            PolicyContainer.Clear();

            if (Policy.PlayerPolicies == null)
                return;

            using (PolicyContainer.Scope())
            {
                foreach (var policy in Policy.PlayerPolicies)
                {
                    using (UI.HeaderPanel().Scope())
                    {
                        UI.H5("PlayerId:").SetWidth(100);
                        UI.SelectableLabel().BindValue(UI.BindReadOnly(() => policy.PlayerId));
                    }

                    using (UI.ContentPanel().Scope())
                    {
                        foreach (var statement in policy.Statements)
                        {
                            PlayerStatementGUI.CreateGUI(UI, statement);

                            using (UI.FooterPanel().Scope())
                            {
                                UI.Button("Delete", () => DeletePlayerPolicyStatementAsync(policy.PlayerId, statement)).SetWidth(100);
                            }

                            UI.Space();
                        }
                    }

                    UI.Space();
                }
            }
        }

        void CreateStatementCreation()
        {
            var binding = UI.BindField(this, () => ShowCreateForm);

            using (UI.HeaderPanel(binding).Scope())
            {
                UI.Foldout().BindValue(UI.BindField(this, () => ShowCreateForm));
                UI.H5("Create Player Statement");
            }

            using (UI.ContentPanel(false).BindVisibility(binding, true).Scope())
            {
                using (UI.HorizontalElement().SetPaddingBottom(5).Scope())
                {
                    UI.Label("PlayerId").SetWidth(100);
                    UI.TextField()
                        .BindValue(UI.BindField(this, () => m_PlayerIdInput))
                        .SetWidth(200);
                }

                UI.TextField()
                    .BindValue(UI.BindField(this, () => StatementInput))
                    .SetMultiline()
                    .SetPaddingBottom(5);

                UI.Button("Create", CreateStatementAsync).SetWidth(100);
            }
        }

        void CreateCommonStatement()
        {
            var binding = UI.BindField(this, () => ShowCommonStatements);

            using (UI.HeaderPanel(binding).Scope())
            {
                UI.Foldout().BindValue(UI.BindField(this, () => ShowCommonStatements));
                UI.H5("Common Player Statements");
            }

            using (UI.ContentPanel().BindVisibility(binding, true).Scope())
            {
                UI.Button("Block All", () => SetDenyStatementInput("BlockAll", "urn:ugs:*:*")).SetWidth(300);
                UI.Button("Block CloudSave", () => SetDenyStatementInput("BlockCloudSave", "urn:ugs:cloud-save:*")).SetWidth(300);
                UI.Button("Block Economy", () => SetDenyStatementInput("BlockEconomy", "urn:ugs:economy:*")).SetWidth(300);
                UI.Button("Block Leaderboards", () => SetDenyStatementInput("BlockLeaderboards", "urn:ugs:leaderboards:*")).SetWidth(300);
            }
        }

        async Task RefreshPlayerPoliciesAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Refreshing player policies...");
                await Policy.RefreshPlayerPoliciesAsync();
                SetStatus(Status.Success, $"Player policies refreshed.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Failed to refresh player policies.", e);
            }
        }

        async Task CreateStatementAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Creating player policy statement...");
                var statement = JsonConvert.DeserializeObject<PlayerStatement>(StatementInput);
                await Policy.UpdatePlayerPolicyAsync(m_PlayerIdInput, statement);
                SetStatus(Status.Success, $"Player policy statement created.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Failed to create player policy statement.", e);
            }
        }

        async Task DeletePlayerPolicyStatementAsync(string playerId, PlayerStatement statement)
        {
            try
            {
                SetStatus(Status.Warning, $"Deleting player policy statement...");
                await Policy.DeletePlayerPolicyAsync(playerId, statement.Sid);
                SetStatus(Status.Success, $"Player policy deleted.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Failed to delete player policy.", e);
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
            SetStatementInput(name, resource, "*", "Deny");
        }
    }
}
