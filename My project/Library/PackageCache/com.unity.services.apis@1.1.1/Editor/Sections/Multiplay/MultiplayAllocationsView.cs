#if ENABLE_SERVICES_EXPERIMENTAL_APIS
using System;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.Multiplay;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class MultiplayAllocationsView : EditorView
    {
        MultiplayController Multiplay => App.Multiplay;

        readonly MultiplaySectionView SectionView;
        UIElement AllocationsContainer;

        public MultiplayAllocationsView(App app, MultiplaySectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            Multiplay.AllocationsChanged += RefreshAllocationsGUI;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.MultiplayAllocationsAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                UI.Button("Load Allocations", RefreshAllocationsAsync).SetWidth(200).SetMarginBottom(10);

                AllocationsContainer = UI.Element();
            }
        }

        void RefreshAllocationsGUI()
        {
            AllocationsContainer.Clear();

            using (AllocationsContainer.Scope())
            {
                if (Multiplay.Allocations != null)
                {
                    foreach (var allocation in Multiplay.Allocations)
                    {
                        using (UI.HeaderPanel().Scope())
                        {
                            UI.Label(allocation.AllocationId);
                        }
                        using (UI.ContentPanel().Scope())
                        {
                            UI.Label($"BuildConfigurationId: {allocation.BuildConfigurationId}");
                            UI.Label($"RequestId: {allocation.RequestId}");
                            UI.Label($"FleetId: {allocation.FleetId}");
                            UI.Label($"MachineId: {allocation.MachineId}");
                            UI.Label($"RegionId: {allocation.RegionId}");
                            UI.Label($"ServerId: {allocation.ServerId}");
                            UI.Separator();
                            UI.Label($"Created: {allocation.Created}");
                            UI.Label($"Requested: {allocation.Requested}");
                            UI.Label($"Fulfilled: {allocation.Fulfilled}");
                            UI.Separator();
                            UI.Label($"Ipv4: {allocation.Ipv4}");
                            UI.Label($"Ipv6: {allocation.Ipv6}");
                            UI.Label($"GamePort: {allocation.GamePort}");
                        }
                        using (UI.FooterPanel().Scope())
                        {
                            //UI.Button("Deallocate", () => DeallocateAsync(allocation)).SetWidth(100);
                        }

                        UI.Space();
                    }
                }
            }
        }
        async Task RefreshAllocationsAsync()
        {
            try
            {
                await Multiplay.RefreshAllocationsAsync();
                SectionView.SetStatus(Status.Success, "RefreshAllocations successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "RefreshAllocations failed.", e);
            }
        }

        async Task DeallocateAsync(MultiplayAllocationsTestAllocation allocation)
        {
            try
            {
                await Multiplay.DeallocateAsync(allocation);
                SectionView.SetStatus(Status.Success, "RefreshAllocations successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "RefreshAllocations failed.", e);
            }
        }
    }
}
#endif
