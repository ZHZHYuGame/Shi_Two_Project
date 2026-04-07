using System.Text;
using BagNet.Net;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Message;

namespace BagNet;

public class BagManger:SingLeton<BagManger>
{
    public void Start()
    {
        MessageControll.GetInstance.AddListener(NetID.C_2_S_BagInit,BagInit);
        MessageControll.GetInstance.AddListener(NetID.C_2_S_Apparel,ApparelUI);
        MessageControll.GetInstance.AddListener(NetID.C_2_S_Dilatation,DilatationUI);
        MessageControll.GetInstance.AddListener(NetID.C_2_S_Money,GXUIMoneyUI);
        MessageControll.GetInstance.AddListener(NetID.C_2_S_SellGood,GXUINumUI);
    }

    private void GXUINumUI(object obj)
    {
        Console.WriteLine("卖出");
        object[] args = obj as object[];
        byte[] data = args[0] as byte[];
        Client c = args[1] as Client;
        C2S_SellGood c_data=C2S_SellGood.Parser.ParseFrom(data);
        S2C_SellGood s_data=new S2C_SellGood();
       if (!c.userData.AllItems.TryGetValue(c_data.Uid, out BagItem item))
       {
           Console.WriteLine("道具不存在");
           return;
       }

       // 2. 检查数量
       if (item.Count < c_data.Num)
       {
           Console.WriteLine("库存不足");
           return;
       }
       ItemData staticData = ConfigManger.GetInstance.FindItem(item.ItemId);
       if (staticData == null || staticData.SellPrice <= 0)
       {
           Console.WriteLine("道具不可出售");
           return;
       }
       item.Count -= c_data.Num;
       // int gainedGold = staticData.SellPrice * c_data.Num;
       // c.userData.Gold += gainedGold;
       if (item.Count == 0)
       {
           string uid = c_data.Uid;
           c.userData.AllItems.Remove(uid);

           // 查找并删除背包格子中的该 uid
           int slotToRemove = -1;
           foreach (var kv in c.userData.MainBag.SlotUids)
           {
               if (kv.Value == uid)
               {
                   slotToRemove = kv.Key;
                   break;
               }
           }
           if (slotToRemove != -1)
               c.userData.MainBag.SlotUids.Remove(slotToRemove);
       }
       s_data.MainBag = c.userData.MainBag;
       s_data.AllItems.AddRange(c.userData.AllItems.Values); // 正确添加所有道具实例
       
       NetManger.GetInstance.SendNetMessage(c.st,NetID.S_2_C_SellGood,s_data.ToByteArray());
    }

    private void GXUIMoneyUI(object obj)
    {
        Console.WriteLine("金币");
        object[] args = obj as object[];
        byte[] data = args[0] as byte[];
        Client c = args[1] as Client;
        C2S_GXUI c_data=C2S_GXUI.Parser.ParseFrom(data);
        c.userData.Gold += c_data.ChangeMoney;
        S2C_GXUI s_data=new S2C_GXUI();
        s_data.Money=c.userData.Gold;
        NetManger.GetInstance.SendNetMessage(c.st,NetID.S_2_C_Money,s_data.ToByteArray());
    }

    private void DilatationUI(object obj)
    {
        object[] args = obj as object[];
        byte[] data = args[0] as byte[];
        Client c = args[1] as Client;
        C2S_Dilatationl c_data=C2S_Dilatationl.Parser.ParseFrom(data);
        // Console.WriteLine(c_data.Num);
        c.userData.MainBag.UnlockedSlots = c.userData.MainBag.UnlockedSlots+c_data.Num;
        S2S_Dilatationl s_data=new S2S_Dilatationl();
        s_data.All = c.userData.MainBag.TotalSlots;
        s_data.Now = c.userData.MainBag.UnlockedSlots;
        NetManger.GetInstance.SendNetMessage(c.st, NetID.S_2_C_Dilatation, s_data.ToByteArray());

    }

