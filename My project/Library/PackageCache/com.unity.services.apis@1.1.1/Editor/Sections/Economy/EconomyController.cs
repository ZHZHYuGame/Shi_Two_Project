using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Apis.Admin.Economy;
using Unity.Services.Apis.Economy;

namespace Unity.Services.Apis.Sample
{
    class EconomyController : ApiController
    {
        IAdminClient AdminClient { get; set; }
        ITrustedClient TrustedClient { get; set; }
        IEconomyAdminApi EconomyAdminApi => AdminClient.Economy;
        IEconomyCurrenciesApi EconomyCurrenciesApi => TrustedClient.EconomyCurrencies;
        IEconomyInventoryApi EconomyInventoryApi => TrustedClient.EconomyInventory;

        public event Action ResourcesChanged;
        public event Action PlayerInventoryChanged;
        public event Action PlayerCurrenciesChanged;

        public EconomyConfiguration Configuration = new EconomyConfiguration();

        public List<string> AllPlayers { get; } = new List<string>();
        public string SelectedPlayer { get; private set; }

        public Dictionary<string, List<CurrencyBalanceResponse>> AllCurrencies { get; private set; } = new Dictionary<string, List<CurrencyBalanceResponse>>();
        public Dictionary<string, List<InventoryResponse>> AllInventories { get; private set; } = new Dictionary<string, List<InventoryResponse>>();

        public List<CurrencyBalanceResponse> PlayerCurrencies { get; private set; }
        public List<InventoryResponse> PlayerInventory { get; private set; }

        public EconomyController(CloudSettings settings) : base(settings)
        {
        }

        public void Initialize(IAdminClient adminClient, ITrustedClient trustedClient)
        {
            AdminClient = adminClient;
            TrustedClient = trustedClient;
        }

        public async Task GetPublishedResourcesAsync()
        {
            Configuration.Clear();

            var response = await EconomyAdminApi.GetPublishedResources(CloudProjectId, EnvironmentId);
            response.EnsureSuccessful();

            Configuration.ProcessResources(response.Data.Results);
            ResourcesChanged?.Invoke();
        }

        public async Task AddEconomyCurrencyAsync(string name, int initial, int max)
        {
            var request = new CurrencyItemRequest(name: name, type: CurrencyItemRequest.TypeEnum.CURRENCY, initial: initial, max: max);
            await EconomyAdminApi.AddConfigResource(CloudProjectId, EnvironmentId, new AddConfigResourceRequest(request));
        }

        public void AddPlayer(string player)
        {
            AllPlayers.Add(player);
            AllCurrencies[player] = new List<CurrencyBalanceResponse>();
            AllInventories[player] = new List<InventoryResponse>();
            SelectPlayer(player);
        }

        public void SelectPlayer(string player)
        {
            SelectedPlayer = player;
            PlayerCurrencies = null;
            PlayerInventory = null;

            if (player != null)
            {
                PlayerCurrencies = AllCurrencies[player];
                PlayerInventory = AllInventories[player];
            }

            PlayerCurrenciesChanged?.Invoke();
            PlayerInventoryChanged?.Invoke();
        }

        public void RemovePlayer()
        {
            AllPlayers.Remove(SelectedPlayer);
            AllCurrencies.Remove(SelectedPlayer);
            AllInventories.Remove(SelectedPlayer);
            SelectPlayer(null);
        }

        public async Task AuthorizeAsync()
        {
            if (TrustedClient.AccessToken == null)
            {
                await TrustedClient.SignInWithServiceAccount(CloudProjectId, EnvironmentId);
            }
        }

        public async Task LoadCurrenciesAsync()
        {
            await AuthorizeAsync();
            var response = await EconomyCurrenciesApi.GetPlayerCurrencies(CloudProjectId, SelectedPlayer);
            response.EnsureSuccessful();
            AllCurrencies[SelectedPlayer] = response.Data.Results;
            PlayerCurrencies = response.Data.Results;
            PlayerCurrenciesChanged?.Invoke();
        }

        public async Task LoadInventoryAsync()
        {
            await AuthorizeAsync();
            var response = await EconomyInventoryApi.GetPlayerInventory(CloudProjectId, SelectedPlayer);
            response.EnsureSuccessful();
            AllInventories[SelectedPlayer] = response.Data.Results;
            PlayerInventory = response.Data.Results;
            PlayerInventoryChanged?.Invoke();
        }
    }
}
