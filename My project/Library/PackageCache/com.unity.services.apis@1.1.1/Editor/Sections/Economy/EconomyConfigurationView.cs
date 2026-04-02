using System;
using System.Threading.Tasks;

namespace Unity.Services.Apis.Sample
{
    class EconomyConfigurationView : EditorView
    {
        EconomyController Economy => App.Economy;

        readonly EconomySectionView SectionView;
        UIElement ResourcesContainer;

        public EconomyConfigurationView(App app, EconomySectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            Economy.ResourcesChanged += RefreshResources;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.EconomyAdmin).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                using (UI.HeaderPanel().Scope())
                {
                    UI.H5($"Configuration");
                    UI.Flex();
                    UI.Button("Get", GetResourcesAsync).SetWidth(100);
                }

                ResourcesContainer = UI.ContentPanel();
            }
        }

        void RefreshResources()
        {
            ResourcesContainer.Clear();

            using (ResourcesContainer.Scope())
            {
                CreateInventoryItemsGUI();
                CreateCurrenciesGUI();
                CreateVirtualPurchasesGUI();
                CreateRealMoneyPurchasesGUI();
            }
        }

        void CreateInventoryItemsGUI()
        {
            UI.H5("Inventory Items");

            if (Economy.Configuration.InventoryItems.Count == 0)
            {
                UI.Label("- No Inventory Items.");
            }

            foreach (var tuple in Economy.Configuration.InventoryItems)
            {
                var resource = tuple.Item1;

                using (UI.Panel().SetMarginBottom(10).Scope())
                {
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Id:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Id}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Name:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Name}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Created:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Created?.Date}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Modified:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Modified?.Date}");
                    }
                }
            }
        }

        void CreateCurrenciesGUI()
        {
            UI.H5("Currencies");

            if (Economy.Configuration.Currencies.Count == 0)
            {
                UI.Label("- No Currencies.");
            }

            foreach (var tuple in Economy.Configuration.Currencies)
            {
                var resource = tuple.Item1;

                using (UI.Panel().SetMarginBottom(10).Scope())
                {
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Id:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Id}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Name:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Name}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Initial:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Initial}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Max:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Max}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Created:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Created?.Date}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Modified:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Modified?.Date}");
                    }
                }
            }
        }

        void CreateVirtualPurchasesGUI()
        {
            UI.H5("Virtual Purchases");

            if (Economy.Configuration.VirtualPurchases.Count == 0)
            {
                UI.Label("- No Virtual Purchases.");
            }

            foreach (var tuple in Economy.Configuration.VirtualPurchases)
            {
                var resource = tuple.Item1;

                using (UI.Panel().SetMarginBottom(10).Scope())
                {
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Id:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Id}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Name:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Name}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Created:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Created?.Date}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Modified:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Modified?.Date}");
                    }

                    UI.H5("Costs");

                    foreach (var cost in resource.Costs)
                    {
                        using (UI.Panel().SetMarginBottom(10).Scope())
                        {
                            using (UI.Block().Scope())
                            {
                                UI.Label($"ResourceId:").SetWidth(100);
                                UI.SelectableLabel($"{cost.ResourceId}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label($"Amount:").SetWidth(100);
                                UI.SelectableLabel($"{cost.Amount}");
                            }
                        }
                    }
                }
            }
        }

        void CreateRealMoneyPurchasesGUI()
        {
            UI.H5("Real Money Purchases");

            if (Economy.Configuration.RealMoneyPurchases.Count == 0)
            {
                UI.Label("- No Real Money Purchases.");
            }

            foreach (var tuple in Economy.Configuration.RealMoneyPurchases)
            {
                var resource = tuple.Item1;

                using (UI.Panel().SetMarginBottom(10).Scope())
                {
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Id:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Id}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Name:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Name}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"AppleAppStore:").SetWidth(100);
                        UI.SelectableLabel($"{resource.StoreIdentifiers.AppleAppStore}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"GooglePlayStore:").SetWidth(100);
                        UI.SelectableLabel($"{resource.StoreIdentifiers.GooglePlayStore}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Created:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Created?.Date}");
                    }
                    using (UI.Block().Scope())
                    {
                        UI.Label($"Modified:").SetWidth(100);
                        UI.SelectableLabel($"{resource.Modified?.Date}");
                    }

                    UI.H5("Rewards");

                    foreach (var reward in resource.Rewards)
                    {
                        using (UI.Panel().SetMarginBottom(10).Scope())
                        {
                            using (UI.Block().Scope())
                            {
                                UI.Label($"ResourceId:").SetWidth(100);
                                UI.SelectableLabel($"{reward.ResourceId}");
                            }
                            using (UI.Block().Scope())
                            {
                                UI.Label($"Amount:").SetWidth(100);
                                UI.SelectableLabel($"{reward.Amount}");
                            }
                        }
                    }
                }
            }
        }

        async Task GetResourcesAsync()
        {
            try
            {
                await Economy.GetPublishedResourcesAsync();
                SectionView.SetStatus(Status.Success, "GetPublishedResources successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "GetPublishedResources failed.", e);
            }
        }
    }
}
