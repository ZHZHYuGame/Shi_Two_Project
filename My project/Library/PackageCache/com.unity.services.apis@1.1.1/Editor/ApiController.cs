namespace Unity.Services.Apis.Sample
{
    abstract class ApiController
    {
        internal CloudSettings CloudSettings { get; }

        internal ApiController(CloudSettings settings)
        {
            CloudSettings = settings;
        }

        internal string CloudProjectId => CloudSettings.CloudProjectId;
        internal string EnvironmentName => CloudSettings.GameEnvironmentName;
        internal string EnvironmentId => CloudSettings.GameEnvironmentId;
        internal string AdminKeyIdentifier => CloudSettings.AdminKeyIdentifier;
        internal string AdminKeySecret => CloudSettings.AdminKeySecret;
        internal string TrustedKeyIdentifier => CloudSettings.AdminKeyIdentifier;
        internal string TrustedKeySecret => CloudSettings.AdminKeySecret;
    }
}
