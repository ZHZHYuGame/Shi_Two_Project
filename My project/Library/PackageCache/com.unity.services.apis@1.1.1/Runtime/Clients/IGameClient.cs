using System;
using System.Threading;
using Unity.Services.Apis.Analytics;
using Unity.Services.Apis.CloudCode;
using Unity.Services.Apis.CloudSave;
using Unity.Services.Apis.Economy;
using Unity.Services.Apis.Friends;
using Unity.Services.Apis.Leaderboards;
using Unity.Services.Apis.Lobbies;
using Unity.Services.Apis.PlayerAuthentication;
using Unity.Services.Apis.PlayerNames;
using Unity.Services.Apis.QoS;
using Unity.Services.Apis.Relay;
using Unity.Services.Apis.RemoteConfig;

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using Unity.Services.Apis.CloudContentDelivery;
using Unity.Services.Apis.Matchmaker;
using Unity.Services.Apis.UGC;
#endif

namespace Unity.Services.Apis
{
    /// <summary>
    /// The Game Client offers apis to achieve player-scale outcomes.
    /// It relies on player authentication to authorize most api calls.
    /// </summary>
    public interface IGameClient
    {
        /// <summary>
        /// Event raised when the access token changed.
        /// </summary>
        event Action AccessTokenChanged;

        /// <summary>
        /// Analytics Api
        /// </summary>
        IAnalyticsApi Analytics { get; }

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
        /// Friends Messaging Api
        /// </summary>
        IFriendsMessagingApi FriendsMessaging { get; }

        /// <summary>
        /// Friends Notifications Api
        /// </summary>
        IFriendsNotificationsApi FriendsNotifications { get; }

        /// <summary>
        /// Friends Presence Api
        /// </summary>
        IFriendsPresenceApi FriendsPresence { get; }

        /// <summary>
        /// Friends Relationships Api
        /// </summary>
        IFriendsRelationshipsApi FriendsRelationships { get; }

        /// <summary>
        /// Leaderboards Api
        /// </summary>
        ILeaderboardsApi Leaderboards { get; }

        /// <summary>
        /// Lobby Api
        /// </summary>
        ILobbyApi Lobby { get; }

        /// <summary>
        /// Player Authentication Api
        /// </summary>
        IPlayerAuthenticationApi PlayerAuthentication { get; }

        /// <summary>
        /// Player Names Api
        /// </summary>
        IPlayerNamesApi PlayerNames { get; }

        /// <summary>
        /// Qos Discovery Api
        /// </summary>
        IQosDiscoveryApi QosDiscovery { get; }

        /// <summary>
        /// Relay Allocations Api
        /// </summary>
        IRelayAllocationsApi RelayAllocations { get; }

        /// <summary>
        /// Remote Config Api
        /// </summary>
        IRemoteConfigSettingsApi RemoteConfig { get; }

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
        /// <summary>
        /// CCD Content Api
        /// </summary>
        ICCDContentApi CCDContent { get; }

        /// <summary>
        /// CCD Entries Api
        /// </summary>
        ICCDEntriesApi CCDEntries { get; }

        /// <summary>
        /// CCD Releases Api
        /// </summary>
        ICCDReleasesApi CCDReleases { get; }

        /// <summary>
        /// Matchmaker Tickets Api
        /// </summary>
        IMatchmakerTicketsApi MatchmakerTickets { get; }

        /// <summary>
        /// UGC Auth Api
        /// </summary>
        IUGCAuthApi UGCAuth { get; }

        /// <summary>
        /// UGC Content Api
        /// </summary>
        IUGCContentApi UGCContent { get; }

        /// <summary>
        /// UGC Content Versions Api
        /// </summary>
        IUGCContentVersionsApi UGCContentVersions { get; }

        /// <summary>
        /// UGC Moderation Api
        /// </summary>
        IUGCModerationApi UGCModeration { get; }

