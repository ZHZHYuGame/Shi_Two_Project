using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.Leaderboards;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class LeaderboardsSectionView : SectionView
    {
        public override Section Section => Section.Leaderboards;
        public override string Url => "https://services.docs.unity.com/leaderboards-admin/";
        public override string Info => "Configure your leaderboard configuration and player entries";

        LeaderboardsController Leaderboards => App.Leaderboards;

        UIElement LeaderboardsContainer;
        UIElement ScoresContainer;

        public LeaderboardsSectionView(App app) : base(app)
        {
            Leaderboards.LeaderboardsChanged += RefreshLeaderboardsGUI;
            Leaderboards.ScoresChanged += RefreshScoresGUI;
        }

        protected override void CreateGUISection()
        {
            UI.Snippet().SetText(Snippets.LeaderboardsAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Load Leaderboards", LoadLeaderboardsAsync).SetWidth(200);
                UI.Space();

                LeaderboardsContainer = UI.Element();
            }
        }

        private void RefreshLeaderboardsGUI()
        {
            LeaderboardsContainer.Clear();
            ScoresContainer?.Clear();

            if (Leaderboards.CurrentLeaderboard != null)
            {
                using (LeaderboardsContainer.Scope())
                {
                    using (UI.HeaderPanel().Scope())
                    {
                        UI.Button("<", Leaderboards.SelectNextLeaderboardAsync).SetWidth(50);
                        UI.Flex();
                        UI.H1(Leaderboards.CurrentLeaderboard.Name);
                        UI.Flex();
                        UI.Button(">", Leaderboards.SelectNextLeaderboardAsync).SetWidth(50);
                    }

                    using (UI.ScrollPanel().Scope())
                    {
                        ScoresContainer = UI.ScrollView();
                        RefreshScoresGUI();
                    }
                }
            }
        }

        private void RefreshScoresGUI()
        {
            if (ScoresContainer == null)
                return;

            ScoresContainer.Clear();

            if (Leaderboards.CurrentScores != null)
            {
                using (ScoresContainer.Scope())
                {
                    using (UI.HeaderPanel().SetPadding(4).SetBorderRadius(0).Scope())
                    {
                        UI.H5("Id").SetMarginRight(6).SetWidth(250);
                        UI.H5("Name").SetMarginRight(6).SetWidth(150);
                        UI.H5("Rank").SetMarginRight(6).SetWidth(50);
                        UI.H5("Score").SetMarginRight(6).SetWidth(50);
                        UI.H5("Actions").SetMarginRight(6).SetWidth(100);
                    }

                    for (var i = 0; i != Leaderboards.CurrentScores.Count; ++i)
                    {
                        var result = Leaderboards.CurrentScores[i];

                        using (UI.HorizontalPanel()
                            .SetBorderRadius(0)
                            .SetBorderBottomWidth(0)
                            .SetBackgroundColor(i % 2 == 0 ? Color.clear : new Color(1f, 1f, 1f, 0.05f))
                            .SetPadding(5, 0, 5, 0)
                            .Scope())
                        {
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => result.PlayerId)).SetWidth(250);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => result.PlayerName)).SetWidth(150);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => (result.Rank + 1).ToString())).SetWidth(50);
                            UI.SelectableLabel().BindValue(UI.BindReadOnly(() => result.Score.ToString())).SetWidth(50);
                            UI.Button("Delete", () => DeleteEntryAsync(result)).SetWidth(100);
                        }
                    }
                }
            }
        }
        async Task LoadLeaderboardsAsync()
        {
            try
            {
                await Leaderboards.LoadAsync();
                SetStatus(Status.Success, "Load successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Load failed.", e);
            }
        }

        async Task DeleteEntryAsync(LeaderboardEntry entry)
        {
            try
            {
                await Leaderboards.DeleteEntryAsync(entry);
                SetStatus(Status.Success, "Deleted entry successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Delete entry failed.", e);
            }
        }
    }
}
