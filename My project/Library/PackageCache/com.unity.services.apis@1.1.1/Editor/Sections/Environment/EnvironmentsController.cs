using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Apis;
using Unity.Services.Apis.Admin.Environment;

namespace Unity.Services.Apis.Sample
{
    class EnvironmentsController : ApiController
    {
        IEnvironmentAdminApi EnvironmentApi { get; set; }
        public List<EnvironmentResponse> Environments { get; private set; }
        public event Action EnvironmentsChange;

        public EnvironmentsController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient)
        {
            EnvironmentApi = adminClient.Environment;
        }

        public async Task GetEnvironmentsAsync()
        {
            var response = await EnvironmentApi.GetEnvironments(CloudProjectId);
            response.EnsureSuccessful();
            Environments = response.Data.Results;
            EnvironmentsChange?.Invoke();
        }

        public async Task CreateAsync(string environmentName)
        {
            var newEnvironment = (await EnvironmentApi.CreateEnvironment(CloudProjectId, new EnvironmentRequestBody(environmentName))).Data;
            Environments.Add(newEnvironment);
            EnvironmentsChange?.Invoke();
        }

        public async Task DeleteAsync(EnvironmentResponse environment)
        {
            await EnvironmentApi.DeleteEnvironment(CloudProjectId, environment.Id.ToString());
            Environments.Remove(environment);
            EnvironmentsChange?.Invoke();
        }
    }
}
