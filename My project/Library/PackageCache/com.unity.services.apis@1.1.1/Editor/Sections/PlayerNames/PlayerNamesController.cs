using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Apis.PlayerNames;

namespace Unity.Services.Apis.Sample
{
    class PlayerNamesController : ApiController
    {
        public event Action OnPlayersChanged;

        public string CurrentPlayerId { get; set; }
        public Dictionary<string, Player> Players { get; set; } = new Dictionary<string, Player>();

        ITrustedClient TrustedClient;
        IPlayerNamesApi PlayerNames => TrustedClient.PlayerNames;

        public PlayerNamesController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(ITrustedClient trustedClient)
        {
            TrustedClient = trustedClient;
        }

        public async Task FetchPlayerNameAsync()
        {
            await AuthorizeAsync();
            var response = await PlayerNames.GetName(CurrentPlayerId);
            response.EnsureSuccessful();
            Players[CurrentPlayerId] = response.Data;
            OnPlayersChanged?.Invoke();
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player.Id);
            OnPlayersChanged?.Invoke();
        }

        async Task AuthorizeAsync()
        {
            if (TrustedClient.AccessToken == null)
            {
                await TrustedClient.SignInWithServiceAccount(CloudProjectId, EnvironmentId);
            }
        }
    }
}
