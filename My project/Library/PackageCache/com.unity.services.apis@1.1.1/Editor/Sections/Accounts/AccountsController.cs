using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.PlayerAuthentication;
using Unity.Services.Apis.PlayerAuthentication;

namespace Unity.Services.Apis.Sample
{
    class AccountsController : ApiController
    {
        IPlayerAuthenticationApi PlayerAuth { get; set; }
        IPlayerAuthenticationAdminApi PlayerAuthAdmin { get; set; }

        public event Action PlayersChanged;
        public PlayerAuthListProjectUserResponse Players { get; private set; }

        public AccountsController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient, IGameClient gameClient)
        {
            PlayerAuth = gameClient.PlayerAuthentication;
            PlayerAuthAdmin = adminClient.PlayerAuthentication;
        }

        public async Task ListPlayersAsync()
        {
            var response = await PlayerAuthAdmin.ListPlayers(CloudProjectId);
            response.EnsureSuccessful();
            Players = response.Data;
            PlayersChanged?.Invoke();
        }

        public void ClearPlayers()
        {
            Players = null;
            PlayersChanged?.Invoke();
        }

        public async Task<AuthenticationResponse> SignUpAnonymouslyAsync()
        {
            var body = new SignUpAnonymouslyRequest();
            var response = await PlayerAuth.SignUpAnonymously(CloudProjectId, EnvironmentName, body);
            response.EnsureSuccessful();
            return response.Data;
        }

        public async Task DeleteAsync(string playerId)
        {
            var response = await PlayerAuthAdmin.DeletePlayer(playerId, CloudProjectId);
            response.EnsureSuccessful();

            if (!response.IsSuccessful)
            {
                throw new Exception(response.ErrorText);
            }
        }
    }
}
