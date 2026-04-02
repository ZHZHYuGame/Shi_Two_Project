namespace Unity.Services.Apis
{
    /// <summary>
    /// Provides functionality to build the different types of clients.
    /// </summary>
    public static class ApiService
    {
        /// <summary>
        /// A client for game apis using player authentication
        /// </summary>
        /// <param name="apiClient">Optional custom api client.</param>
        /// <returns>An instance of a game client</returns>
        public static IGameClient CreateGameClient(IApiClient apiClient = null) => GameClient.Create(apiClient);

#if UNITY_EDITOR || UNITY_SERVER || ENABLE_SERVER_APIS
        /// <summary>
        /// A client for game apis using server authentication
        /// </summary>
        /// <param name="apiClient">Optional custom api client.</param>
        /// <returns>An instance of a server client</returns>
        public static IServerClient CreateServerClient(IApiClient apiClient = null) => ServerClient.Create(apiClient);
#endif

#if UNITY_EDITOR || ENABLE_RUNTIME_ADMIN_APIS
        /// <summary>
        /// A client for admin apis using service account credentials
        /// </summary>
        /// <param name="apiClient">Optional custom api client.</param>
        /// <returns>An instance of a admin client</returns>
        public static IAdminClient CreateAdminClient(IApiClient apiClient = null) => AdminClient.Create(apiClient);

        /// <summary>
        /// A client for game apis using trusted authentication
        /// </summary>
        /// <param name="apiClient">Optional custom api client.</param>
        /// <returns>An instance of a trusted client</returns>
        public static ITrustedClient CreateTrustedClient(IApiClient apiClient = null) => TrustedClient.Create(apiClient);
#endif
    }
}
