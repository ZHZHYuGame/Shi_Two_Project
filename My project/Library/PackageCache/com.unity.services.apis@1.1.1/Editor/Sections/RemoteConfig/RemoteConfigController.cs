using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Services.Apis.Admin.RemoteConfig;

namespace Unity.Services.Apis.Sample
{
    class RemoteConfigController : ApiController
    {
        IConfigsAdminApi ConfigsApi { get; set; }

        public bool CurrentConfigExists { get; private set; }
        public string CurrentConfigType { get; private set; }
        public string CurrentConfig { get; private set; }

        public RemoteConfigController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient)
        {
            ConfigsApi = adminClient.RemoteConfig;
        }

        public async Task GetRemoteConfigAsync()
        {
            CurrentConfigExists = false;

            var getConfigsResponse = await ConfigsApi.GetConfigByEnvironmentId(CloudProjectId, EnvironmentId);
            var token = JObject.Parse(getConfigsResponse.Content);
            CurrentConfig = token.ToString(Formatting.Indented);
            CurrentConfigExists = true;
        }

        public void SetCurrentType(string configType)
        {
            CurrentConfigExists = false;
            CurrentConfigType = configType;
            CurrentConfig = null;
        }

        public async Task CreateRemoteConfigAsync(string configContent)
        {
            if (CurrentConfigExists)
                return;

            var settings = JsonConvert.DeserializeObject<List<Setting>>(configContent);

            var datetime = DateTime.UtcNow;
            AppLogger.Log($"Datetime: {datetime}");
            var request = new CreateConfigRequest(
                id: Guid.NewGuid().ToString(),
                value: settings,
                projectId: CloudProjectId,
                environmentId: EnvironmentId,
                createdAt: datetime,
                updatedAt: datetime);

            await ConfigsApi.CreateConfig(CloudProjectId, request);
        }

        public async Task UpdateRemoteConfigAsync(string configContent)
        {
            if (!CurrentConfigExists)
                return;

            var existingConfig = (await ConfigsApi.GetConfigByEnvironmentId(CloudProjectId, EnvironmentId)).Data.Configs.FirstOrDefault();

            var settings = JsonConvert.DeserializeObject<List<Setting>>(configContent);

            var configId = existingConfig.Id;
            var request = new UpdateConfigByIdRequest(value: settings);
            await ConfigsApi.UpdateConfigById(configId, CloudProjectId, request);
            CurrentConfig = configContent;
        }
    }
}
