#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.Matchmaker;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class MatchmakerSectionView : SectionView
    {
        public override Section Section => Section.Matchmaker;
        public override string Url => "https://services.docs.unity.com/matchmaker/";
        public override string Info => "View and manage matchmaker queues";

        MatchmakerController Matchmaker => App.Matchmaker;

        UIElement QueuesContainer;

        public MatchmakerSectionView(App app) : base(app)
        {
            Matchmaker.QueuesChanged += RefreshQueuesGUI;
        }

        protected override void CreateGUISection()
        {
            UI.Snippet().SetText(Snippets.MatchmakerAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Load Queues", LoadQueuesAsync).SetWidth(200);
                UI.Space();

                QueuesContainer = UI.Element();
            }
        }

        private void RefreshQueuesGUI()
        {
            QueuesContainer.Clear();

            if (Matchmaker.Queues != null)
            {
                using (QueuesContainer.Scope())
                {
                    using (UI.HeaderPanel().SetPadding(4).SetBorderRadius(0).Scope())
                    {
                        UI.H5("Name").SetMarginRight(6).SetWidth(100);
                        UI.H5("Enabled").SetMarginRight(6).SetWidth(75);
                        UI.H5("Pools").SetMarginRight(6).SetWidth(75);
                        UI.H5("MaxPlayers").SetMarginRight(6).SetWidth(85);
                    }

                    for (var i = 0; i != Matchmaker.Queues.Count; ++i)
                    {
                        var queue = Matchmaker.Queues[i];

                        using (UI.HorizontalPanel()
                            .SetBorderRadius(0)
                            .SetBorderBottomWidth(i == Matchmaker.Queues.Count - 1 ? 1 : 0)
                            .SetBackgroundColor(i % 2 == 0 ? Color.clear : new Color(1f, 1f, 1f, 0.05f))
                            .SetPadding(5, 0, 5, 0)
                            .Scope())
                        {
                            UI.SelectableLabel().SetText(queue.Name).SetWidth(100);
                            UI.SelectableLabel().SetText(queue.Enabled.ToString()).SetWidth(75);
                            UI.SelectableLabel().SetText(queue.FilteredPools.Count.ToString()).SetWidth(75);
                            UI.SelectableLabel().SetText(queue.MaxPlayersPerTicket.ToString()).SetWidth(85);
                            UI.Button("Delete", () => DeleteQueueAsync(queue));
                        }
                    }
                }
            }
        }

        async Task LoadQueuesAsync()
        {
            try
            {
                await Matchmaker.LoadAsync();
                SetStatus(Status.Success, "Load successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Load failed.", e);
            }
        }

        async Task DeleteQueueAsync(QueueConfig queue)
        {
            try
            {
                await Matchmaker.DeleteQueueAsync(queue);
                SetStatus(Status.Success, "Delete successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Delete failed.", e);
            }
        }
    }
}
#endif
