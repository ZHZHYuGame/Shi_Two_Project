using UnityEditor;

namespace Unity.Services.Apis.Sample
{
    class CloudSettings
    {
        string CloudProjectIdKey => $"apis.sample.cloudsettings.{Scope}.cloud-project-id";
        string GameEnvironmentNameKey => $"apis.sample.cloudsettings.{Scope}.game-environment-name";
        string GameEnvironmentIdKey => $"apis.sample.cloudsettings.{Scope}.game-environment-id";
        string AdminKeyIdentifierKey => $"apis.sample.cloudsettings.{Scope}.admin-key-identifier";
        string AdminKeySecretKey => $"apis.sample.cloudsettings.{Scope}.admin-key-secret";
        string TrustedKeyIdentifierKey => $"apis.sample.cloudsettings.{Scope}.trusted-key-identifier";
        string TrustedKeySecretKey => $"apis.sample.cloudsettings.{Scope}.trusted-key-secret";

        public string CloudProjectId { get; set; }
        public string GameEnvironmentName { get; set; }
        public string GameEnvironmentId { get; set; }
        public string AdminKeyIdentifier { get; set; }
        public string AdminKeySecret { get; set; }
        public string TrustedKeyIdentifier { get; set; }
        public string TrustedKeySecret { get; set; }

        public string Scope { get; private set; }

        public bool IsConfigured =>
            !string.IsNullOrEmpty(CloudProjectId) &&
             !string.IsNullOrEmpty(GameEnvironmentName) &&
             !string.IsNullOrEmpty(GameEnvironmentId) &&
             !string.IsNullOrEmpty(AdminKeyIdentifier) &&
             !string.IsNullOrEmpty(AdminKeySecret) &&
             !string.IsNullOrEmpty(TrustedKeyIdentifier) &&
             !string.IsNullOrEmpty(TrustedKeySecret);

        public CloudSettings(string scope = "default")
        {
            Scope = scope;
        }

        public void Load()
        {
            if (EditorPrefs.HasKey(CloudProjectIdKey))
            {
                CloudProjectId = EditorPrefs.GetString(CloudProjectIdKey);
            }
            if (EditorPrefs.HasKey(GameEnvironmentNameKey))
            {
                GameEnvironmentName = EditorPrefs.GetString(GameEnvironmentNameKey);
            }

            if (EditorPrefs.HasKey(GameEnvironmentIdKey))
            {
                GameEnvironmentId = EditorPrefs.GetString(GameEnvironmentIdKey);
            }

            if (EditorPrefs.HasKey(AdminKeyIdentifierKey))
            {
                AdminKeyIdentifier = EditorPrefs.GetString(AdminKeyIdentifierKey);
            }

            if (EditorPrefs.HasKey(AdminKeySecretKey))
            {
                AdminKeySecret = EditorPrefs.GetString(AdminKeySecretKey);
            }

            if (EditorPrefs.HasKey(TrustedKeyIdentifierKey))
            {
                TrustedKeyIdentifier = EditorPrefs.GetString(TrustedKeyIdentifierKey);
            }

            if (EditorPrefs.HasKey(TrustedKeySecretKey))
            {
                TrustedKeySecret = EditorPrefs.GetString(TrustedKeySecretKey);
            }

        }

        public void Update(string cloudProjectId, string environmentName, string environmentId, string adminKeyIdentifier, string adminKeySecret, string trustedKeyIdentifier, string trustedKeySecret)
        {
            CloudProjectId = cloudProjectId;
            GameEnvironmentName = environmentName;
            GameEnvironmentId = environmentId;
            AdminKeyIdentifier = adminKeyIdentifier;
            AdminKeySecret = adminKeySecret;
            TrustedKeyIdentifier = trustedKeyIdentifier;
            TrustedKeySecret = trustedKeySecret;
        }

        public void Save()
        {
            EditorPrefs.SetString(GameEnvironmentNameKey, GameEnvironmentName);
            EditorPrefs.SetString(GameEnvironmentIdKey, GameEnvironmentId);
            EditorPrefs.SetString(CloudProjectIdKey, CloudProjectId);
            EditorPrefs.SetString(AdminKeyIdentifierKey, AdminKeyIdentifier);
            EditorPrefs.SetString(AdminKeySecretKey, AdminKeySecret);
            EditorPrefs.SetString(TrustedKeyIdentifierKey, TrustedKeyIdentifier);
            EditorPrefs.SetString(TrustedKeySecretKey, TrustedKeySecret);
        }
    }
}
