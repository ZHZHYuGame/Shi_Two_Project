#if UNITY_EDITOR || ENABLE_RUNTIME_ADMIN_APIS
using System.Collections.Generic;
using Unity.Services.Apis.Admin.AccessPolicy;
using Unity.Services.Apis.Admin.CloudCode;
using Unity.Services.Apis.Admin.CloudSave;
using Unity.Services.Apis.Admin.Economy;
using Unity.Services.Apis.Admin.Environment;
using Unity.Services.Apis.Admin.Leaderboards;
using Unity.Services.Apis.Admin.Observability;
using Unity.Services.Apis.Admin.PlayerAuthentication;
using Unity.Services.Apis.Admin.RemoteConfig;
using Unity.Services.Apis.Admin.Scheduler;
using Unity.Services.Apis.Admin.ServiceAuthentication;
using Unity.Services.Apis.Admin.Triggers;

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using Unity.Services.Apis.Admin.CloudContentDelivery;
using Unity.Services.Apis.Admin.Matchmaker;
using Unity.Services.Apis.Admin.Multiplay;
using Unity.Services.Apis.Admin.UGC;
#endif

namespace Unity.Services.Apis
{
    class AdminClient : IAdminClient
    {
        public ICloudCodeModulesAdminApi CloudCodeModules { get; }
        public ICloudCodeScriptsAdminApi CloudCodeScripts { get; }
        public ICloudSaveDataAdminApi CloudSaveData { get; }
        public ICloudSaveFilesAdminApi CloudSaveFiles { get; }
        public IEconomyAdminApi Economy { get; }
        public IEnvironmentAdminApi Environment { get; }
        public IGameOverridesAdminApi GameOverrides { get; }
        public ILeaderboardsAdminApi Leaderboards { get; }
        public ILogsAdminApi Logs { get; }
        public IPlayerAuthenticationAdminApi PlayerAuthentication { get; }
        public IPlayerPolicyAdminApi PlayerPolicy { get; }
        public IProjectPolicyAdminApi ProjectPolicy { get; }
        public IConfigsAdminApi RemoteConfig { get; }
        public ISchemasAdminApi RemoteConfigSchemas { get; }
        public ISchedulerAdminApi Scheduler { get; }
        public IServiceAuthenticationAdminApi ServiceAuthentication { get; }
        public ITriggersAdminApi Triggers { get; }

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
        public ICCDBadgesAdminApi CCDBadges { get; }
        public ICCDBucketAccessTokensAdminApi CCDBucketAccessTokens { get; }
        public ICCDBucketsAdminApi CCDBuckets { get; }
        public ICCDContentAdminApi CCDContent { get; }
        public ICCDEntriesAdminApi CCDEntries { get; }
        public ICCDEnvironmentsAdminApi CCDEnvironments { get; }
        public ICCDOrgsAdminApi CCDOrgs { get; }
        public ICCDPermissionsAdminApi CCDPermissions { get; }
        public ICCDReleasesAdminApi CCDReleases { get; }

        public IMatchmakerAdminApi Matchmaker { get; }

        public IMultiplayAllocationsAdminApi MultiplayAllocations { get; }
        public IMultiplayBuildConfigurationsAdminApi MultiplayBuildConfigurations { get; }
        public IMultiplayBuildsAdminApi MultiplayBuilds { get; }
        public IMultiplayFleetsAdminApi MultiplayFleets { get; }
        public IMultiplayMachinesAdminApi MultiplayMachines { get; }
        public IMultiplayProvidersAdminApi MultiplayProviders { get; }
        public IMultiplayRegistryAdminApi MultiplayRegistry { get; }
        public IMultiplayServersAdminApi MultiplayServers { get; }

        public IUGCContentAdminApi UGCContent { get; }
        public IUGCContentVersionsAdminApi UGCContentVersions { get; }
        public IUGCModerationAdminApi UGCModeration { get; }
        public IUGCPlayerAdminApi UGCPlayer { get; }
        public IUGCProjectAdminApi UGCProject { get; }
        public IUGCRepresentationAdminApi UGCRepresentation { get; }
        public IUGCTagAdminApi UGCTag { get; }
        public IUGCWebhookAdminApi UGCWebhook { get; }
#endif

        internal List<IApiAccessor> Apis { get; }

