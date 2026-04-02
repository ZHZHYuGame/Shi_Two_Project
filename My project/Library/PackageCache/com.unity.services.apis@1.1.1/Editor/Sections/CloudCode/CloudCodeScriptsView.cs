using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.CloudCode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class CloudCodeScriptsView : EditorView
    {
        CloudCodeController CloudCode => App.CloudCode;

        readonly CloudCodeSectionView SectionView;
        UIElement Container;

        public CloudCodeScriptsView(App app, CloudCodeSectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            CloudCode.ScriptsChanged += CreateGUIListScripts;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.CloudCodeScriptsAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Load Scripts", GetScriptsAsync).SetWidth(200);
                UI.Space();
                Container = UI.Element();
            }
        }

        void CreateGUIListScripts()
        {
            Container.Clear();

            if (CloudCode.CloudCodeScripts == null)
                return;

            using (Container.Scope())
            {
                foreach (var script in CloudCode.CloudCodeScripts)
                {
                    using (UI.Block().Scope())
                    {
                        using (UI.Block().Scope())
                        {
                            UI.Label(script.Name).SetWidth(200);
                        }
                        using (UI.Block().Scope())
                        {
                            UI.Label(script.Language.ToString()).SetWidth(50);
                        }
                        using (UI.Block().Scope())
                        {
                            UI.Label(script.Type.ToString()).SetWidth(50);
                        }

                        UI.Button("Run", () => RunCloudCodeScriptAsync(script)).SetWidth(100);

                        UI.Button("Delete", () => DeleteCloudCodeScriptAsync(script))
                            .SetBackgroundImageTint(Color.red)
                            .SetWidth(100);
                    }
                }
            }
        }

        async Task GetScriptsAsync()
        {
            try
            {
                SetStatus(Status.Warning, $"Getting CloudCode Scripts...");
                await CloudCode.GetCloudCodeScriptsAsync();
                SetStatus(Status.Success, $"GetCloudCodeScripts Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "GetCloudCodeScripts Failed.", e);
            }
        }

        async Task RunCloudCodeScriptAsync(ScriptMetadata script)
        {
            try
            {
                SetStatus(Status.Warning, $"Running CloudCode Script '{script.Name}'...");
                await CloudCode.RunCloudCodeScriptAsync(script);
                SetStatus(Status.Success, $"Running Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "RunCloudCodeScript Failed.", e);
            }
        }

        async Task DeleteCloudCodeScriptAsync(ScriptMetadata script)
        {
            try
            {
                SetStatus(Status.Warning, $"Deleting CloudCode Script '{script.Name}'...");
                await CloudCode.DeleteCloudCodeScriptAsync(script);
                SetStatus(Status.Success, $"DeleteCloudCodeScript Successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SetStatus(Status.Error, "DeleteCloudCodeScript Failed.", e);
            }
        }

        void SetStatus(Status status, string message, Exception exception = null)
        {
            SectionView.SetStatus(status, message, exception);
        }
    }
}
