using System;
using System.Collections.Generic;
using Unity.Services.Apis.Admin.Economy;

namespace Unity.Services.Apis.Sample
{
    class EconomyConfiguration
    {
        public List<Tuple<CurrencyItemResponse, bool>> Currencies { get; private set; } = new List<Tuple<CurrencyItemResponse, bool>>();
        public List<Tuple<InventoryItemResponse, bool>> InventoryItems { get; private set; } = new List<Tuple<InventoryItemResponse, bool>>();
        public List<Tuple<RealMoneyPurchaseResourceResponse, bool>> RealMoneyPurchases { get; private set; } = new List<Tuple<RealMoneyPurchaseResourceResponse, bool>>();
        public List<Tuple<VirtualPurchaseResourceResponse, bool>> VirtualPurchases { get; private set; } = new List<Tuple<VirtualPurchaseResourceResponse, bool>>();

        public void Clear()
        {
            Currencies.Clear();
            InventoryItems.Clear();
            RealMoneyPurchases.Clear();
            VirtualPurchases.Clear();
        }

        public void ProcessResources(List<GetResourcesResponseResultsInner> resources)
        {
            foreach (var resource in resources)
            {
                if (resource.ActualInstance.GetType() == typeof(CurrencyItemResponse))
                {
                    Currencies.Add(new Tuple<CurrencyItemResponse, bool>(resource.GetCurrencyItemResponse(), false));
                }
                else if (resource.ActualInstance.GetType() == typeof(InventoryItemResponse))
                {
                    InventoryItems.Add(new Tuple<InventoryItemResponse, bool>(resource.GetInventoryItemResponse(), false));
                }
                else if (resource.ActualInstance.GetType() == typeof(RealMoneyPurchaseResourceResponse))
                {
                    RealMoneyPurchases.Add(new Tuple<RealMoneyPurchaseResourceResponse, bool>(resource.GetRealMoneyPurchaseResourceResponse(), false));
                }
                else if (resource.ActualInstance.GetType() == typeof(VirtualPurchaseResourceResponse))
                {
                    VirtualPurchases.Add(new Tuple<VirtualPurchaseResourceResponse, bool>(resource.GetVirtualPurchaseResourceResponse(), false));
                }
            }
        }
    }
}