        internal AdminClient(
            ICloudCodeModulesAdminApi cloudCodeModules,
            ICloudCodeScriptsAdminApi cloudCodeScripts,
            ICloudSaveDataAdminApi cloudSaveData,
            ICloudSaveFilesAdminApi cloudSaveFiles,
            IEconomyAdminApi economy,
            IEnvironmentAdminApi environment,
            IConfigsAdminApi remoteConfig,
            ISchemasAdminApi remoteConfigSchemas,
            IGameOverridesAdminApi gameOverrides,
            ILeaderboardsAdminApi leaderboards,
            ILogsAdminApi logs,
            IPlayerAuthenticationAdminApi playerAuth,
            IPlayerPolicyAdminApi playerPolicy,
            IProjectPolicyAdminApi projectPolicy,
            ISchedulerAdminApi scheduler,
            IServiceAuthenticationAdminApi serviceAuthentication,
            ITriggersAdminApi triggers
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
            ,
            ICCDBadgesAdminApi ccdBadges,
            ICCDBucketAccessTokensAdminApi ccdBucketAccessTokens,
            ICCDBucketsAdminApi ccdBuckets,
            ICCDContentAdminApi ccdContent,
            ICCDEntriesAdminApi ccdEntries,
            ICCDEnvironmentsAdminApi ccdEnvironments,
            ICCDOrgsAdminApi ccdOrgs,
            ICCDPermissionsAdminApi ccdPermissions,
            ICCDReleasesAdminApi ccdReleases,
            IMatchmakerAdminApi matchmaker,
            IMultiplayAllocationsAdminApi multiplayAllocations,
            IMultiplayBuildConfigurationsAdminApi multiplayBuildConfigurations,
            IMultiplayBuildsAdminApi multiplayBuilds,
            IMultiplayFleetsAdminApi multiplayFleets,
            IMultiplayMachinesAdminApi multiplayMachines,
            IMultiplayProvidersAdminApi multiplayProviders,
            IMultiplayRegistryAdminApi multiplayRegistry,
            IMultiplayServersAdminApi multiplayServers,
            IUGCContentAdminApi ugcContent,
            IUGCContentVersionsAdminApi ugcContentVersions,
            IUGCModerationAdminApi ugcModeration,
            IUGCPlayerAdminApi ugcPlayer,
            IUGCProjectAdminApi ugcProject,
            IUGCRepresentationAdminApi ugcRepresentation,
            IUGCTagAdminApi ugcTag,
            IUGCWebhookAdminApi ugcWebhook
#endif
            )
        {
            CloudCodeModules = cloudCodeModules;
            CloudCodeScripts = cloudCodeScripts;
            CloudSaveData = cloudSaveData;
            CloudSaveFiles = cloudSaveFiles;
            Economy = economy;
            Environment = environment;
            RemoteConfig = remoteConfig;
            RemoteConfigSchemas = remoteConfigSchemas;
            GameOverrides = gameOverrides;
            Leaderboards = leaderboards;
            Logs = logs;
            PlayerAuthentication = playerAuth;
            PlayerPolicy = playerPolicy;
            ProjectPolicy = projectPolicy;
            Scheduler = scheduler;
            ServiceAuthentication = serviceAuthentication;
            Triggers = triggers;
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
            CCDBadges = ccdBadges;
            CCDBucketAccessTokens = ccdBucketAccessTokens;
            CCDBuckets = ccdBuckets;
            CCDContent = ccdContent;
            CCDEntries = ccdEntries;
            CCDEnvironments = ccdEnvironments;
            CCDOrgs = ccdOrgs;
            CCDPermissions = ccdPermissions;
            CCDReleases = ccdReleases;
            Matchmaker = matchmaker;
            MultiplayAllocations = multiplayAllocations;
            MultiplayBuildConfigurations = multiplayBuildConfigurations;
            MultiplayBuilds = multiplayBuilds;
            MultiplayFleets = multiplayFleets;
            MultiplayMachines = multiplayMachines;
            MultiplayProviders = multiplayProviders;
            MultiplayRegistry = multiplayRegistry;
            MultiplayServers = multiplayServers;
            UGCContent = ugcContent;
            UGCContentVersions = ugcContentVersions;
            UGCModeration = ugcModeration;
            UGCPlayer = ugcPlayer;
            UGCProject = ugcProject;
            UGCRepresentation = ugcRepresentation;
            UGCTag = ugcTag;
            UGCWebhook = ugcWebhook;
#endif

            Apis = new List<IApiAccessor>()
            {
                CloudCodeModules,
                CloudCodeScripts,
                CloudSaveData,
                CloudSaveFiles,
                Economy,
                Environment,
                RemoteConfig,
                RemoteConfigSchemas,
                GameOverrides,
                Leaderboards,
                Logs,
                PlayerAuthentication,
                PlayerPolicy,
                ProjectPolicy,
                Scheduler,
                ServiceAuthentication,
                Triggers,
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
                CCDBadges,
                CCDBucketAccessTokens,
                CCDBuckets,
                CCDContent,
                CCDEntries,
                CCDEnvironments,
                CCDOrgs,
                CCDPermissions,
                CCDReleases,
                Matchmaker,
                MultiplayAllocations,
                MultiplayBuildConfigurations,
                MultiplayBuilds,
                MultiplayFleets,
                MultiplayMachines,
                MultiplayProviders,
                MultiplayRegistry,
                MultiplayServers,
                UGCContent,
                UGCContentVersions,
                UGCModeration,
                UGCPlayer,
                UGCProject,
                UGCRepresentation,
                UGCTag,
                UGCWebhook
#endif
            };

        }

