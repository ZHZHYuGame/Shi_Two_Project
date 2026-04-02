namespace Unity.Services.Apis.Sample
{
    class SettingsController : ApiController
    {
        IAdminClient AdminClient { get; set; }
        ITrustedClient TrustedClient { get; set; }

        public SettingsController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient, ITrustedClient trustedClient)
        {
            AdminClient = adminClient;
            TrustedClient = trustedClient;
            UpdateClients();
        }

        public void Save(string cloudProjectId, string environmentName, string environmentId, string adminKeyIdentifier, string adminKeySecret, string trustedKeyIdentifier, string trustedKeySecret)
        {
            CloudSettings.Update(cloudProjectId, environmentName, environmentId, adminKeyIdentifier, adminKeySecret, trustedKeyIdentifier, trustedKeySecret);
            CloudSettings.Save();
            UpdateClients();
        }

        void UpdateClients()
        {
            AdminClient.SetServiceAccount(CloudSettings.AdminKeyIdentifier, CloudSettings.AdminKeySecret);
            TrustedClient.SetServiceAccount(CloudSettings.TrustedKeyIdentifier, CloudSettings.TrustedKeySecret);
        }
    }
}
