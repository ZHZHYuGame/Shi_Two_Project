using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Unity.Services.Apis.Sample
{
    class EconomyPlayerView : EditorView
    {
        EconomyController Economy => App.Economy;

        readonly EconomySectionView SectionView;

        string m_AddPlayerIdPrompt = "";

        UIElement CurrenciesContainer;
        UIElement InventoryContainer;

        public EconomyPlayerView(App app, EconomySectionView sectionView) : base(app)
        {
            SectionView = sectionView;
            Economy.PlayerCurrenciesChanged += RefreshCurrencies;
            Economy.PlayerInventoryChanged += RefreshInventory;
        }

        public override void CreateGUI()
        {
            UI.Snippet().SetText(Snippets.EconomyTrusted).SetPaddingBottom(10);

            using (UI.Element().BindVisibility(App.ValidConfigurationBinding, true).Scope())
            {
                using (UI.Block().SetName("LoadPlayerPanel").Scope())
                {
                    UI.Label("Load Player").SetWidth(100);
                    UI.TextField().BindValue(UI.BindField(this, () => m_AddPlayerIdPrompt)).SetWidth(150);
                    UI.Button("Add", AddPlayer).SetWidth(100);
                }

                UI.Space();

                var binding = UI.BindReadOnly(() => Economy.SelectedPlayer != null);

                using (UI.HeaderPanel().BindVisibility(binding, true).Scope())
                {
                    var selectedIndex = Economy.AllPlayers.IndexOf(Economy.SelectedPlayer);

                    UI.PopupField(Economy.AllPlayers)
                        .BindValue(UI.BindProperty(() => Economy.SelectedPlayer))
                        .RegisterValueChanged((value) => Economy.SelectPlayer(value.newValue))
                        .SetWidth(300);

                    UI.Button("X", Economy.RemovePlayer)
                        .SetWidth(50);
                }
                using (UI.ContentPanel().BindVisibility(binding, true).Scope())
                {
                    CreateGUIPlayer();
                }
            }
        }

        void CreateGUIPlayer()
        {
            using (UI.HeaderPanel().Scope())
            {
                UI.H3($"Currencies");
                UI.Flex();
                UI.Button("Load", LoadCurrenciesAsync).SetWidth(50);
            }

            CurrenciesContainer = UI.ContentPanel();
            UI.Space();

            using (UI.HeaderPanel().Scope())
            {
                UI.H3($"Inventory");
                UI.Flex();
                UI.Button("Load", LoadInventoryAsync).SetWidth(50);
            }

            InventoryContainer = UI.ContentPanel();
        }

        void RefreshCurrencies()
        {
            CurrenciesContainer.Clear();

            using (CurrenciesContainer.Scope())
            {
                if (Economy.PlayerCurrencies != null)
                {
                    if (Economy.PlayerCurrencies.Count > 0)
                    {
                        foreach (var value in Economy.PlayerCurrencies)
                        {
                            using (UI.Block().Scope())
                            {
                                UI.Label(value.CurrencyId).SetWidth(200);
                                UI.Label(value.Balance.ToString());
                            }
                        }
                    }
                    else
                    {
                        UI.Label("- No currencies.");
                    }
                }
            }
        }

        void RefreshInventory()
        {
            InventoryContainer.Clear();

            using (InventoryContainer.Scope())
            {
                if (Economy.PlayerInventory != null)
                {
                    if (Economy.PlayerInventory.Count > 0)
                    {
                        foreach (var value in Economy.PlayerInventory)
                        {
                            using (UI.Block().Scope())
                            {
                                UI.Label(value.InventoryItemId);
                                UI.Label(value.PlayersInventoryItemId);
                            }
                        }
                    }
                    else
                    {
                        UI.Label("- No items.");
                    }
                }
            }
        }

        void AddPlayer()
        {
            if (!string.IsNullOrEmpty(m_AddPlayerIdPrompt))
            {
                Economy.AddPlayer(m_AddPlayerIdPrompt);
            }

            m_AddPlayerIdPrompt = "";
        }

        async Task LoadCurrenciesAsync()
        {
            try
            {
                await Economy.LoadCurrenciesAsync();
                SectionView.SetStatus(Status.Success, "LoadCurrencies successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "LoadCurrencies failed.");
            }
        }

        async Task LoadInventoryAsync()
        {
            try
            {
                await Economy.LoadInventoryAsync();
                SectionView.SetStatus(Status.Success, "LoadInventory successful.");
            }
            catch (Exception e)
            {
                AppLogger.LogError(e.Message);
                SectionView.SetStatus(Status.Error, "LoadInventory failed.");
            }
        }
    }
}
