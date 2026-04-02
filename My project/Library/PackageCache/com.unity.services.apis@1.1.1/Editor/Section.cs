namespace Unity.Services.Apis.Sample
{
    enum Section
    {
        About,
        Settings,
        Authorization,
        AccessPolicy,
        Accounts,
        CloudCode,
        CloudSave,
        Economy,
        Environment,
        Leaderboards,
        Lobbies,
        PlayerNames,
        RemoteConfig,

#if ENABLE_SERVICES_EXPERIMENTAL_APIS
        Matchmaker,
        Multiplay,
        UGC
#endif
    }
}
