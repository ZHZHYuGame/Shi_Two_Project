#if UNITY_EDITOR || ENABLE_RUNTIME_ADMIN_APIS
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
    /// <summary>
    /// The Admin Client offers all public admin apis clients available on the Services Gateway.
    /// It relies on service account credentials to authorize it's apis.
    /// </summary>
    public interface IAdminClient
    {
        /// <summary>
        /// CloudCode Modules Api (C#)
        /// </summary>
        ICloudCodeModulesAdminApi CloudCodeModules { get; }

        /// <summary>
        /// CloudCode Scripts Api (JS)
        /// </summary>
        ICloudCodeScriptsAdminApi CloudCodeScripts { get; }

        /// <summary>
        /// CloudSave Data Api
        /// </summary>
        ICloudSaveDataAdminApi CloudSaveData { get; }

        /// <summary>
        /// CloudSave Files Api
        /// </summary>
        ICloudSaveFilesAdminApi CloudSaveFiles { get; }

        /// <summary>
        /// Economy Api
        /// </summary>
        IEconomyAdminApi Economy { get; }

        /// <summary>
        /// Environment Api
        /// </summary>
        IEnvironmentAdminApi Environment { get; }

        /// <summary>
        /// RemoteConfig Api
        /// </summary>
        IConfigsAdminApi RemoteConfig { get; }

        /// <summary>
        /// RemoteConfig Schemas Api
        /// </summary>
        ISchemasAdminApi RemoteConfigSchemas { get; }

        /// <summary>
        /// GameOverrides Api
        /// </summary>
        IGameOverridesAdminApi GameOverrides { get; }

        /// <summary>
        /// Leaderboards Api
        /// </summary>
        ILeaderboardsAdminApi Leaderboards { get; }

        /// <summary>
        /// Logs Api
        /// </summary>
        ILogsAdminApi Logs { get; }

        /// <summary>
        /// PlayerAuth Api
        /// </summary>
        IPlayerAuthenticationAdminApi PlayerAuthentication { get; }

        /// <summary>
        /// PlayerPolicy Api
        /// </summary>
        IPlayerPolicyAdminApi PlayerPolicy { get; }

        /// <summary>
        /// ProjectPolicy Api
        /// </summary>
        IProjectPolicyAdminApi ProjectPolicy { get; }

        /// <summary>
        /// Scheduler Api
        /// </summary>
        ISchedulerAdminApi Scheduler { get; }

        /// <summary>
        /// Service Authentication Api
        /// </summary>
        IServiceAuthenticationAdminApi ServiceAuthentication { get; }

        /// <summary>
        /// Triggers Api
        /// </summary>
        ITriggersAdminApi Triggers { get; }

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
        /// <summary>
        /// CCDBadges api
        /// </summary>
        ICCDBadgesAdminApi CCDBadges { get; }

        /// <summary>
        /// CCDBucketAccessTokens api
        /// </summary>
        ICCDBucketAccessTokensAdminApi CCDBucketAccessTokens { get; }

        /// <summary>
        /// CCDBuckets api
        /// </summary>
        ICCDBucketsAdminApi CCDBuckets { get; }

        /// <summary>
        /// CCDContent api
        /// </summary>
        ICCDContentAdminApi CCDContent { get; }

        /// <summary>
        /// CCDEntries api
        /// </summary>
        ICCDEntriesAdminApi CCDEntries { get; }

        /// <summary>
        /// CCDEnvironments api
        /// </summary>
        ICCDEnvironmentsAdminApi CCDEnvironments { get; }

        /// <summary>
        /// CCDOrgs api
        /// </summary>
        ICCDOrgsAdminApi CCDOrgs { get; }

        /// <summary>
        /// CCDPermissions api
        /// </summary>
        ICCDPermissionsAdminApi CCDPermissions { get; }

        /// <summary>
        /// CCDReleases api
        /// </summary>
        ICCDReleasesAdminApi CCDReleases { get; }

        /// <summary>
        /// Matchmaker Api
        /// </summary>
        IMatchmakerAdminApi Matchmaker { get; }

        /// <summary>
        /// Multiplay Allocations Api
        /// </summary>
        IMultiplayAllocationsAdminApi MultiplayAllocations { get; }

        /// <summary>
        /// Multiplay Build Configurations Api
        /// </summary>
        IMultiplayBuildConfigurationsAdminApi MultiplayBuildConfigurations { get; }

        /// <summary>
        /// Multiplay Builds Api
        /// </summary>
        IMultiplayBuildsAdminApi MultiplayBuilds { get; }

        /// <summary>
        /// Multiplay Fleets Api
        /// </summary>
        IMultiplayFleetsAdminApi MultiplayFleets { get; }

        /// <summary>
        /// Multiplay Machines Api
        /// </summary>
        IMultiplayMachinesAdminApi MultiplayMachines { get; }

        /// <summary>
        /// Multiplay Providers Api
        /// </summary>
        IMultiplayProvidersAdminApi MultiplayProviders { get; }

        /// <summary>
        /// Multiplay Registry Api
        /// </summary>
        IMultiplayRegistryAdminApi MultiplayRegistry { get; }

        /// <summary>
        /// Multiplay Servers Api
        /// </summary>
        IMultiplayServersAdminApi MultiplayServers { get; }

        /// <summary>
        /// UGC Content Api
        /// </summary>
        IUGCContentAdminApi UGCContent { get; }

        /// <summary>
        /// UGC Content Versions Api
        /// </summary>
        IUGCContentVersionsAdminApi UGCContentVersions { get; }

        /// <summary>
        /// UGC CModeration Api
        /// </summary>
        IUGCModerationAdminApi UGCModeration { get; }

        /// <summary>
        /// UGC Player Api
        /// </summary>
        IUGCPlayerAdminApi UGCPlayer { get; }

        /// <summary>
        /// UGC Project Api
        /// </summary>
        IUGCProjectAdminApi UGCProject { get; }

        /// <summary>
        /// UGC Representation Api
        /// </summary>
        IUGCRepresentationAdminApi UGCRepresentation { get; }

        /// <summary>
        /// UGC Tag Api
        /// </summary>
        IUGCTagAdminApi UGCTag { get; }

        /// <summary>
        /// UGC Webhook Api
        /// </summary>
        IUGCWebhookAdminApi UGCWebhook { get; }
#endif

        /// <summary>
        /// Set the service account that the apis will use.
        /// </summary>
        /// <param name="apiKey">The service account key</param>
        /// <param name="apiSecret">The service account secret</param>
        void SetServiceAccount(string apiKey, string apiSecret);

        /// <summary>
        /// Clears the credentials from the configuration of all apis
        /// </summary>
        void ClearCredentials();
    }
}
#endif
