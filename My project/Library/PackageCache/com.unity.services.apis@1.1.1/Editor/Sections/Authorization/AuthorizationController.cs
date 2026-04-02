namespace Unity.Services.Apis.Sample
{
    enum AccessTokenViewType { Decoded, Raw }

    class AuthorizationController : ApiController
    {
        ITrustedClient TrustedClient { get; set; }

        public bool Authorized { get; private set; }
        public string AccessToken { get; private set; }
        public string Payload { get; private set; }

        public AccessTokenViewType ViewType { get => m_ViewType; set { m_ViewType = value; UpdateCurrentToken(); } }
        public string ViewToken { get; private set; }

        AccessTokenViewType m_ViewType;

        public AuthorizationController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(ITrustedClient trustedClient)
        {
            TrustedClient = trustedClient;
            TrustedClient.AccessTokenChanged += OnAccessTokenChanged;
        }

        public ApiOperation SignInWithServiceAccount()
        {
            return TrustedClient.SignInWithServiceAccount(CloudProjectId, EnvironmentId);
        }

        public void ClearCredentials()
        {
            TrustedClient.ClearCredentials();
        }

        void OnAccessTokenChanged()
        {
            AccessToken = TrustedClient.AccessToken;
            Authorized = AccessToken != null;
            Payload = AccessToken != null ? JsonWebToken.DecodePayload(AccessToken) : null;
            UpdateCurrentToken();
        }

        void UpdateCurrentToken()
        {
            switch (ViewType)
            {
                case AccessTokenViewType.Decoded:
                    ViewToken = Payload;
                    break;
                case AccessTokenViewType.Raw:
                    ViewToken = AccessToken;
                    break;
            }
        }
    }
}
