using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Apis.Lobbies;

namespace Unity.Services.Apis.Sample
{
    class LobbiesController : ApiController
    {
        public event Action LobbiesChanged;

        ITrustedClient TrustedClient { get; set; }
        ILobbyApi LobbyApi { get; set; }

        public List<Lobby> Lobbies { get; } = new List<Lobby>();
        readonly string ServiceId;

        public LobbiesController(CloudSettings settings) : base(settings)
        {
            ServiceId = Guid.NewGuid().ToString();
        }

        public void Initialize(ITrustedClient trustedClient)
        {
            TrustedClient = trustedClient;
            LobbyApi = trustedClient.Lobby;
        }

        public async Task LoadAsync()
        {
            await AuthorizeAsync();
            Lobbies.Clear();
            var response = await LobbyApi.QueryLobbies(ServiceId);
            response.EnsureSuccessful();
            Lobbies.AddRange(response.Data.Results);
            LobbiesChanged?.Invoke();
        }

        public async Task DeleteLobbyAsync(Lobby lobby)
        {
            await AuthorizeAsync();
            var response = await LobbyApi.DeleteLobby(lobby.Id, ServiceId, lobby.HostId);
            response.EnsureSuccessful();
            Lobbies.Remove(lobby);
            LobbiesChanged?.Invoke();
        }

        public async Task AuthorizeAsync()
        {
            if (TrustedClient.AccessToken == null)
            {
                await TrustedClient.SignInWithServiceAccount(CloudProjectId, EnvironmentId);
            }
        }
    }
}
