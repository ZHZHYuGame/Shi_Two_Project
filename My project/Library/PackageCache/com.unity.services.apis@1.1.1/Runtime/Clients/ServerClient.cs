#if UNITY_EDITOR || UNITY_SERVER || ENABLE_SERVER_APIS
using System.Collections.Generic;
using System.Threading;
using Unity.Services.Apis.Admin.ServiceAuthentication;
using Unity.Services.Apis.CloudCode;
using Unity.Services.Apis.CloudSave;
using Unity.Services.Apis.Economy;
using Unity.Services.Apis.Leaderboards;
using Unity.Services.Apis.Lobbies;
using Unity.Services.Apis.PlayerNames;
using Unity.Services.Apis.Proxy;

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using Unity.Services.Apis.Matchmaker;
using Unity.Services.Apis.Multiplay;
#endif

namespace Unity.Services.Apis
{
    class ServerClient : IServerClient
    {
        public ICloudCodeApi CloudCode { get; }
        public ICloudSaveDataApi CloudSaveData { get; }
        public ICloudSaveFilesApi CloudSaveFiles { get; }
        public IEconomyConfigurationApi EconomyConfiguration { get; }
        public IEconomyCurrenciesApi EconomyCurrencies { get; }
        public IEconomyInventoryApi EconomyInventory { get; }
        public IEconomyPurchasesApi EconomyPurchases { get; }
        public ILeaderboardsApi Leaderboards { get; }
        public ILobbyApi Lobby { get; }
        public IPlayerNamesApi PlayerNames { get; }
        internal IProxyApi Proxy { get; }

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
        public IMatchmakerBackfillApi MatchmakerBackfill { get; }
        public IMatchmakerTicketsApi MatchmakerTickets { get; }
        public IMultiplayAllocationsApi MultiplayAllocations { get; }
        public IMultiplayFleetsApi MultiplayFleets { get; }
#endif

        public string AccessToken { get; private set; }

        internal IServiceAuthenticationAdminApi ServiceAuthentication { get; }

        internal List<IApiAccessor> Apis { get; }

        internal ServerClient(
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
            IPlayerNamesApi playerNamesApi,
            IProxyApi proxyApi
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
            ,
            IMatchmakerBackfillApi matchmakerBackfillApi,
            IMatchmakerTicketsApi matchmakerTicketsApi,
            IMultiplayAllocationsApi multiplayAllocationsApi,
            IMultiplayFleetsApi multiplayFleetsApi
#endif
            )
        {
            // Only for authorization
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
            PlayerNames = playerNamesApi;
            Proxy = proxyApi;

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
            MatchmakerBackfill = matchmakerBackfillApi;
            MatchmakerTickets = matchmakerTicketsApi;
            MultiplayAllocations = multiplayAllocationsApi;
            MultiplayFleets = multiplayFleetsApi;
#endif

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
                PlayerNames,
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
                MatchmakerBackfill,
                MatchmakerTickets,
                MultiplayAllocations,
                MultiplayFleets,
#endif
            };
        }

        public static IServerClient Create(IApiClient apiClient = null)
        {
            if (apiClient == null)
            {
                apiClient = new ApiClient();
            }

            return new ServerClient(
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
                new PlayerNamesApi(apiClient),
                new ProxyApi(apiClient)
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
                ,
                new MatchmakerBackfillApi(apiClient),
                new MatchmakerTicketsApi(apiClient),
                new MultiplayAllocationsApi(apiClient),
                new MultiplayFleetsApi(apiClient)
#endif
                );
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

        public ApiOperation<TokenResponse> SignInFromServer(CancellationToken token = default)
        {
            var operation = Proxy.GetToken(token);

            operation.Completed += (response) =>
            {
                if (response.IsSuccessful)
                {
                    SetAccessToken(response.Data.Token);
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
        }

        public void ClearCredentials()
        {
            AccessToken = null;

            foreach (var api in Apis)
            {
                api.Configuration.AccessToken = null;
            }
        }
    }
}
#endif
