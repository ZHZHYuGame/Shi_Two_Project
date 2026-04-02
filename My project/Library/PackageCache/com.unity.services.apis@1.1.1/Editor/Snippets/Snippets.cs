namespace Unity.Services.Apis.Sample
{
    static class Snippets
    {
        public const string GameClient = "IGameClient gameClient = ApiService.CreateGameClient();";

        public const string ServerClient = "IServerClient serverClient = ApiService.CreateServerClient();";

        public const string TrustedClient = "ITrustedClient trustedClient = ApiService.CreateTrustedClient();";

        public const string AdminClient = "IAdminClient adminClient = ApiService.CreateAdminClient();";

        public const string OperationAwait =
            "async Task SignUpAsync()\n" +
            "{\n" +
            "    var client = ApiService.CreateGameClient();\n" +
            "    var response = await client.SignUpAnonymously(\"project_id\", \"environment\");\n" +
            "    Debug.Log($\"IsSuccessful: {response.IsSuccessful}\");\n" +
            "};";

        public const string OperationEvents =
            "void SignUp()\n" +
            "{\n" +
            "    var client = ApiService.CreateGameClient();\n" +
            "    var operation = client.SignUpAnonymously(\"project_id\", \"environment_name\");\n" +
            "    operation.Completed += (response) =>\n" +
            "    {\n" +
            "        Debug.Log($\"IsSuccessful: {response.IsSuccessful}\");\n" +
            "    }\n" +
            "}";

        public const string OperationCoroutine =
            "IEnumerator SignUp()\n" +
            "{\n" +
            "    var client = ApiService.CreateGameClient();\n" +
            "    var operation = client.SignUpAnonymously(\"project_id\", \"environment_name\");\n" +
            "    yield return operation.WaitForCompletion();\n" +
            "    Debug.Log($\"IsSuccessful: {operation.Response.IsSuccessful}\");\n" +
            "}";

        public const string ResponseIsSuccessful =
            "var response = await client.SignUpAnonymously(\"project_id\", \"environment\");\n\n" +
            "if (response.IsSuccessful)\n" +
            "{\n" +
            "}";

        public const string ResponseEnsureSuccessful =
            "try\n" +
            "{\n" +
            "    var response = await client.SignUpAnonymously(\"project_id\", \"environment\");\n" +
            "    response.EnsureSuccessful();\n" +
            "}\n" +
            "catch (ApiException e)\n" +
            "{\n" +
            "}";

        public const string AdminAuth =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "client.SetServiceAccount(apiKey, apiSecret);";

        public const string GameAuth =
            "IGameClient client = ApiService.CreateGameClient();\n" +
            "var operation = client.SignUpAnonymously(...);\n" +
            "var operation = client.SignInWithExternalToken(...);\n" +
            "var operation = client.SignInWithSessionToken(...);\n" +
            "var operation = client.SignUpWithUsernamePassword(...);\n" +
            "var operation = client.SignInWithUsernamePassword(...);\n" +
            "var operation = client.SignInWithCode(...);\n" +
            "var operation = client.SignInWithCustomID(...);";

        public const string ServerMultiplayAuth =
            "IServerClient client = ApiService.CreateServerClient();\n" +
            "var operation = client.SignInFromServer();";

        public const string ServerServiceAccountAuth =
            "IServerClient client = ApiService.CreateServerClient();\n" +
            "client.SetServiceAccount(apiKey, apiSecret);\n" +
            "var operation = client.SignInWithServiceAccount(ProjectId, EnvironmentId);";

        public const string TrustedAuth =
            "ITrustedClient client = ApiService.CreateTrustedClient();\n" +
            "client.SetServiceAccount(apiKey, apiSecret);\n" +
            "var operation = client.SignInWithServiceAccount(ProjectId, EnvironmentId);";

        public const string PlayerPolicyAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IPlayerPolicyAdminApi api = client.PlayerPolicy;";

        public const string ProjectPolicyAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IProjectPolicyAdminApi api = client.ProjectPolicy;";

        public const string CloudCodeModulesAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "ICloudCodeScriptsAdminApi api = client.CloudCodeModules;";

        public const string CloudCodeScriptsAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "ICloudCodeScriptsAdminApi api = client.CloudCodeScripts;";

        public const string CloudSaveAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "ICloudSaveDataAdminApi api = client.CloudSaveData;";

        public const string CloudSaveTrusted =
            "ITrustedClient client = ApiService.CreateTrustedClient();\n" +
            "ICloudSaveDataApi api = client.CloudSaveData;";

        public const string EconomyAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IEconomyAdminApi api = client.Economy;";

        public const string EconomyTrusted =
            "ITrustedClient client = ApiService.CreateTrustedClient();\n" +
            "IEconomyConfigurationApi api = client.EconomyConfiguration;\n" +
            "IEconomyCurrenciesApi api = client.EconomyCurrencies;\n" +
            "IEconomyInventoryApi api = client.EconomyInventory;\n" +
            "IEconomyPurchasesApi api = client.EconomyPurchases;";

        public const string EnvironmentAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IEnvironmentAdminApi api = client.Environment;";

        public const string LeaderboardsAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "ILeaderboardsAdminApi api = client.Leaderboards;";

        public const string LobbiesTrusted =
            "ITrustedClient client = ApiService.CreateTrustedClient();\n" +
            "ILobbyApi api = client.Lobby;";

        public const string MatchmakerAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IMatchmakerAdminApi api = client.Matchmaker;";

        public const string MultiplayAllocationsAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IMultiplayAllocationsAdminApi api = client.MultiplayAllocations;";

        public const string MultiplayServersAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IMultiplayServersAdminApi api = client.MultiplayServers;";

        public const string PlayerAuthAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IPlayerAuthenticationAdminApi api = client.PlayerAuthentication;";

        public const string PlayerAuthGame =
            "IGameClient client = ApiService.CreateGameClient();\n" +
            "IPlayerAuthenticationApi api = client.PlayerAuthentication;";

        public const string PlayerNamesTrusted =
            "ITrustedClient client = ApiService.CreateTrustedClient();\n" +
            "IPlayerNamesApi api = client.PlayerNames;";

        public const string RemoteConfigAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IConfigsAdminApi api = client.RemoteConfig;";

        public const string UGCTagsAdmin =
            "IAdminClient client = ApiService.CreateAdminClient();\n" +
            "IUGCTagAdminApi api = client.UGCTag;";

    }
}
