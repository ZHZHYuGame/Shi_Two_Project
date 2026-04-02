using System;
using System.Threading.Tasks;
using Unity.Services.Apis.PlayerAuthentication;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class AccountsSectionView : SectionView
    {
        public override Section Section => Section.Accounts;
        public override string Url => "https://services.docs.unity.com/player-auth-admin";
        public override string Info => "Manage your player accounts.";

        AccountsController Controller => App.Account;

        AuthenticationResponse Response { get; set; }

        string m_DeletePlayerId;

        UIElement ListContainer;

        public AccountsSectionView(App app) : base(app)
        {
            Controller.PlayersChanged += RefreshListGUI;
        }

        protected override void CreateGUISection()
        {
            UI.H5("Admin Api");
            UI.Separator();
            UI.Snippet().SetText(Snippets.PlayerAuthAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                CreatePlayerListGUI();
                UI.Space();
                CreatePlayerDeleteGUI();
                UI.Space();
            }

            UI.H5("Game Api");
            UI.Separator();
            UI.Snippet().SetText(Snippets.PlayerAuthGame).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                CreatePlayerAuthGUI();
            }
        }

        void CreatePlayerListGUI()
        {
            UI.Button("List", ListPlayersAsync).SetWidth(100).SetMargin(5, 0, 5, 0);
            ListContainer = UI.Element().SetMargin(5, 0, 5, 0);
        }

        void RefreshListGUI()
        {
            ListContainer.Clear();

            using (ListContainer.Scope())
            {
                if (Controller.Players == null)
                {
                    return;
                }

                if (Controller.Players.Results.Count > 0)
                {
                    using (UI.HeaderPanel().SetPadding(4).Scope())
                    {
                        UI.H5("Players").SetMarginRight(6).SetWidth(250);
                        UI.H5("LastLoginAt").SetMarginRight(6).SetWidth(125);
                        UI.H5("CreatedAt").SetMarginRight(6).SetWidth(125);
                        UI.H5("Disabled").SetMarginRight(6).SetWidth(75);
                        UI.H5("Actions").SetMarginRight(6).SetWidth(100);
                    }

                    for (var i = 0; i != Controller.Players.Results.Count; ++i)
                    {
                        var player = Controller.Players.Results[i];

                        using (UI.HorizontalPanel()
                            .SetBorderRadius(0)
                            .SetBorderBottomWidth(0)
                            .SetBackgroundColor(i % 2 == 0 ? Color.clear : new Color(1f, 1f, 1f, 0.05f))
                            .SetPadding(5, 0, 5, 0)
                            .Scope())
                        {
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => player.Id)).SetWidth(250);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => player.LastLoginAt)).SetWidth(125);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => player.CreatedAt)).SetWidth(125);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => player.Disabled.ToString())).SetWidth(75);
                            UI.Button("Delete", () => DeleteAsync(player.Id))
                                .SetBackgroundImageTint(Color.red)
                                .SetWidth(100);
                        }
                    }

                    using (UI.FooterPanel().SetPadding(4).Scope())
                    {
                        UI.Button("Clear", ClearPlayers).SetWidth(100);
                    }
                }
                else
                {
                    UI.Label("No results.");
                }
            }
        }

        void CreatePlayerDeleteGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H5("Delete Player");
            }
            using (UI.ContentPanel(false).Scope())
            {
                UI.TextField().BindValue(UI.BindField(this, () => m_DeletePlayerId)).SetWidth(300);
            }
            using (UI.FooterPanel().Scope())
            {
                UI.Button("Delete", () => DeleteAsync(m_DeletePlayerId)).SetWidth(100);
            }
        }

        void CreatePlayerAuthGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H5("Authentication");
            }
            using (UI.ContentPanel(false).Scope())
            {
                UI.SelectableLabel().BindValue(UI.BindReadOnly(() => Response?.UserId ?? "-"));
            }
            using (UI.FooterPanel().Scope())
            {
                UI.Button("Create", CreateAnonymousAsync).SetWidth(50);
                UI.Flex();
                UI.Button("Clear", ClearResponse).SetWidth(50);
            }
        }

        async Task ListPlayersAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"ListPlayers executing...");
                await Controller.ListPlayersAsync();
                SetStatus(Status.Success, $"ListPlayers Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "ListPlayers Failed.", e);
            }
        }

        void ClearPlayers()
        {
            Controller.ClearPlayers();
        }

        async Task CreateAnonymousAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Creating Anonymous Player...");
                Response = await Controller.SignUpAnonymouslyAsync();
                SetStatus(Status.Success, $"Create Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Create Failed.", e);
            }
        }

        async Task DeleteAsync(string playerId)
        {
            try
            {
                SetStatus(Status.Warning, $"Deleting Player {playerId}...");
                await Controller.DeleteAsync(playerId);
                m_DeletePlayerId = "";
                SetStatus(Status.Success, $"Delete Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Delete Failed.", e);
            }
        }

        void ClearResponse()
        {
            Response = null;
        }
    }
}
