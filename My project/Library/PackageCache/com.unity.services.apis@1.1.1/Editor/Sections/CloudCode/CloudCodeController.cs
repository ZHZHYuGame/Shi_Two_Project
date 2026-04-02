using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.CloudCode;
using Unity.Services.Apis.CloudCode;

namespace Unity.Services.Apis.Sample
{
    class CloudCodeController : ApiController
    {
        public event Action ScriptsChanged;
        public event Action ModulesChanged;
        public List<ScriptMetadata> CloudCodeScripts { get; private set; }
        public List<ModuleMetadata> CloudCodeModules { get; private set; }

        ICloudCodeScriptsAdminApi CloudCodeScriptsAdmin { get; set; }
        ICloudCodeModulesAdminApi CloudCodeModulesAdmin { get; set; }
        ICloudCodeApi CloudCodeClient { get; set; }

        public CloudCodeController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient, ITrustedClient trustedClient)
        {
            CloudCodeScriptsAdmin = adminClient.CloudCodeScripts;
            CloudCodeModulesAdmin = adminClient.CloudCodeModules;
            CloudCodeClient = trustedClient.CloudCode;
        }

        public async Task GetCloudCodeScriptsAsync()
        {
            var response = await CloudCodeScriptsAdmin.ListScripts(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            CloudCodeScripts = response.Data.Results;
            ScriptsChanged?.Invoke();
        }

        public async Task GetCloudCodeModulesAsync()
        {
            var response = await CloudCodeModulesAdmin.ListModules(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            CloudCodeModules = response.Data.Results;
            ModulesChanged?.Invoke();
        }

        public async Task DeleteCloudCodeModuleAsync(ModuleMetadata module)
        {
            await CloudCodeModulesAdmin.DeleteModule(CloudProjectId, EnvironmentId, module.Name);
            CloudCodeModules.Remove(module);
            ModulesChanged?.Invoke();
        }

        public async Task DeleteCloudCodeScriptAsync(ScriptMetadata script)
        {
            await CloudCodeScriptsAdmin.DeleteScript(CloudProjectId, EnvironmentId, script.Name);
            CloudCodeScripts.Remove(script);
            ScriptsChanged?.Invoke();
        }

        public async Task RunCloudCodeScriptAsync(ScriptMetadata script)
        {
            await CloudCodeClient.RunScript(CloudProjectId, script.Name);
        }
    }
}
