#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.UGC;

namespace Unity.Services.Apis.Sample
{
    class UGCController : ApiController
    {
        public ContentModerationAuditLogDTOPagedResult AuditLogs { get; private set; }
        public ModeratorDTOPagedResult Moderators { get; private set; }
        public List<PlayerRoleDTO> PlayerRoles { get; private set; }
        public List<TagDTO> Tags { get; private set; }
        public string[] PlayerRolesNames { get; private set; }

        public event Action TagsChanged;

        IAdminClient AdminClient { get; set; }

        IUGCContentAdminApi ContentApi => AdminClient.UGCContent;
        IUGCContentVersionsAdminApi ContentVersionsApi => AdminClient.UGCContentVersions;
        IUGCModerationAdminApi ModerationApi => AdminClient.UGCModeration;
        IUGCPlayerAdminApi PlayerApi => AdminClient.UGCPlayer;
        IUGCProjectAdminApi ProjectApi => AdminClient.UGCProject;
        IUGCRepresentationAdminApi RepresentationApi => AdminClient.UGCRepresentation;
        IUGCTagAdminApi TagApi => AdminClient.UGCTag;
        IUGCWebhookAdminApi WebhookApi => AdminClient.UGCWebhook;

        public UGCController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient)
        {
            AdminClient = adminClient;
        }

        public async Task LoadAuditLogsAsync()
        {
            AuditLogs = null;
            var response = await ModerationApi.SearchContentModerationAuditLogs(CloudProjectId, EnvironmentId, includeTotal: true, includeModerator: true);
            response.EnsureSuccessful();
            AuditLogs = response.Data;
        }

        public async Task SearchModeratorsAsync()
        {
            Moderators = null;
            var response = await PlayerApi.SearchModerators(CloudProjectId, EnvironmentId, includeTotal: true, includeStatistics: true);
            response.EnsureSuccessful();
            Moderators = response.Data;
        }

        public async Task CreateModeratorAsync(string playerId, string roleId)
        {
            await GetPlayerRolesIfNullAsync();
            var response = await PlayerApi.CreateModerator(CloudProjectId, EnvironmentId, null, new CreateModeratorRequest(playerId, roleId));
            response.EnsureSuccessful();
        }

        public async Task RemoveModeratorAsync(string playerId)
        {
            var response = await PlayerApi.RemoveModerator(CloudProjectId, EnvironmentId, playerId);
            response.EnsureSuccessful();
        }

        public async Task GetPlayerRolesAsync()
        {
            PlayerRoles = null;
            PlayerRolesNames = null;
            var response = await PlayerApi.GetPlayerRoles(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            PlayerRoles = response.Data;
            PlayerRolesNames = PlayerRoles.Select(x => x.Name).ToArray();
        }


        public async Task<TagDTO> CreateTagAsync(string tagName)
        {
            var response = await TagApi.CreateTag(CloudProjectId, EnvironmentId, newTagRequest: new NewTagRequest(tagName));
            response.EnsureSuccessful();
            await LoadTagsAsync();
            return response.Data;
        }

        public async Task DeleteTagAsync(string tagId)
        {
            var response = await TagApi.DeleteTag(CloudProjectId, EnvironmentId, tagId);
            response.EnsureSuccessful();
        }

        public async Task LoadTagsAsync()
        {
            var response = await TagApi.GetTags(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();
            Tags = response.Data;
            TagsChanged?.Invoke();
        }

        public void ClearTags()
        {
            Tags = null;
            TagsChanged?.Invoke();
        }

        Task GetPlayerRolesIfNullAsync()
        {
            return PlayerRoles == null ? GetPlayerRolesAsync() : Task.CompletedTask;
        }
    }
}
#endif
