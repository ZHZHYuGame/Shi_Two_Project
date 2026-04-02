#if UNITY_EDITOR || ENABLE_RUNTIME_TRUSTED_APIS
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Services.Apis.Admin.ServiceAuthentication;
using Unity.Services.Apis.CloudCode;
using Unity.Services.Apis.CloudSave;
using Unity.Services.Apis.Economy;
using Unity.Services.Apis.Leaderboards;
using Unity.Services.Apis.Lobbies;
using Unity.Services.Apis.Multiplay;
using Unity.Services.Apis.PlayerAuthentication;
using Unity.Services.Apis.PlayerNames;

namespace Unity.Services.Apis
{
    class TrustedClient : ITrustedClient
    {
        public event Action AccessTokenChanged;

        public ICloudCodeApi CloudCode { get; }
        public ICloudSaveDataApi CloudSaveData { get; }
        public ICloudSaveFilesApi CloudSaveFiles { get; }
        public IEconomyConfigurationApi EconomyConfiguration { get; }
        public IEconomyCurrenciesApi EconomyCurrencies { get; }
        public IEconomyInventoryApi EconomyInventory { get; }
        public IEconomyPurchasesApi EconomyPurchases { get; }
        public ILeaderboardsApi Leaderboards { get; }
        public ILobbyApi Lobby { get; }
        public IMultiplayAllocationsApi MultiplayAllocations { get; }
        public IMultiplayFleetsApi MultiplayFleets { get; }
        public IPlayerAuthenticationApi PlayerAuthentication { get; }
        public IPlayerNamesApi PlayerNames { get; }

        public string AccessToken { get; private set; }

        internal IServiceAuthenticationAdminApi ServiceAuthentication { get; }
        internal List<IApiAccessor> Apis { get; }

        internal TrustedClient(
            IServiceAuthenticationAdminApi serviceAuthenticationApi,
            ICloudCodeApi cloudCodeApi,
            ICloudSaveDataApi cloudSaveDataApi,
            ICloudSaveFilesApi cloudSaveFilesApi,
            IEconomyConfigurationApi economyConfigurationApi,
            IEconomyCurrenciesApi economyCurrenciesApi,
            IEconomyInventoryApi economyInventoryApi,
            IEconomyPurchasesApi economyPurchasesApi,
            ILeaderboardsApi leaderboardsApi,
            ILobbyApi lobbyApi,
            IMultiplayAllocationsApi multiplayAllocationsApi,
            IMultiplayFleetsApi multiplayFleetsApi,
            IPlayerAuthenticationApi playerAuthenticationApi,
            IPlayerNamesApi playerNamesApi)
        {
            ServiceAuthentication = serviceAuthenticationApi;
            CloudCode = cloudCodeApi;
            CloudSaveData = cloudSaveDataApi;
            CloudSaveFiles = cloudSaveFilesApi;
            EconomyConfiguration = economyConfigurationApi;
            EconomyCurrencies = economyCurrenciesApi;
            EconomyInventory = economyInventoryApi;
            EconomyPurchases = economyPurchasesApi;
            Leaderboards = leaderboardsApi;
            Lobby = lobbyApi;
            MultiplayAllocations = multiplayAllocationsApi;
            MultiplayFleets = multiplayFleetsApi;
            PlayerAuthentication = playerAuthenticationApi;
            PlayerNames = playerNamesApi;

            Apis = new List<IApiAccessor>()
            {
                CloudCode,
                CloudSaveData,
                CloudSaveFiles,
                EconomyConfiguration,
                EconomyCurrencies,
                EconomyInventory,
                EconomyPurchases,
                Leaderboards,
                Lobby,
                MultiplayAllocations,
                MultiplayFleets,
                PlayerAuthentication,
                PlayerNames
            };
        }

        public static ITrustedClient Create(IApiClient apiClient = null)
        {
            if (apiClient == null)
            {
                apiClient = new ApiClient();
            }

            return new TrustedClient(
                new ServiceAuthenticationAdminApi(apiClient),
                new CloudCodeApi(apiClient),
                new CloudSaveDataApi(apiClient),
                new CloudSaveFilesApi(apiClient),
                new EconomyConfigurationApi(apiClient),
                new EconomyCurrenciesApi(apiClient),
                new EconomyInventoryApi(apiClient),
                new EconomyPurchasesApi(apiClient),
                new LeaderboardsApi(apiClient),
                new LobbyApi(apiClient),
                new MultiplayAllocationsApi(apiClient),
                new MultiplayFleetsApi(apiClient),
                new PlayerAuthenticationApi(apiClient),
                new PlayerNamesApi(apiClient));
        }

        public ApiOperation<ExchangeResponse> SignInWithServiceAccount(string projectId, string environmentId, List<string> scopes = default, CancellationToken token = default)
        {
            var request = new ExchangeRequest(scopes);
            var operation = ServiceAuthentication.ExchangeToStateless(projectId, environmentId, request, token);

            operation.Completed += (response) =>
            {
                if (response.IsSuccessful)
                {
                    SetAccessToken(response.Data.AccessToken);
                }
            };

            return operation;
        }

        public void SetServiceAccount(string apiKey, string apiSecret)
        {
            ServiceAuthentication.Configuration.Username = apiKey;
            ServiceAuthentication.Configuration.Password = apiSecret;
        }

        public void SetAccessToken(string accessToken)
        {
            AccessToken = accessToken;

            foreach (var api in Apis)
            {
                api.Configuration.AccessToken = accessToken;
            }

            AccessTokenChanged?.Invoke();
        }

        public void ClearCredentials()
        {
            AccessToken = null;

            foreach (var api in Apis)
            {
                api.Configuration.AccessToken = null;
            }

            AccessTokenChanged?.Invoke();
        }
    }
}
#endif
