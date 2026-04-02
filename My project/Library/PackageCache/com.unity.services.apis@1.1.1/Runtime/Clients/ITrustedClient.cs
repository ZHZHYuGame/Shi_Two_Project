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
    /// <summary>
    /// The Trusted Client provides management for game apis using trusted authorization.
    /// Authorization relies on Service Account authentication.
    /// </summary>
    public interface ITrustedClient
    {
        /// <summary>
        /// Event raised when the access token changed.
        /// </summary>
        event Action AccessTokenChanged;

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
        /// EconomyConfiguration Api
        /// </summary>
        IEconomyConfigurationApi EconomyConfiguration { get; }

        /// <summary>
        /// EconomyCurrencies Api
        /// </summary>
        IEconomyCurrenciesApi EconomyCurrencies { get; }

        /// <summary>
        /// EconomyInventory Api
        /// </summary>
        IEconomyInventoryApi EconomyInventory { get; }

        /// <summary>
        /// EconomyPurchases Api
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
        /// Multiplay Allocations Api
        /// </summary>
        IMultiplayAllocationsApi MultiplayAllocations { get; }

        /// <summary>
        /// Multiplay Fleets Api
        /// </summary>
        IMultiplayFleetsApi MultiplayFleets { get; }

        /// <summary>
        /// PlayerAuthentication Api
        /// </summary>
        IPlayerAuthenticationApi PlayerAuthentication { get; }

        /// <summary>
        /// Player Names Api
        /// </summary>
        IPlayerNamesApi PlayerNames { get; }

        /// <summary>
        /// The access token applied to apis.
        /// </summary>
        string AccessToken { get; }

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
        /// Apply the trusted access token to the apis
        /// </summary>
        /// <param name="accessToken">The trusted access token</param>
        void SetAccessToken(string accessToken);

        /// <summary>
        /// Clears the credentials from the configuration of all apis
        /// </summary>
        void ClearCredentials();
    }
}
#endif
