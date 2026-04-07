using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ShiSan_Server
{
    public class BagManager : Singleton<BagManager>
    {
        public void Start()
        {
            // 使用 NetID 中定义的消息ID
            MessageControll.GetInstance().AddListener(NetID.C_To_S_GetBagData_Message, C_To_S_GetBagData_Message_Handle);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_SortBagData_Message, C_To_S_SortBagData_Message_Handle);
            MessageControll.GetInstance().AddListener(NetID.C_To_S_SellItem_Message, C_To_S_SellItem_Message_Handle);
        }

        #region 处理背包数据请求
        private void C_To_S_GetBagData_Message_Handle(object obj)
        {
            object[] parList = obj as object[];
            byte[] netData = parList[0] as byte[];
            Client client = parList[1] as Client;

            try
            {
                // 注意：根据 Bag.cs 协议，客户端实际发送的是 C_To_S_Bags 消息
                C_To_S_Bags request = C_To_S_Bags.Parser.ParseFrom(netData);

                // 获取玩家背包数据（根据背包类型筛选）
                List<CBagGoodsData> bagGoodsList = GetPlayerBagItems(client.ip, request.BagType);

                // 构建响应（使用 S_To_C_Bags 消息格式，与客户端预期一致）
                S_To_C_Bags response = new S_To_C_Bags
                {
                    BagType = request.BagType
                };

                // 添加物品数据（需填充所有客户端需要的字段）
                foreach (var good in bagGoodsList)
                {
                    // 注意：必须与客户端 BagUI.cs 中解析的字段对应
                    BagGoodsData data = new BagGoodsData
                    {
                        ItemUID = good.itemUID,
                        ItemID = good.itemID,
                        ItemName = good.itemName,
                        Icon = good.icon,
                        Description = good.description, // 客户端使用的字段名
                        MaxStack = good.maxStack,
                        Quality = good.quality,       // 对应客户端的 ShopData.Quality
                        EquipType = good.equipType,
                        GridID = good.gridID,
                        Count = good.count,
                        Level = good.level,
                        CombatPower = good.combatPower,
                        UseLevel = good.useLevel
                    };
                    response.GoodsList.Add(data);
                }

                // 发送响应
                NetManager.GetInstance().SendNetMessage(client.st, NetID.S_To_C_GetBagData_Message, response.ToByteArray());
            }
            catch (Exception e)
            {
                Console.WriteLine($"处理背包数据请求异常: {e.Message}");
            }
        }
        #endregion

        #region 处理整理背包请求
        private void C_To_S_SortBagData_Message_Handle(object obj)
        {
            object[] parList = obj as object[];
            byte[] netData = parList[0] as byte[];
            Client client = parList[1] as Client;

            try
            {
                // 解析整理请求（使用 Bag.cs 中定义的 C_2_S_SortBagData）
                C_2_S_SortBagData request = C_2_S_SortBagData.Parser.ParseFrom(netData);

                // 获取玩家对应背包类型的物品
                List<CBagGoodsData> bagGoodsList = GetPlayerBagItems(client.ip, request.BagType);

                // 执行排序
                List<CBagGoodsData> sortedList = SortBagItems(bagGoodsList, request.SortRule, out int mergeCount);

                // 保存排序后的数据
                SavePlayerBagItems(client.ip, request.BagType, sortedList);

                // 构建响应（使用 S_2_C_SortBagData 消息格式）
                S_2_C_SortBagData response = new S_2_C_SortBagData
                {
                    BagType = request.BagType,
                    MergeCount = mergeCount
                };

                // 添加排序后的物品
                foreach (var good in sortedList)
                {
                    BagGoodsData data = new BagGoodsData
                    {
                        ItemUID = good.itemUID,
                        ItemID = good.itemID,
                        ItemName = good.itemName,
                        Icon = good.icon,
                        Description = good.description,
                        MaxStack = good.maxStack,
                        Quality = good.quality,
                        EquipType = good.equipType,
                        GridID = good.gridID,
                        Count = good.count,
                        Level = good.level,
                        CombatPower = good.combatPower,
                        UseLevel = good.useLevel
                    };
                    response.NewItemList.Add(data);
                }

                // 发送响应
                NetManager.GetInstance().SendNetMessage(client.st, NetID.S_To_C_SortBagData_Message, response.ToByteArray());
            }
            catch (Exception e)
            {
                Console.WriteLine($"处理整理背包请求异常: {e.Message}");
            }
        }
        #endregion

        #region 处理出售物品请求
        private void C_To_S_SellItem_Message_Handle(object obj)
        {
            object[] parList = obj as object[];
            byte[] netData = parList[0] as byte[];
            Client client = parList[1] as Client;

            try
            {
                // 解析出售请求（使用 Bag.cs 中定义的 C_2_S_SellItem）
                C_2_S_SellItem request = C_2_S_SellItem.Parser.ParseFrom(netData);

                // 构建响应
                S_2_C_SellItem response = new S_2_C_SellItem();

                if (request.AutoSell)
                {
                    // 一键出售处理
                    ProcessQuickSell(client, request.SellQuality, response);
                }
                else
                {
                    // 普通出售处理
                    ProcessNormalSell(client, request.SellItems.ToList(), response);
                }

                // 发送响应
                NetManager.GetInstance().SendNetMessage(client.st, NetID.S_To_C_SellItem_Message, response.ToByteArray());
            }
            catch (Exception e)
            {
                Console.WriteLine($"处理出售物品请求异常: {e.Message}");
            }
        }
        #endregion

        #region 工具方法
        // 排序背包物品
        private List<CBagGoodsData> SortBagItems(List<CBagGoodsData> items, SortRule sortRule, out int mergeCount)
        {
            mergeCount = 0;

            if (items == null || items.Count == 0)
                return items;

            List<CBagGoodsData> result = new List<CBagGoodsData>();

            // 1. 合并相同物品
            Dictionary<int, List<CBagGoodsData>> sameItems = new Dictionary<int, List<CBagGoodsData>>();

            foreach (var item in items)
            {
                if (!sameItems.ContainsKey(item.itemID))
                    sameItems[item.itemID] = new List<CBagGoodsData>();
                sameItems[item.itemID].Add(item);
            }

            // 2. 处理每个物品组
            foreach (var kvp in sameItems)
            {
                int totalCount = kvp.Value.Sum(x => x.count);
                CBagGoodsData firstItem = kvp.Value.First();

                // 记录合并次数
                if (kvp.Value.Count > 1)
                    mergeCount += (kvp.Value.Count - 1);

                // 处理堆叠
                int maxStack = firstItem.maxStack;
                if (maxStack <= 0) maxStack = 99; // 与客户端默认堆叠数一致

                while (totalCount > 0)
                {
                    int stackCount = Math.Min(totalCount, maxStack);
                    totalCount -= stackCount;

                    result.Add(new CBagGoodsData
                    {
                        itemID = firstItem.itemID,
                        itemName = firstItem.itemName,
                        icon = firstItem.icon,
                        description = firstItem.description,
                        maxStack = firstItem.maxStack,
                        quality = firstItem.quality,
                        equipType = firstItem.equipType,
                        gridID = result.Count, // 重新分配格子ID
                        itemUID = GenerateItemUID(), // 生成新的唯一ID
                        count = stackCount,
                        level = firstItem.level,
                        combatPower = firstItem.combatPower,
                        useLevel = firstItem.useLevel
                    });
                }
            }

            // 3. 排序（支持客户端 BagUI.cs 中的 Sale 价格排序）
            switch (sortRule)
            {
                case SortRule.Price:
                    result = result.OrderByDescending(x => GetItemPrice(x.itemID)).ToList();
                    break;
                case SortRule.Level:
                    result = result.OrderByDescending(x => x.level).ToList();
                    break;
                default:
                    result = result.OrderBy(x => x.itemID).ToList();
                    break;
            }

            return result;
        }

        // 普通出售处理
        private void ProcessNormalSell(Client client, List<SellItemData> sellItems, S_2_C_SellItem response)
        {
            int totalEarned = 0;
            List<CBagGoodsData> allItems = GetPlayerAllBagItems(client.ip);
            List<CBagGoodsData> remainingItems = new List<CBagGoodsData>();

            // 复制所有物品
            foreach (var item in allItems)
            {
                remainingItems.Add(new CBagGoodsData
                {
                    itemID = item.itemID,
                    itemName = item.itemName,
                    icon = item.icon,
                    description = item.description,
                    maxStack = item.maxStack,
                    quality = item.quality,
                    equipType = item.equipType,
                    gridID = item.gridID,
                    itemUID = item.itemUID,
                    count = item.count,
                    level = item.level,
                    combatPower = item.combatPower,
                    useLevel = item.useLevel
                });
            }

            // 处理每个出售项
            foreach (var sellItem in sellItems)
            {
                // 找到要出售的物品
                var targetItem = remainingItems.FirstOrDefault(x =>
                    x.gridID == sellItem.GirdID &&
                    x.itemID == sellItem.ItemID);

                if (targetItem != null && targetItem.count >= sellItem.SellCount)
                {
                    // 计算出售价格
                    int price = GetItemSellPrice(targetItem.itemID, targetItem.quality);
                    int earned = price * sellItem.SellCount;
                    totalEarned += earned;

                    // 更新或移除物品
                    if (targetItem.count == sellItem.SellCount)
                    {
                        remainingItems.Remove(targetItem);
                    }
                    else
                    {
                        targetItem.count -= sellItem.SellCount;
                    }
                }
            }

            // 更新玩家货币
            UpdatePlayerCurrency(client.ip, 1, totalEarned);

            // 保存背包数据
            SaveAllBagItems(client.ip, remainingItems);

            // 设置响应
            response.Success = true;
            response.EarnedGold = totalEarned;
        }

        // 一键出售处理
        private void ProcessQuickSell(Client client, int sellQuality, S_2_C_SellItem response)
        {
            List<CBagGoodsData> allItems = GetPlayerAllBagItems(client.ip);
            List<CBagGoodsData> remainingItems = new List<CBagGoodsData>();
            int totalEarned = 0;

            foreach (var item in allItems)
            {
                if (item.quality <= sellQuality && IsItemSellable(item.itemID))
                {
                    int price = GetItemSellPrice(item.itemID, item.quality);
                    totalEarned += price * item.count;
                }
                else
                {
                    remainingItems.Add(item);
                }
            }

            UpdatePlayerCurrency(client.ip, 1, totalEarned);
            SaveAllBagItems(client.ip, remainingItems);

            response.Success = true;
            response.EarnedGold = totalEarned;
        }

        // 获取物品出售价格（基于品质）
        private int GetItemSellPrice(int itemID, int quality)
        {
            // 从配置中获取基础价格
            ShopJsonData config = ConfigManager.GetInstance().shopJsonDatas.FirstOrDefault(x => x.id == itemID.ToString());
            if (config != null && int.TryParse(config.sale, out int basePrice))
            {
                return basePrice * (quality + 1);
            }
            return 10 * (quality + 1); // 默认价格
        }

        // 获取物品价格（用于排序）
        private int GetItemPrice(int itemID)
        {
            ShopJsonData config = ConfigManager.GetInstance().shopJsonDatas.FirstOrDefault(x => x.id == itemID.ToString());
            if (config != null && int.TryParse(config.sale, out int price))
            {
                return price;
            }
            return 0;
        }

        // 判断物品是否可出售
        private bool IsItemSellable(int itemID)
        {
            // 可根据配置添加不可出售的物品
            return true;
        }

        // 生成物品唯一ID
        private long GenerateItemUID()
        {
            return DateTime.Now.Ticks;
        }
        #endregion

        #region 数据库方法
        // 获取玩家背包物品（根据背包类型筛选）
        private List<CBagGoodsData> GetPlayerBagItems(string ip, BagType bagType)
        {
            List<CBagGoodsData> allItems = GetPlayerAllBagItems(ip);
            List<CBagGoodsData> filteredItems = new List<CBagGoodsData>();

            foreach (var item in allItems)
            {
                switch (bagType)
                {
                    case BagType.All:
                        filteredItems.Add(item);
                        break;
                    case BagType.Weapon:
                        if (item.equipType == "武器")
                            filteredItems.Add(item);
                        break;
                    case BagType.Material:
                        if (item.equipType == "宝箱")
                            filteredItems.Add(item);
                        break;
                    case BagType.Armor:
                        if (item.equipType != "武器" && item.equipType != "药品" && item.equipType != "宝箱")
                            filteredItems.Add(item);
                        break;
                    case BagType.Medicine:
                        if (item.equipType == "药品")
                            filteredItems.Add(item);
                        break;
                }
            }

            return filteredItems;
        }

        // 获取玩家所有背包物品
        private List<CBagGoodsData> GetPlayerAllBagItems(string ip)
        {
            // 实际项目应从数据库读取，此处使用配置模拟数据
            return ConfigManager.GetInstance().RandomBagDatas();
        }

        // 保存玩家背包物品
        private void SavePlayerBagItems(string ip, BagType bagType, List<CBagGoodsData> items)
        {
            // TODO: 保存到数据库
            Console.WriteLine($"保存玩家 {ip} 的 {bagType} 背包数据，物品数量：{items.Count}");
        }

        // 保存所有背包物品
        private void SaveAllBagItems(string ip, List<CBagGoodsData> items)
        {
            // TODO: 保存到数据库
            Console.WriteLine($"保存玩家 {ip} 的所有背包数据，物品数量：{items.Count}");
        }

        // 更新玩家货币
        private void UpdatePlayerCurrency(string ip, int currencyType, int amount)
        {
            // TODO: 更新数据库
            Console.WriteLine($"更新玩家 {ip} 的货币类型 {currencyType}，增加 {amount}");
        }
        #endregion
    }
}