namespace Unity.Services.Apis.Sample
{
    class App
    {
        internal CloudSettings CloudSettings { get; }

        internal SettingsController Settings { get; private set; }
        internal AuthorizationController Authorization { get; private set; }
        internal AccessPolicyController AccessPolicy { get; private set; }
        internal AccountsController Account { get; private set; }
        internal CloudSaveController CloudSave { get; private set; }
        internal CloudCodeController CloudCode { get; private set; }
        internal EconomyController Economy { get; private set; }
        internal EnvironmentsController Environments { get; private set; }
        internal LeaderboardsController Leaderboards { get; private set; }
        internal LobbiesController Lobbies { get; private set; }
        internal RemoteConfigController RemoteConfig { get; private set; }
        internal PlayerNamesController PlayerNames { get; private set; }
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
        internal MatchmakerController Matchmaker { get; private set; }
        internal MultiplayController Multiplay { get; private set; }
        internal UGCController UGC { get; private set; }
#endif
        internal UIController UI { get; private set; }

        internal IAdminClient AdminClient { get; private set; }
        internal IGameClient GameClient { get; private set; }
        internal ITrustedClient TrustedClient { get; private set; }

        internal IUIBinding<bool> ValidConfigurationBinding { get; private set; }
        internal IUIBinding<bool> InvalidConfigurationBinding { get; private set; }

        public App()
        {
            AdminClient = ApiService.CreateAdminClient();
            GameClient = ApiService.CreateGameClient();
            TrustedClient = ApiService.CreateTrustedClient();

            UI = new UIController();

            CloudSettings = new CloudSettings();

            Settings = new SettingsController(CloudSettings);
            Authorization = new AuthorizationController(CloudSettings);
            AccessPolicy = new AccessPolicyController(CloudSettings);
            Account = new AccountsController(CloudSettings);
            CloudSave = new CloudSaveController(CloudSettings);
            CloudCode = new CloudCodeController(CloudSettings);
            Economy = new EconomyController(CloudSettings);
            Environments = new EnvironmentsController(CloudSettings);
            Leaderboards = new LeaderboardsController(CloudSettings);
            Lobbies = new LobbiesController(CloudSettings);
            RemoteConfig = new RemoteConfigController(CloudSettings);
            PlayerNames = new PlayerNamesController(CloudSettings);
#if ENABLE_SERVICES_EXPERIMENTAL_APIS
            Matchmaker = new MatchmakerController(CloudSettings);
            Multiplay = new MultiplayController(CloudSettings);
            UGC = new UGCController(CloudSettings);
#endif

            ValidConfigurationBinding = UI.BindReadOnly(() => CloudSettings.IsConfigured);
            InvalidConfigurationBinding = UI.BindReadOnly(() => !CloudSettings.IsConfigured);
        }

        public void Initialize()
        {
            CloudSettings.Load();

            Settings.Initialize(AdminClient, TrustedClient);
            Authorization.Initialize(TrustedClient);
            AccessPolicy.Initialize(AdminClient);
            Account.Initialize(AdminClient, GameClient);
            CloudSave.Initialize(TrustedClient);
            CloudCode.Initialize(AdminClient, TrustedClient);
            Economy.Initialize(AdminClient, TrustedClient);
            Environments.Initialize(AdminClient);
            Leaderboards.Initialize(AdminClient);
            Lobbies.Initialize(TrustedClient);
            PlayerNames.Initialize(TrustedClient);
            RemoteConfig.Initialize(AdminClient);

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
            Matchmaker.Initialize(AdminClient);
            Multiplay.Initialize(AdminClient);
            UGC.Initialize(AdminClient);
#endif
        }

        public void Update()
        {
            UI.Update();
        }
    }
}
