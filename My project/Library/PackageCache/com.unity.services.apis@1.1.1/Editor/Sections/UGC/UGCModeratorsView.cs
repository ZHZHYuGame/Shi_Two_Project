#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.UGC;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class UGCModeratorsView : EditorView
    {
        UGCController UGC => App.UGC;
        UGCSectionView SectionView { get; }

        string m_CreateModeratorInput;
        string m_CreateModeratorRole;

        public UGCModeratorsView(App app, UGCSectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H2("Add Moderator");
            }
            using (UI.ContentPanel().Scope())
            {
                if (UGC.PlayerRolesNames != null)
                {
                    using (UI.HorizontalScope())
                    {
                        UI.Label("Player Id:").SetWidth(100);
                        UI.TextField().BindValue(UI.BindField(this, () => m_CreateModeratorInput)).SetWidth(350);
                    }
                    using (UI.HorizontalScope())
                    {
                        UI.Label("Player Role:").SetWidth(100);
                        UI.PopupField(UGC.PlayerRolesNames.ToList()).BindValue(UI.BindField(this, () => m_CreateModeratorRole)).SetWidth(150);
                    }
                }
                else
                {
                    UI.Button("Load Player Roles", GetPlayerRolesAsync).SetWidth(150);
                }
            }
            using (UI.FooterPanel().Scope())
            {
                if (!string.IsNullOrEmpty(m_CreateModeratorInput) && UGC.PlayerRolesNames != null)
                {
                    UI.Button("Add", () => CreateModeratorAsync(m_CreateModeratorInput, m_CreateModeratorRole)).SetWidth(100);
                }
            }

            UI.Space();
            UI.Separator();
            UI.Space();
            UI.Button("Search Moderators", SearchModeratorsAsync).SetWidth(200);
            UI.Space();

            if (UGC.Moderators != null)
            {
                using (UI.HeaderPanel().Scope())
                {
                    UI.H2("Moderators");
                }
                using (UI.ContentPanel().Scope())
                {
                    if (UGC.Moderators.Results.Count > 0)
                    {
                        foreach (var log in UGC.Moderators.Results)
                        {
                            OnModeratorGUI(log);
                        }
                    }
                    else
                    {
                        UI.Label("No results.");
                    }
                }
                using (UI.FooterPanel().Scope())
                {
                    UI.Label($"{UGC.Moderators.Offset} - {UGC.Moderators.Offset + UGC.Moderators.Results.Count} of {UGC.Moderators.Total}");
                }
            }
        }

        void OnModeratorGUI(ModeratorDTO moderator)
        {
            using (UI.VerticalPanel().Scope())
            {
                using (UI.Block().Scope())
                {
                    UI.Label("Player Id").SetWidth(100);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => moderator.PlayerId));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Player Role").SetWidth(100);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => moderator.PlayerRole?.Name));
                }

                UI.Button("Remove", () => RemoveModeratorAsync(moderator.PlayerId))
                    .SetBackgroundImageTint(Color.red)
                    .SetWidth(150);
            }
        }

        public async Task SearchModeratorsAsync()
        {
            try
            {
                await UGC.SearchModeratorsAsync();
                SectionView.SetStatus(Status.Success, "SearchModerators successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "SearchModerators failed.");
            }
        }

        public async Task CreateModeratorAsync(string playerId, string playerRoleId)
        {
            try
            {
                await UGC.CreateModeratorAsync(playerId, playerRoleId);
                SectionView.SetStatus(Status.Success, "CreateModerator successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "CreateModerator failed.");
            }
        }

        public async Task RemoveModeratorAsync(string playerId)
        {
            try
            {
                await UGC.RemoveModeratorAsync(playerId);
                SectionView.SetStatus(Status.Success, "RemoveModerator successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "RemoveModerator failed.");
            }
        }

        public async Task GetPlayerRolesAsync()
        {
            try
            {
                await UGC.GetPlayerRolesAsync();
                SectionView.SetStatus(Status.Success, "GetPlayerRoles successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "GetPlayerRoles failed.");
            }
        }
    }
}
#endif
