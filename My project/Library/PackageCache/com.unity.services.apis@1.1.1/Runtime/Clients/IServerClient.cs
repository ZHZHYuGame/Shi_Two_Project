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
    /// <summary>
    /// The Server Client provides functionality for servers that want to use Unity Services.
    /// Authorization can be done through either Service Account authentication or running within a Hosted Game Server.
    /// </summary>
    public interface IServerClient
    {
        /// <summary>
        /// CloudCode Api
        /// </summary>
        ICloudCodeApi CloudCode { get; }

        /// <summary>
        /// CloudSave Player Data Api
        /// </summary>
        ICloudSaveDataApi CloudSaveData { get; }

        /// <summary>
        /// CloudSave Files Api
        /// </summary>
        ICloudSaveFilesApi CloudSaveFiles { get; }

        /// <summary>
        /// Economy Configuration Api
        /// </summary>
        IEconomyConfigurationApi EconomyConfiguration { get; }

        /// <summary>
        /// Economy Currencies Api
        /// </summary>
        IEconomyCurrenciesApi EconomyCurrencies { get; }

        /// <summary>
        /// Economy Inventory Api
        /// </summary>
        IEconomyInventoryApi EconomyInventory { get; }

        /// <summary>
        /// Economy Purchases Api
        /// </summary>
        IEconomyPurchasesApi EconomyPurchases { get; }

        /// <summary>
        /// Leaderboards Api
        /// </summary>
        ILeaderboardsApi Leaderboards { get; }

        /// <summary>
        /// Lobby Api
        /// </summary>
        ILobbyApi Lobby { get; }

        /// <summary>
        /// Player Names Api
        /// </summary>
        IPlayerNamesApi PlayerNames { get; }

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
        /// <summary>
        /// Matchmaker Backfill Api
        /// </summary>
        IMatchmakerBackfillApi MatchmakerBackfill { get; }

        /// <summary>
        /// Matchmaker Tickets Api
        /// </summary>
        IMatchmakerTicketsApi MatchmakerTickets { get; }

        /// <summary>
        /// Multiplay Allocations Api
        /// </summary>
        IMultiplayAllocationsApi MultiplayAllocations { get; }

        /// <summary>
        /// Multiplay Fleets Api
        /// </summary>
        IMultiplayFleetsApi MultiplayFleets { get; }
#endif

        /// <summary>
        /// The access token applied to apis.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Retrieve a token to authorize server operations from a hosted server.
        /// Must be running on a multiplay server or with the server local proxy activated.
        /// </summary>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The operation</returns>
        ApiOperation<TokenResponse> SignInFromServer(CancellationToken token = default);

        /// <summary>
        /// Exchange service account credentials for an access token to use with game apis.
        /// * IMPORTANT *
        /// Always protect your service account credentials.
        /// Make sure service account information is never used in a game client.
        /// </summary>
        /// <param name="projectId">The cloud project id</param>
        /// <param name="environmentId">The environment id</param>
        /// <param name="scopes">The permission scopes</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The operation</returns>
        ApiOperation<ExchangeResponse> SignInWithServiceAccount(string projectId, string environmentId, List<string> scopes = default, CancellationToken token = default);

        /// <summary>
        /// Set the service account that the apis will use.
        /// </summary>
        /// <param name="apiKey">The service account key</param>
        /// <param name="apiSecret">The service account secret</param>
        void SetServiceAccount(string apiKey, string apiSecret);

        /// <summary>
        /// Apply the access token to the apis
        /// </summary>
        /// <param name="accessToken">The access token</param>
        void SetAccessToken(string accessToken);

        /// <summary>
        /// Clears the credentials from the configuration of all apis
        /// </summary>
        void ClearCredentials();
    }
}
#endif