        public static IAdminClient Create(IApiClient apiClient = null)
        {
            if (apiClient == null)
            {
                apiClient = new ApiClient();
            }

            return new AdminClient(
                new CloudCodeModulesAdminApi(apiClient),
                new CloudCodeScriptsAdminApi(apiClient),
                new CloudSaveDataAdminApi(apiClient),
                new CloudSaveFilesAdminApi(apiClient),
                new EconomyAdminApi(apiClient),
                new EnvironmentAdminApi(apiClient),
                new ConfigsAdminApi(apiClient),
                new SchemasAdminApi(apiClient),
                new GameOverridesAdminApi(apiClient),
                new LeaderboardsAdminApi(apiClient),
                new LogsAdminApi(apiClient),
                new PlayerAuthenticationAdminApi(apiClient),
                new PlayerPolicyAdminApi(apiClient),
                new ProjectPolicyAdminApi(apiClient),
                new SchedulerAdminApi(apiClient),
                new ServiceAuthenticationAdminApi(apiClient),
                new TriggersAdminApi(apiClient)
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
                ,
                new CCDBadgesAdminApi(apiClient),
                new CCDBucketAccessTokensAdminApi(apiClient),
                new CCDBucketsAdminApi(apiClient),
                new CCDContentAdminApi(apiClient),
                new CCDEntriesAdminApi(apiClient),
                new CCDEnvironmentsAdminApi(apiClient),
                new CCDOrgsAdminApi(apiClient),
                new CCDPermissionsAdminApi(apiClient),
                new CCDReleasesAdminApi(apiClient),
                new MatchmakerAdminApi(apiClient),
                new MultiplayAllocationsAdminApi(apiClient),
                new MultiplayBuildConfigurationsAdminApi(apiClient),
                new MultiplayBuildsAdminApi(apiClient),
                new MultiplayFleetsAdminApi(apiClient),
                new MultiplayMachinesAdminApi(apiClient),
                new MultiplayProvidersAdminApi(apiClient),
                new MultiplayRegistryAdminApi(apiClient),
                new MultiplayServersAdminApi(apiClient),
                new UGCContentAdminApi(apiClient),
                new UGCContentVersionsAdminApi(apiClient),
                new UGCModerationAdminApi(apiClient),
                new UGCPlayerAdminApi(apiClient),
                new UGCProjectAdminApi(apiClient),
                new UGCRepresentationAdminApi(apiClient),
                new UGCTagAdminApi(apiClient),
                new UGCWebhookAdminApi(apiClient)
#endif
                );
        }

        public void SetServiceAccount(string apiKey, string apiSecret)
        {
            foreach (var api in Apis)
            {
                api.Configuration.Username = apiKey;
                api.Configuration.Password = apiSecret;
            }
        }

        public void ClearCredentials()
        {
            foreach (var api in Apis)
            {
                api.Configuration.Username = null;
                api.Configuration.Password = null;
            }
        }
    }
}
#endif
