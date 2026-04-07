using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Message;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
   
    public static BagUI instance;
    private string currentFilter = "全部";
    private bool isFiltering = false; // 是否处于筛选显示模式
    private List<BagItem> filteredItems = new List<BagItem>(); // 缓存筛选后的道具列表
    public List<BagItemUI> bagItems = new List<BagItemUI>(); // 所有格子UI组件
    public Transform bagParent;
    public Button b1, b2, b3, b4, b5;
    public Text cellNum;
    private EquippedData equippedData; //装备数据
    private int totalSlots;                 // 总格子数
    private int unlockedSlots;              // 已解锁格子数
    private Dictionary<string, BagItem> allItemsDict; // uid -> BagItem（所有道具实例）
    private MapField<int, string> slotUids; // 格子索引 -> 道具uid
    public List<EquipItem> equipSlots;
    public int nowMoney = 0;
    public UnityEngine.UI.Text moneyText;
    public GameObject Unlock;

    public GameObject SellPanel;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        MessageControll.GetInstance.AddListener(NetID.S_2_C_BagInit, OnInit);
        // 监听穿脱装备响应（用于增量更新装备栏，背包UI也会受到影响）
        MessageControll.GetInstance.AddListener(NetID.S_2_C_Apparel, OnApparel);
        MessageControll.GetInstance.AddListener(NetID.S_2_C_Dilatation, DilatationUI);
        MessageControll.GetInstance.AddListener(NetID.S_2_C_Money,RefreshUI);
        MessageControll.GetInstance.AddListener(NetID.S_2_C_SellGood,RefreshCell);
        b1.onClick.AddListener(() => SetFilter("全部"));
        b2.onClick.AddListener(() => SetFilter("武器"));
        b3.onClick.AddListener(() => SetFilter("防具"));
        b4.onClick.AddListener(() => SetFilter("药水"));
        b5.onClick.AddListener(() => SetFilter("材料"));
       Invoke("InitMoney",0.5f);
    }

    private void RefreshCell(object obj)
    {
        object[] args = (object[])obj;
        byte[] data = (byte[])args[0];
        S2C_SellGood s_data=S2C_SellGood.Parser.ParseFrom(data);
        slotUids = s_data.MainBag.SlotUids;
        allItemsDict = new Dictionary<string, BagItem>();
        foreach (var item in s_data.AllItems)
        {
            allItemsDict[item.Uid] = item;
        }

        RefreshAllSlots();
    }

    public void InitMoney()
    {
        C2S_GXUI c_data=new C2S_GXUI();
        c_data.ChangeMoney = 0;
        NetManger.GetInstance.SendMessage(NetID.C_2_S_Money, c_data.ToByteArray());
    }
    private void RefreshUI(object obj)
    {
        object[] args = (object[])obj;
        byte[] data = (byte[])args[0];
        S2C_GXUI s_data=S2C_GXUI.Parser.ParseFrom(data);
        GXUI(s_data.Money);
    }

    public void GXUI(int num)
    {
        nowMoney = num;
        moneyText.text = nowMoney.ToString();
    }
    private void DilatationUI(object obj)
    {
        object[] args = (object[])obj;
        byte[] data = (byte[])args[0];
        S2S_Dilatationl s_data=S2S_Dilatationl.Parser.ParseFrom(data);
        totalSlots = s_data.All;
        unlockedSlots = s_data.Now;
        cellNum.text = slotUids.Count + "/" + unlockedSlots;
        for (int i = 0; i < totalSlots; i++)
        {
            if (i >= unlockedSlots)
            {
                bagItems[i].SetLocked(true);
            }
            else
            {
                bagItems[i].SetLocked(false);
            }
        }
    }

    private void SetFilter(string filter)
    {
        currentFilter = filter;
        if (filter == "全部")
        {
            // 退出筛选模式，恢复原始显示
            isFiltering = false;
            RefreshAllSlots(); // 恢复原始格子显示
        }
        else
        {
            // 进入筛选模式，生成紧凑显示
            isFiltering = true;
            RefreshFilteredDisplay();
        }
    }
    private void RefreshFilteredDisplay()
    {
        // 1. 收集符合条件的道具
        filteredItems.Clear();
        for (int i = 0; i < unlockedSlots; i++)
        {
            if (slotUids.TryGetValue(i, out string uid) && allItemsDict.TryGetValue(uid, out BagItem item))
            {
                if (IsItemMatchFilter(item))
                    filteredItems.Add(item);
            }
        }

        // 2. 先清空所有格子
        for (int i = 0; i < unlockedSlots; i++)
            bagItems[i].SetEmpty();

        // 3. 将符合条件的道具紧凑填充到前面
        for (int i = 0; i < filteredItems.Count && i < unlockedSlots; i++)
        {
            bagItems[i].SetItem(filteredItems[i]);
        }

        // 注意：后面多出的格子保持为空（已经SetEmpty了）
    }
   
    // private void OnSellItem(object obj)
    // {
    //     // 解析响应...
    //     S2C_SellItem s_data = ...;
    //     if (s_data.Result == SellResult.SELL_SUCCESS)
    //     {
    //         // 更新本地 allItemsDict 和 slotUids（略）
    //         // ...
    //         // 刷新背包UI显示
    //         BagUI.instance.OnBackpackDataChanged();
    //     }
    // }
    private void OnInit(object obj)
    {
        object[] args = (object[])obj;
        byte[] data = (byte[])args[0];
        S2C_GetBackpackData s_data = S2C_GetBackpackData.Parser.ParseFrom(data);
        
        totalSlots = s_data.MainBag.TotalSlots;
        unlockedSlots = s_data.MainBag.UnlockedSlots;
        slotUids = s_data.MainBag.SlotUids;
        equippedData = s_data.Equipped;   
        allItemsDict = new Dictionary<string, BagItem>();
        foreach (var item in s_data.AllItems)
        {
            allItemsDict[item.Uid] = item;
        }
        // 生成所有格子UI（总格子数）
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject itemObj = Instantiate(Resources.Load<GameObject>("BagItem"), bagParent);
            BagItemUI ui = itemObj.GetComponent<BagItemUI>();
            bagItems.Add(ui);
            // 初始化格子索引
            ui.InitSlot(i, i < unlockedSlots);
        }
        // 刷新所有格子内容
        RefreshAllSlots();
        cellNum.text = slotUids.Count + "/" + unlockedSlots;
        RefreshAllEquipSlots(); 
    }
    /// <summary>
    /// 刷新所有格子（根据 slotUids 和 allItemsDict）
    /// </summary>
    private void RefreshAllSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            if (i >= unlockedSlots)
            {
                bagItems[i].SetLocked(true);
                continue;
            }

            bagItems[i].SetLocked(false);
            if (slotUids.TryGetValue(i, out string uid) && allItemsDict.TryGetValue(uid, out BagItem item))
            {
                // 检查道具是否匹配当前筛选
                if (IsItemMatchFilter(item))
                    bagItems[i].SetItem(item);
                else
                    bagItems[i].SetEmpty(); // 不匹配则显示为空（但数据未丢失）
            }
            else
            {
                bagItems[i].SetEmpty();
            }
        }
    }
    private bool IsItemMatchFilter(BagItem item)
    {
        if (currentFilter == "全部") return true;
        ItemData data = ConfigManger.GetInstance.GetItemData(item.ItemId);
        if (data == null) return false;
        switch (currentFilter)
        {
            case "武器": return data.Type == 1;
            case "防具": return data.Type == 2;
            case "药水": return data.Type == 4;
            case "材料": return data.Type == 8;
            default: return false;
        }
    }
    public void OnDataChanged()
    {
        if (isFiltering && currentFilter != "全部")
            RefreshFilteredDisplay();
        else
            RefreshAllSlots();
    }
    private void OnApparel(object obj)
    {
        print("完全放弃我");
        object[] args = (object[])obj;
        byte[] data = (byte[])args[0];
        S2C_Apparel s_data = S2C_Apparel.Parser.ParseFrom(data);
        if (s_data.Result != EquipResult.EquipSuccess) return;
        if (s_data.Result != EquipResult.EquipSuccess)
        {
            Debug.LogWarning($"装备操作失败: {s_data.Result}");
            return;
        }
         // 方式一：重新请求全量背包数据（简单可靠）
        C2S_GetBackpackData req = new C2S_GetBackpackData();
        NetManger.GetInstance.SendMessage(NetID.C_2_S_BagInit, req.ToByteArray());
    }
    /// <summary>
    /// 判断某个道具是否已被装备
    /// </summary>
    public bool IsItemEquipped(string uid)
    {
        if (equippedData == null) return false;
        return equippedData.Weapon == uid ||
               equippedData.Head == uid ||
               equippedData.Chest == uid ||
               equippedData.Necklace == uid ||
               equippedData.Ring == uid ||
               equippedData.Legs == uid ||
               equippedData.Boots == uid;
    }
    private void RefreshAllEquipSlots()
    {
        if (equipSlots == null || equipSlots.Count == 0) return;
        // 根据 EquipSlot 枚举顺序：Weapon=0, Head=1, Chest=2, Necklace=3, Ring=4, Legs=5, Boots=6
        // 假设 equipSlots 列表索引对应这些顺序
        SetEquipSlot(EquipSlot.Weapon, equippedData.Weapon);
        SetEquipSlot(EquipSlot.Head, equippedData.Head);
        SetEquipSlot(EquipSlot.Chest, equippedData.Chest);
        SetEquipSlot(EquipSlot.Necklace, equippedData.Necklace);
        SetEquipSlot(EquipSlot.Ring, equippedData.Ring);
        SetEquipSlot(EquipSlot.Legs, equippedData.Legs);
        SetEquipSlot(EquipSlot.Boots, equippedData.Boots);
    }
    private void SetEquipSlot(EquipSlot slot, string uid)
    {
        int index = (int)slot;
        if (index >= equipSlots.Count) return;
        EquipItem slotUI = equipSlots[index];
        if (string.IsNullOrEmpty(uid))
        {
            slotUI.Refresh(null);   // 清空显示
        }
        else if (allItemsDict.TryGetValue(uid, out BagItem item))
        {
            slotUI.Init(item);
        }
        else
        {
            slotUI.Refresh(null);
        }
    }
    void Update()
    {
        
    }
}