        /// <summary>
        /// UGC Player Api
        /// </summary>
        IUGCPlayerApi UGCPlayer { get; }

        /// <summary>
        /// UGC Project Api
        /// </summary>
        IUGCProjectApi UGCProject { get; }

        /// <summary>
        /// UGC Representation Api
        /// </summary>
        IUGCRepresentationApi UGCRepresentation { get; }

        /// <summary>
        /// UGC Subscription Api
        /// </summary>
        IUGCSubscriptionApi UGCSubscription { get; }

        /// <summary>
        /// UGC Tag Api
        /// </summary>
        IUGCTagApi UGCTag { get; }
#endif

        /// <summary>
        /// The access token applied to apis.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Create a new anonymous player account
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="environment">The environment name</param>
        /// <param name="request">The request options</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The operation</returns>
        ApiOperation<AuthenticationResponse> SignUpAnonymously(
            string projectId,
            string environment = default,
            SignUpAnonymouslyRequest request = null,
            CancellationToken token = default);

        /// <summary>
        /// Sign in using a session token
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="request">The request options</param>
        /// <param name="environment">The environment name</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The operation</returns>
        ApiOperation<AuthenticationResponse> SignInWithSessionToken(
            string projectId,
            SignInWithSessionTokenRequest request,
            string environment = default,
            CancellationToken token = default);

        /// <summary>
        /// Sign in using a token obtained from a supported external identity provider.
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="idProvider">The key for the identity provider</param>
        /// <param name="request">The request options</param>
        /// <param name="environment">The environment name</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The operation</returns>
        ApiOperation<AuthenticationResponse> SignInWithExternalToken(
            string projectId,
            string idProvider,
            SignInWithExternalTokenRequest request,
            string environment = default,
            CancellationToken token = default);

        /// <summary>
        /// Sign in using a code obtained from requesting a code.
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="request">The request options</param>
        /// <param name="environment">The environment name</param>
        /// <param name="codeLinkSessionId">The code link session id</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The operation</returns>
        ApiOperation<AuthenticationResponse> SignInWithCode(
            string projectId,
            SignInWithCodeRequest request,
            string environment = default,
            string codeLinkSessionId = default,
            CancellationToken token = default);

        /// <summary>
        /// Sign in using a username and password
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="request">The request options</param>
        /// <param name="environment">The environment name</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The operation</returns>
        ApiOperation<AuthenticationResponse> SignInWithUsernamePassword(
            string projectId,
            UsernamePasswordRequest request,
            string environment = null,
            CancellationToken token = default);

        /// <summary>
        /// Sign up using a username and password
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="request">The request options</param>
        /// <param name="environment">The environment name</param>
        /// <param name="token">The optional cancellation token</param>
        /// <returns>The operation</returns>
        ApiOperation<AuthenticationResponse> SignUpWithUsernamePassword(
            string projectId,
            UsernamePasswordRequest request,
            string environment = null,
            CancellationToken token = default);

        /// <summary>
        /// Sets the player access token for all apis.
        /// </summary>
        /// <param name="accessToken">The access token</param>
        void SetAccessToken(string accessToken);

        /// <summary>
        /// Sets the analytics user id into the headers of the supported services
        /// </summary>
        /// <param name="analyticsUserId">The analytics user id</param>
        void SetAnalyticsUserId(string analyticsUserId);

        /// <summary>
        /// Clears the analytics user id from the headers of the supported services
        /// </summary>
        void ClearAnalyticsUserId();

        /// <summary>
        /// Sets the installation id into the headers of the supported services
        /// </summary>
        /// <param name="installationId">The installation id</param>
        void SetInstallationId(string installationId);

        /// <summary>
        /// Clears the installation id from the headers of the supported services
        /// </summary>
        void ClearInstallationId();

        /// <summary>
        /// Clears the credentials from the configuration of all apis
        /// </summary>
        void ClearCredentials();
    }
}
