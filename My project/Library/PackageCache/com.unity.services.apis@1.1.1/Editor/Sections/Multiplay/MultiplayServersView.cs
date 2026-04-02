#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Threading.Tasks;

namespace Unity.Services.Apis.Sample
{
    class MultiplayServersView : EditorView
    {
        MultiplayController Multiplay => App.Multiplay;

        MultiplaySectionView SectionView;
        UIElement ServersContainer;

        public MultiplayServersView(App app, MultiplaySectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            Multiplay.ServersChanged += RefreshServersGUI;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.MultiplayServersAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Load Servers", RefreshServersAsync).SetWidth(200).SetMarginBottom(10);

                ServersContainer = UI.Element();
            }
        }

        void RefreshServersGUI()
        {
            ServersContainer.Clear();

            using (ServersContainer.Scope())
            {
                if (Multiplay.Servers != null)
                {
                    foreach (var server in Multiplay.Servers)
                    {
                        using (UI.HeaderPanel().Scope())
                        {
                            UI.Label(server.Id.ToString());
                        }
                        using (UI.ContentPanel().Scope())
                        {
                            using (UI.Block().Scope())
                            {
                                UI.Label("HardwareType").SetWidth(100);
                                UI.SelectableLabel($"{server.HardwareType}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("Status").SetWidth(100);
                                UI.SelectableLabel($"{server.Status}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("BuildConfigurationID").SetWidth(100);
                                UI.SelectableLabel($"{server.BuildConfigurationID}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("BuildConfigurationName").SetWidth(100);
                                UI.SelectableLabel($"{server.BuildConfigurationName}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("BuildName").SetWidth(100);
                                UI.SelectableLabel($"{server.BuildName}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("CpuLimit").SetWidth(100);
                                UI.SelectableLabel($"{server.CpuLimit}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("Deleted").SetWidth(100);
                                UI.SelectableLabel($"{server.Deleted}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("FleetID").SetWidth(100);
                                UI.SelectableLabel($"{server.FleetID}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("FleetName").SetWidth(100);
                                UI.SelectableLabel($"{server.FleetName}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("HoldExpiresAt").SetWidth(100);
                                UI.SelectableLabel($"{server.HoldExpiresAt}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("Id").SetWidth(100);
                                UI.SelectableLabel($"{server.Id}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("Ip").SetWidth(100);
                                UI.SelectableLabel($"{server.Ip}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("LocationID").SetWidth(100);
                                UI.SelectableLabel($"{server.LocationID}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("LocationName").SetWidth(100);
                                UI.SelectableLabel($"{server.LocationName}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("MachineID").SetWidth(100);
                                UI.SelectableLabel($"{server.MachineID}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("MachineName").SetWidth(100);
                                UI.SelectableLabel($"{server.MachineName}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("MemoryLimit").SetWidth(100);
                                UI.SelectableLabel($"{server.MemoryLimit}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label("Port").SetWidth(100);
                                UI.SelectableLabel($"{server.Port}");
                            }
                            /*
                            using (UI.Block().Scope())
                            {
                                UI.Label("MachineSpec").SetWidth(100);
                                UI.SelectableLabel($"{server.MachineSpec.}");
                            }
                             */
                        }
                        using (UI.FooterPanel().Scope())
                        {
                            UI.Label($"");
                        }

                        UI.Space();
                    }
                }
            }
        }

        async Task RefreshServersAsync()
        {
            try
            {
                await Multiplay.RefreshServersAsync();
                SectionView.SetStatus(Status.Success, "RefreshServers successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "RefreshServers failed.", e);
            }
        }
    }
}
#endif
