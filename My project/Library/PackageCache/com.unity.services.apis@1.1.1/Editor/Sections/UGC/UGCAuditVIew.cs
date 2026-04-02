#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.UGC;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    class UGCAuditView : EditorView
    {
        UGCController UGC => App.UGC;
        UGCSectionView SectionView { get; }

        public UGCAuditView(App app, UGCSectionView sectionView) : base(app)
        {
            SectionView = sectionView;
        }

        public override void CreateGUI()
        {
            UI.Button("Search Audit Logs", SearchAuditLogsAsync).SetWidth(200);
            UI.Space();

            if (UGC.AuditLogs != null)
            {
                using (UI.HeaderPanel().Scope())
                {
                    UI.H2("Audit Logs");
                }
                using (UI.ContentPanel().Scope())
                {
                    if (UGC.AuditLogs.Results.Count > 0)
                    {
                        foreach (var log in UGC.AuditLogs.Results)
                        {
                            OnAuditLogGUI(log);
                        }
                    }
                    else
                    {
                        UI.Label("No results.");
                    }
                }
                using (UI.FooterPanel().Scope())
                {
                    UI.Label($"{UGC.AuditLogs.Offset} - {UGC.AuditLogs.Offset + UGC.AuditLogs.Results.Count} of {UGC.AuditLogs.Total}");
                }
            }
        }

        void OnAuditLogGUI(ContentModerationAuditLogDTO log)
        {
            using (UI.VerticalPanel().Scope())
            {
                using (UI.Block().Scope())
                {
                    UI.Label("Id").SetWidth(200);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => log.Id));
                }

                using (UI.Block().Scope())
                {
                    UI.Label("Content Id").SetWidth(200);
                    UI.SelectableLabel().BindValue(UI.BindReadOnly(() => log.ContentId));
                }
            }

        }

        public async Task SearchAuditLogsAsync()
        {
            try
            {
                await UGC.LoadAuditLogsAsync();
                SectionView.SetStatus(Status.Success, "SearchAuditLogs successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "SearchAuditLogs failed.");
            }
        }
    }
}
#endif
