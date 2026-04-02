using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Apis;
using Unity.Services.Apis.Admin.AccessPolicy;

namespace Unity.Services.Apis.Sample
{
    class AccessPolicyController : ApiController
    {
        public event Action PlayerPoliciesChanged;
        public event Action ProjectPolicyChanged;

        public List<PlayerPolicy> PlayerPolicies { get; private set; }
        public Policy ProjectPolicy { get; private set; }

        IPlayerPolicyAdminApi PlayerPolicyApi { get; set; }
        IProjectPolicyAdminApi ProjectPolicyApi { get; set; }

        public AccessPolicyController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient)
        {
            PlayerPolicyApi = adminClient.PlayerPolicy;
            ProjectPolicyApi = adminClient.ProjectPolicy;
        }

        public async Task RefreshPlayerPoliciesAsync()
        {
            PlayerPolicies = null;
            var response = await PlayerPolicyApi.GetAllPlayerPolicies(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            PlayerPolicies = response.Data.Results;
            PlayerPoliciesChanged?.Invoke();
        }

        public async Task RefreshProjectPolicyAsync()
        {
            ProjectPolicy = null;
            var response = await ProjectPolicyApi.GetPolicy(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            ProjectPolicy = response.Data;
            ProjectPolicyChanged?.Invoke();
        }

        public Task CreateProjectPolicyStatementAsync(ProjectStatement statement)
        {
            ProjectPolicy.Statements.Add(statement);
            return UpdateProjectPolicyAsync();
        }

        public async Task UpdateProjectPolicyAsync()
        {
            var response = await ProjectPolicyApi.UpsertPolicy(CloudProjectId, EnvironmentId, ProjectPolicy);
            response.EnsureSuccessful();
            await RefreshProjectPolicyAsync();
        }

        public async Task UpdatePlayerPolicyAsync(string playerId, PlayerStatement statement)
        {
            var playerPolicy = PlayerPolicies.FirstOrDefault(x => x.PlayerId == playerId);
            var policy = new PlayerPolicyUpsert(playerPolicy?.Statements ?? new List<PlayerStatement>());
            policy.Statements.Add(statement);
            var response = await PlayerPolicyApi.UpsertPlayerPolicy(CloudProjectId, EnvironmentId, playerId, policy);
            response.EnsureSuccessful();
            await RefreshPlayerPoliciesAsync();
        }

        public async Task DeleteProjectPolicyAsync(string statementId)
        {
            var statementIds = new List<string>() { statementId };
            await ProjectPolicyApi.DeletePolicyStatements(CloudProjectId, EnvironmentId, new DeleteOptions(statementIds));
            await RefreshProjectPolicyAsync();
        }

        public async Task DeletePlayerPolicyAsync(string playerId, string statementId)
        {
            var statementIds = new List<string>() { statementId };
            await PlayerPolicyApi.DeletePlayerPolicyStatements(CloudProjectId, EnvironmentId, playerId, new DeleteOptions(statementIds));
            await RefreshPlayerPoliciesAsync();
        }
    }
}
