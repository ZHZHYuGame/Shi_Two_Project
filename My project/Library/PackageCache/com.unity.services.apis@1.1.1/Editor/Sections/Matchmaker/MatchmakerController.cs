#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.Matchmaker;

namespace Unity.Services.Apis.Sample
{
    class MatchmakerController : ApiController
    {
        public event Action QueuesChanged;

        IAdminClient AdminClient { get; set; }
        IMatchmakerAdminApi MatchmakerApi { get; set; }

        public List<QueueConfig> Queues { get; } = new List<QueueConfig>();
        readonly string ServiceId;

        public MatchmakerController(CloudSettings settings) : base(settings)
        {
            ServiceId = Guid.NewGuid().ToString();
        }

        public void Initialize(IAdminClient adminClient)
        {
            AdminClient = adminClient;
            MatchmakerApi = adminClient.Matchmaker;
        }

        public async Task LoadAsync()
        {
            Queues.Clear();
            var response = await MatchmakerApi.ListQueues(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            Queues.AddRange(response.Data);
            QueuesChanged?.Invoke();
        }

        public async Task DeleteQueueAsync(QueueConfig queue)
        {
            var response = await MatchmakerApi.DeleteQueue(CloudProjectId, EnvironmentId, queue.Name);
            response.EnsureSuccessful();
            Queues.Remove(queue);
            QueuesChanged?.Invoke();
        }
    }
}
#endif
