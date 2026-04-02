using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Lobbies;

namespace Unity.Services.Apis.Sample
{
    class LobbiesSectionView : SectionView
    {
        public override Section Section => Section.Lobbies;
        public override string Url => "https://services.docs.unity.com/lobby/";
        public override string Info => "View and manage active lobbies";

        LobbiesController Lobbies => App.Lobbies;

        UIElement LobbiesContainer;

        public LobbiesSectionView(App app) : base(app)
        {
            Lobbies.LobbiesChanged += RefreshLobbiesGUI;
        }

        protected override void CreateGUISection()
        {
            UI.Snippet().SetText(Snippets.LobbiesTrusted).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Load Lobbies", LoadLobbiesAsync).SetWidth(200);
                UI.Space();

                LobbiesContainer = UI.Element();
            }
        }

        private void RefreshLobbiesGUI()
        {
            LobbiesContainer.Clear();

            if (Lobbies.Lobbies != null)
            {
                using (LobbiesContainer.Scope())
                {
                    foreach (var lobby in Lobbies.Lobbies)
                    {
                        using (UI.Block().Scope())
                        {
                            UI.Label(lobby.Id);
                            UI.Button("Delete", () => DeleteLobbyAsync(lobby));
                        }
                    }
                }
            }
        }

        async Task LoadLobbiesAsync()
        {
            try
            {
                await Lobbies.LoadAsync();
                SetStatus(Status.Success, "Load successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "Load failed.", e);
            }
        }

        async Task DeleteLobbyAsync(Lobby lobby)
        {
            try
            {
                await Lobbies.DeleteLobbyAsync(lobby);
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
