using System;
using System.Collections.Generic;
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
    class GameClient : IGameClient
    {
        const string AnalyticsUserIdKey = "analytics-user-id";
        const string InstallationIdKey = "installation-id";

        public event Action AccessTokenChanged;

        public IAnalyticsApi Analytics { get; }
        public ICloudCodeApi CloudCode { get; }
        public ICloudSaveDataApi CloudSaveData { get; }
        public ICloudSaveFilesApi CloudSaveFiles { get; }
        public IEconomyConfigurationApi EconomyConfiguration { get; }
        public IEconomyCurrenciesApi EconomyCurrencies { get; }
        public IEconomyInventoryApi EconomyInventory { get; }
        public IEconomyPurchasesApi EconomyPurchases { get; }
        public IFriendsMessagingApi FriendsMessaging { get; }
        public IFriendsNotificationsApi FriendsNotifications { get; }
        public IFriendsPresenceApi FriendsPresence { get; }
        public IFriendsRelationshipsApi FriendsRelationships { get; }
        public ILeaderboardsApi Leaderboards { get; }
        public ILobbyApi Lobby { get; }
        public IPlayerAuthenticationApi PlayerAuthentication { get; }
        public IPlayerNamesApi PlayerNames { get; }
        public IQosDiscoveryApi QosDiscovery { get; }
        public IRelayAllocationsApi RelayAllocations { get; }
        public IRemoteConfigSettingsApi RemoteConfig { get; }

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
        public ICCDContentApi CCDContent { get; }
        public ICCDEntriesApi CCDEntries { get; }
        public ICCDReleasesApi CCDReleases { get; }

        public IMatchmakerTicketsApi MatchmakerTickets { get; }

        public IUGCAuthApi UGCAuth { get; }
        public IUGCContentApi UGCContent { get; }
        public IUGCContentVersionsApi UGCContentVersions { get; }
        public IUGCModerationApi UGCModeration { get; }
        public IUGCPlayerApi UGCPlayer { get; }
        public IUGCProjectApi UGCProject { get; }
        public IUGCRepresentationApi UGCRepresentation { get; }
        public IUGCSubscriptionApi UGCSubscription { get; }
        public IUGCTagApi UGCTag { get; }
#endif

        public string AccessToken { get; private set; }

        internal List<IApiAccessor> Apis { get; }

        internal GameClient(
            IAnalyticsApi analytics,
            ICloudCodeApi cloudCode,
            ICloudSaveDataApi cloudSaveData,
            ICloudSaveFilesApi cloudSaveFiles,
            IEconomyConfigurationApi economyConfiguration,
            IEconomyCurrenciesApi economyCurrencies,
            IEconomyInventoryApi economyInventory,
            IEconomyPurchasesApi economyPurchases,
            IFriendsMessagingApi friendsMessaging,
            IFriendsNotificationsApi friendsNotifications,
            IFriendsPresenceApi friendsPresence,
            IFriendsRelationshipsApi friendsRelationships,
            ILeaderboardsApi leaderboards,
            ILobbyApi lobby,
            IPlayerAuthenticationApi playerAuthentication,
            IPlayerNamesApi playerNames,
            IQosDiscoveryApi qosDiscovery,
            IRelayAllocationsApi relayAllocations,
            IRemoteConfigSettingsApi remoteConfig
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
            ,
            ICCDContentApi ccdContent,
            ICCDEntriesApi ccdEntries,
            ICCDReleasesApi ccdReleases,
            IMatchmakerTicketsApi matchmakerTickets,
            IUGCAuthApi ugcAuth,
            IUGCContentApi ugcContent,
            IUGCContentVersionsApi ugcContentVersions,
            IUGCModerationApi ugcModeration,
            IUGCPlayerApi ugcPlayer,
            IUGCProjectApi ugcProject,
            IUGCRepresentationApi ugcRepresentation,
            IUGCSubscriptionApi ugcSubscription,
            IUGCTagApi ugcTag
#endif
        )
        {
            Analytics = analytics;
            CloudCode = cloudCode;
            CloudSaveData = cloudSaveData;
            CloudSaveFiles = cloudSaveFiles;
            EconomyConfiguration = economyConfiguration;
            EconomyCurrencies = economyCurrencies;
            EconomyInventory = economyInventory;
            EconomyPurchases = economyPurchases;
            Leaderboards = leaderboards;
            Lobby = lobby;
            PlayerAuthentication = playerAuthentication;
            PlayerNames = playerNames;
            QosDiscovery = qosDiscovery;
            RelayAllocations = relayAllocations;
            RemoteConfig = remoteConfig;
            FriendsMessaging = friendsMessaging;
            FriendsNotifications = friendsNotifications;
            FriendsPresence = friendsPresence;
            FriendsRelationships = friendsRelationships;
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
            CCDContent = ccdContent;
            CCDEntries = ccdEntries;
            CCDReleases = ccdReleases;
            MatchmakerTickets = matchmakerTickets;
            UGCAuth = ugcAuth;
            UGCContent = ugcContent;
            UGCContentVersions = ugcContentVersions;
            UGCModeration = ugcModeration;
            UGCPlayer = ugcPlayer;
            UGCProject = ugcProject;
            UGCRepresentation = ugcRepresentation;
            UGCSubscription = ugcSubscription;
            UGCTag = ugcTag;
#endif

            Apis = new List<IApiAccessor>()
            {
                Analytics,
                CloudCode,
                CloudSaveData,
                CloudSaveFiles,
                EconomyConfiguration,
                EconomyCurrencies,
                EconomyInventory,
                EconomyPurchases,
                Leaderboards,
                Lobby,
                PlayerAuthentication,
                PlayerNames,
                QosDiscovery,
                RelayAllocations,
                RemoteConfig,
                FriendsMessaging,
                FriendsNotifications,
                FriendsPresence,
                FriendsRelationships,
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
                CCDContent,
                CCDEntries,
                CCDReleases,
                MatchmakerTickets,
                UGCAuth,
                UGCContent,
                UGCContentVersions,
                UGCModeration,
                UGCPlayer,
                UGCProject,
                UGCRepresentation,
                UGCSubscription,
                UGCTag
#endif
            };
        }

        public static IGameClient Create(IApiClient apiClient = null)
        {
            if (apiClient == null)
            {
                apiClient = new ApiClient();
            }

            return new GameClient(
                new AnalyticsApi(apiClient),
                new CloudCodeApi(apiClient),
                new CloudSaveDataApi(apiClient),
                new CloudSaveFilesApi(apiClient),
                new EconomyConfigurationApi(apiClient),
                new EconomyCurrenciesApi(apiClient),
                new EconomyInventoryApi(apiClient),
                new EconomyPurchasesApi(apiClient),
                new FriendsMessagingApi(apiClient),
                new FriendsNotificationsApi(apiClient),
                new FriendsPresenceApi(apiClient),
                new FriendsRelationshipsApi(apiClient),
                new LeaderboardsApi(apiClient),
                new LobbyApi(apiClient),
                new PlayerAuthenticationApi(apiClient),
                new PlayerNamesApi(apiClient),
                new QosDiscoveryApi(apiClient),
                new RelayAllocationsApi(apiClient),
                new RemoteConfigSettingsApi(apiClient)
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
                ,
                new CCDContentApi(apiClient),
                new CCDEntriesApi(apiClient),
                new CCDReleasesApi(apiClient),
                new MatchmakerTicketsApi(apiClient),
                new UGCAuthApi(apiClient),
                new UGCContentApi(apiClient),
                new UGCContentVersionsApi(apiClient),
                new UGCModerationApi(apiClient),
                new UGCPlayerApi(apiClient),
                new UGCProjectApi(apiClient),
                new UGCRepresentationApi(apiClient),
                new UGCSubscriptionApi(apiClient),
                new UGCTagApi(apiClient)
#endif
            );
        }

        public ApiOperation<AuthenticationResponse> SignUpAnonymously(
            string projectId,
            string environment = default,
            SignUpAnonymouslyRequest request = null,
            CancellationToken token = default)
        {
            var operation = PlayerAuthentication.SignUpAnonymously(projectId, environment, request, token);

            operation.Completed += (response) =>
            {
                if (response.IsSuccessful)
                {
                    SetAccessToken(response.Data.IdToken);
                }
            };

            return operation;
        }

        public ApiOperation<AuthenticationResponse> SignInWithSessionToken(
            string projectId,
            SignInWithSessionTokenRequest request,
            string environment = default,
            CancellationToken token = default)
        {
            var operation = PlayerAuthentication.SignInWithSessionToken(projectId, request, environment, token);

            operation.Completed += (response) =>
            {
                if (response.IsSuccessful)
                {
                    SetAccessToken(response.Data.IdToken);
                }
            };

            return operation;
        }

        public ApiOperation<AuthenticationResponse> SignInWithExternalToken(
            string projectId,
            string idProvider,
            SignInWithExternalTokenRequest request,
            string environment = default,
            CancellationToken token = default)
        {
            var operation = PlayerAuthentication.SignInWithExternalToken(idProvider, projectId, request, environment, token);

            operation.Completed += (response) =>
            {
                if (response.IsSuccessful)
                {
                    SetAccessToken(response.Data.IdToken);
                }
            };

            return operation;
        }

        public ApiOperation<AuthenticationResponse> SignInWithCode(
            string projectId,
            SignInWithCodeRequest request,
            string environment = default,
            string codeLinkSessionId = default,
            CancellationToken token = default)
        {
            var operation = PlayerAuthentication.SignInWithCode(projectId, codeLinkSessionId, request, environment, token);

            operation.Completed += (response) =>
            {
                if (response.IsSuccessful)
                {
                    SetAccessToken(response.Data.IdToken);
                }
            };

            return operation;
        }

        public ApiOperation<AuthenticationResponse> SignInWithUsernamePassword(
            string projectId,
            UsernamePasswordRequest request,
            string environment = null,
            CancellationToken token = default)
        {
            var operation = PlayerAuthentication.SignInWithUsernamePassword(projectId, request, environment, token);

            operation.Completed += (response) =>
            {
                if (response.IsSuccessful)
                {
                    SetAccessToken(response.Data.IdToken);
                }
            };

            return operation;
        }

        public ApiOperation<AuthenticationResponse> SignUpWithUsernamePassword(
            string projectId,
            UsernamePasswordRequest request,
            string environment = null,
            CancellationToken token = default)
        {
            var operation = PlayerAuthentication.SignUpWithUsernamePassword(projectId, request, environment, null, token);

            operation.Completed += (response) =>
            {
                if (response.IsSuccessful)
                {
                    SetAccessToken(response.Data.IdToken);
                }
            };

            return operation;
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

        public void SetAnalyticsUserId(string analyticsUserId)
        {
            foreach (var api in Apis)
            {
                api.Configuration.DefaultHeaders[AnalyticsUserIdKey] = analyticsUserId;
            }
        }

        public void ClearAnalyticsUserId()
        {
            foreach (var api in Apis)
            {
                api.Configuration.DefaultHeaders.Remove(AnalyticsUserIdKey);
            }
        }

        public void SetInstallationId(string installationId)
        {
            foreach (var api in Apis)
            {
                api.Configuration.DefaultHeaders[InstallationIdKey] = installationId;
            }
        }

        public void ClearInstallationId()
        {
            foreach (var api in Apis)
            {
                api.Configuration.DefaultHeaders.Remove(InstallationIdKey);
            }
        }
    }
}
