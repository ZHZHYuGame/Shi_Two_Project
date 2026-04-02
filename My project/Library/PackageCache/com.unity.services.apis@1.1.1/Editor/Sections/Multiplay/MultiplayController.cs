#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.Multiplay;

namespace Unity.Services.Apis.Sample
{
    class MultiplayController : ApiController
    {
        IMultiplayAllocationsAdminApi AllocationsApi { get; set; }
        IMultiplayServersAdminApi ServersApi { get; set; }

        public event Action AllocationsChanged;
        public event Action ServersChanged;
        public List<MultiplayAllocationsTestAllocation> Allocations { get; private set; }
        public List<MultiplayServersServer1> Servers { get; private set; }

        public MultiplayController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient)
        {
            AllocationsApi = adminClient.MultiplayAllocations;
            ServersApi = adminClient.MultiplayServers;
        }

        public async Task RefreshAllocationsAsync()
        {
            var response = await AllocationsApi.ListTestAllocations(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            Allocations = response.Data.Allocations;
            AllocationsChanged?.Invoke();
        }

        public async Task RefreshServersAsync()
        {
            var response = await ServersApi.ListServers(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            Servers = response.Data;
            ServersChanged?.Invoke();
        }

        public async Task AllocateAsync(string fleetId)
        {
            await AllocationsApi.ProcessTestAllocation(CloudProjectId, EnvironmentId, fleetId);
        }

        public async Task DeallocateAsync(MultiplayAllocationsTestAllocation allocation)
        {
            await AllocationsApi.ProcessTestDeallocation(CloudProjectId, EnvironmentId, allocation.FleetId, allocation.AllocationId);
        }
    }
}
#endif
