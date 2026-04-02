using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Apis;
using Unity.Services.Apis.Admin.Leaderboards;

namespace Unity.Services.Apis.Sample
{
    class LeaderboardsController : ApiController
    {
        public event Action LeaderboardsChanged;
        public event Action ScoresChanged;

        ILeaderboardsAdminApi LeaderboardsApi { get; set; }

        public string[] LeaderboardNames { get; private set; }
        public int CurrentLeaderboardIndex { get; private set; }
        public UpdatedLeaderboardConfig CurrentLeaderboard => Leaderboards.Count > CurrentLeaderboardIndex ? Leaderboards[CurrentLeaderboardIndex] : null;
        public List<UpdatedLeaderboardConfig> Leaderboards { get; } = new List<UpdatedLeaderboardConfig>();
        public List<LeaderboardEntry> CurrentScores { get; private set; }

        public LeaderboardsController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient)
        {
            LeaderboardsApi = adminClient.Leaderboards;
        }

        public async Task LoadAsync()
        {
            Leaderboards.Clear();

            var response = await LeaderboardsApi.GetLeaderboardConfigs(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            Leaderboards.AddRange(response.Data.Results);
            LeaderboardNames = Leaderboards.Select(x => x.Name).ToArray();
            CurrentLeaderboardIndex = 0;
            LeaderboardsChanged?.Invoke();

            if (CurrentLeaderboard != null)
            {
                await LoadScoresAsync();
            }
        }

        public async Task LoadScoresAsync()
        {
            var response = await LeaderboardsApi.GetLeaderboardScores(CloudProjectId, EnvironmentId, CurrentLeaderboard.Id);
            response.EnsureSuccessful();
            CurrentScores = response.Data.Results;
            ScoresChanged?.Invoke();
        }

        public Task SelectPreviousLeaderboardAsync()
        {
            if (Leaderboards.Count > 1)
            {
                if (CurrentLeaderboardIndex - 1 < 0)
                {
                    CurrentLeaderboardIndex = Leaderboards.Count - 1;
                }
                else
                {
                    CurrentLeaderboardIndex--;
                }

                return LoadScoresAsync();
            }

            return Task.CompletedTask;
        }

        public Task SelectNextLeaderboardAsync()
        {
            if (Leaderboards.Count > 1)
            {
                if (CurrentLeaderboardIndex + 1 >= Leaderboards.Count)
                {
                    CurrentLeaderboardIndex = 0;
                }
                else
                {
                    CurrentLeaderboardIndex++;
                }

                return LoadScoresAsync();
            }

            return Task.CompletedTask;
        }

        public async Task DeleteEntryAsync(LeaderboardEntry entry)
        {
            await LeaderboardsApi.DeleteLeaderboardPlayerScore(CloudProjectId, EnvironmentId, CurrentLeaderboard.Id, entry.PlayerId);
            await LoadScoresAsync();
        }
    }
}