    private void ApparelUI(object obj)
    {
        object[] args = obj as object[];
        byte[] data = args[0] as byte[];
        Client c = args[1] as Client;

        C2S_Apparel req = C2S_Apparel.Parser.ParseFrom(data);
        var user = c.userData;
        if (!user.AllItems.TryGetValue(req.UId, out BagItem item))
        {
            return;
        }
        if (req.IsUnequip)
        {
            Console.WriteLine($"卸下请求: Slot={req.Slot}, UId={req.UId}");
            string oldUid = GetEquippedUid(user.Equipped, req.Slot);
            if (string.IsNullOrEmpty(oldUid))
            {
                SendApparelResponse(c, EquipResult.EquipFailNotExist, req.Slot, "", "");
                return;
            }

            int freeSlot = FindFreeSlot(user.MainBag.SlotUids, user.MainBag.UnlockedSlots);
            if (freeSlot == -1)
            {
                // 背包已满，无法卸下
                SendApparelResponse(c, EquipResult.EquipFailNotExist, req.Slot, "", "");
                return;
            }

            // 从装备位移除，放回背包
            SetEquippedSlot(user.Equipped, req.Slot, "");
            user.MainBag.SlotUids[freeSlot] = oldUid;

            SendApparelResponse(c, EquipResult.EquipSuccess, req.Slot, "", oldUid);
            return;
        }

        if (!req.IsUnequip)
        {
            ItemData staticData = ConfigManger.GetInstance.FindItem(item.ItemId);
            // 3. 装备/替换操作
            if (!CanEquipToSlot(staticData, req.Slot))
            {
                SendApparelResponse(c, EquipResult.EquipFailWrongSlot, req.Slot, "", "");
                return;
            }

            // 等级检查（假设玩家等级在 c.Level）
            if (staticData.MinLevel > c.playerLevel)
            {
                SendApparelResponse(c, EquipResult.EquipFailLevel, req.Slot, "", "");
                return;
            }

            string oldEquipUid = GetEquippedUid(user.Equipped, req.Slot);

            // 从背包中移除新装备
            int oldSlot = FindSlotByUid(user.MainBag.SlotUids, req.UId);
            if (oldSlot == -1)
            {
                SendApparelResponse(c, EquipResult.EquipFailNotExist, req.Slot, "", "");
                return;
            }
            user.MainBag.SlotUids.Remove(oldSlot);

            // 如果有旧装备，放回背包
            if (!string.IsNullOrEmpty(oldEquipUid))
            {
                int freeSlot = FindFreeSlot(user.MainBag.SlotUids, user.MainBag.UnlockedSlots);
                if (freeSlot == -1)
                {
                    // 背包已满，回滚：将新装备放回原格子
                    user.MainBag.SlotUids[oldSlot] = req.UId;
                    SendApparelResponse(c, EquipResult.EquipFailNotExist, req.Slot, "", "");
                    return;
                }
                user.MainBag.SlotUids[freeSlot] = oldEquipUid;
            }

            // 装备新道具
            SetEquippedSlot(user.Equipped, req.Slot, req.UId);

            SendApparelResponse(c, EquipResult.EquipSuccess, req.Slot, req.UId, oldEquipUid);
        }
        
    }

    private void SendApparelResponse(Client c, EquipResult result, EquipSlot slot, string newUid, string oldUid)
    {    S2C_Apparel res = new S2C_Apparel
        {
            Result = result,
            Slot = slot,
            NewItemUid = newUid ?? "",
            OldItemUid = oldUid ?? ""
        };
        Console.WriteLine(res.NewItemUid);
        NetManger.GetInstance.SendNetMessage(c.st, NetID.S_2_C_Apparel, res.ToByteArray());
    }
    //寻找
    int FindFreeSlot(MapField<int, string> slotUids, int unlockedSlots)
    {
        for (int i = 0; i < unlockedSlots; i++)
            if (!slotUids.ContainsKey(i)) return i;
        return -1;
    }
    int FindSlotByUid(MapField<int, string> slotUids, string uid)
    {
        foreach (var kv in slotUids)
            if (kv.Value == uid) return kv.Key;
        return -1;
    }

    private bool CanEquipToSlot(ItemData nowData, EquipSlot slot)
    {
        if (nowData.Type != 1 && nowData.Type != 2)
            return false;
        if (nowData.Type==1)
            return slot == EquipSlot.Weapon;
        if (nowData.Type == 2)
        {
            switch (nowData.SubType)
            {
                case 102: return slot == EquipSlot.Head;
                case 103: return slot == EquipSlot.Chest;
                case 104: return slot == EquipSlot.Necklace;
                case 105: return slot == EquipSlot.Ring;
                case 106:return slot == EquipSlot.Legs;
                case 107: return slot == EquipSlot.Boots;
            }
        }
        return false;
    }
    
    void SetEquippedSlot(EquippedData equipped, EquipSlot slot, string uid)
    {
        switch (slot)
        {
            case EquipSlot.Weapon: equipped.Weapon = uid; break;
            case EquipSlot.Head: equipped.Head = uid; break;
            case EquipSlot.Chest: equipped.Chest = uid; break;
            case EquipSlot.Necklace: equipped.Necklace = uid; break;
            case EquipSlot.Ring: equipped.Ring = uid; break;
            case EquipSlot.Legs: equipped.Legs = uid; break;
            case EquipSlot.Boots: equipped.Boots = uid; break;
        }
    }
    string GetEquippedUid(EquippedData equipped, EquipSlot slot)
    {
        switch (slot)
        {
            case EquipSlot.Weapon: return equipped.Weapon;
            case EquipSlot.Head: return equipped.Head;
            case EquipSlot.Chest: return equipped.Chest;
            case EquipSlot.Necklace: return equipped.Necklace;
            case EquipSlot.Ring: return equipped.Ring;
            case EquipSlot.Legs: return equipped.Legs;
            case EquipSlot.Boots: return equipped.Boots;
            default: return null;
        }
    }
    private void BagInit(object obj)
    {
        var idGen = new SnowflakeIdGenerator(1);
        object[] args = (object[])obj;
        byte[] content = (byte[])args[0];
        Client c = (Client)args[1];
        Console.WriteLine(c.port + " 发送背包请求");
        if (c.userData.MainBag != null && c.userData.AllItems != null)
        {
            S2C_GetBackpackData response2 = new S2C_GetBackpackData();
            response2.MainBag = c.userData.MainBag;
            response2.Equipped = c.userData.Equipped;
            response2.QuickDrugs.AddRange(c.userData.QuickDrugs ?? new List<QuickDrugSlot>());
            response2.AllItems.AddRange(c.userData.AllItems.Values);
            NetManger.GetInstance.SendNetMessage(c.st, NetID.S_2_C_BagInit, response2.ToByteArray());
            return;
        }

        // 创建响应对象
        S2C_GetBackpackData response = new S2C_GetBackpackData();
        //  背包基础数据 ---
        BagData bagData = new BagData();
        bagData.TotalSlots = 50;       // 总格子数
        bagData.UnlockedSlots = 20;    // 已解锁格子数

        //  准备 AllItems 字典和背包格子映射
        Dictionary<string, BagItem> allItems = new Dictionary<string, BagItem>();
        MapField<int, string> slotUids = bagData.SlotUids;
        for (int i = 0; i < ConfigManger.GetInstance.allList.Count; i++)
        {
            ItemData staticItem = ConfigManger.GetInstance.allList[i];
            BagItem bagItem = new BagItem();
            long uid = idGen.NextId();
            bagItem.Uid = uid.ToString();
            bagItem.ItemId = staticItem.Id;
            bagItem.Count = 1;
            bagItem.IsNew = true;

            // 装备类添加 Extra
            if (staticItem.Type == 1 || staticItem.Type == 2)
            {
                ItemExtra extra = new ItemExtra();
                extra.EnhanceLevel = 0;
                extra.RefineLevel = 0;
                extra.SocketedGems.AddRange(new List<int>());
                bagItem.Extra = extra;
            }

            allItems[bagItem.Uid] = bagItem;

          
            if (i < bagData.UnlockedSlots)
            {
                slotUids[i] = bagItem.Uid;
            }
        }

      
        EquippedData equipped = new EquippedData();

       
        List<QuickDrugSlot> quickDrugs = new List<QuickDrugSlot>();
        
        response.MainBag = bagData;
        response.Equipped = equipped;
        response.QuickDrugs.AddRange(quickDrugs);
        response.AllItems.AddRange(allItems.Values);  // 所有道具实例
        
        c.userData.MainBag = bagData;
        c.userData.Equipped = equipped;
        c.userData.AllItems = allItems;
        c.userData.QuickDrugs = quickDrugs;
        c.userData.Gold = 100;
        
        NetManger.GetInstance.SendNetMessage(c.st, NetID.S_2_C_BagInit, response.ToByteArray());
    }
}