using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    public GameObject bagUI;
    public Transform bagParent;
    public List<BagItem> bagItems = new List<BagItem>();
    public Button allBtn, weaponBtn, materialBtn, armorBtn, medicineBtn, sortBtn, sellBtn;
    public static BagUI ins;
    public List<BagData> BagDatas = new List<BagData>();
    public GameObject tips;
    [SerializeField] private Image tipsIcon;
    [SerializeField] private Text tipsName;
    [SerializeField] private Text tipsDes;
    [SerializeField] private Text tipsStats;
    
    // 当前选中的背包类型
    private BagType currentBagType = BagType.All;
    
    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
    }
    
    void Start()
    {
        // 只添加一次消息监听
        MessageControll.Instance.AddListener(NetID.S_To_C_GetBagData_Message, S_To_C_GetBagData);
        MessageControll.Instance.AddListener(NetID.S_To_C_SortBagData_Message, S_To_C_SortBagData);
        MessageControll.Instance.AddListener(NetID.S_To_C_SellItem_Message, S_To_C_SellItem);
        
        // 初始化背包格子（只创建一次）
        InitializeBagSlots();
        
        // 延迟请求背包数据
        Invoke("RequestAllBagData", 0.5f);
        
        // 设置按钮事件
        allBtn.onClick.AddListener(() =>
        {
            currentBagType = BagType.All;
            RequestBagData(BagType.All);
        });
        
        weaponBtn.onClick.AddListener(() =>
        {
            currentBagType = BagType.Weapon;
            RequestBagData(BagType.Weapon);
        });
        
        materialBtn.onClick.AddListener(() =>
        {
            currentBagType = BagType.Material;
            RequestBagData(BagType.Material);
        });
        
        armorBtn.onClick.AddListener(() =>
        {
            currentBagType = BagType.Armor;
            RequestBagData(BagType.Armor);
        });
        
        medicineBtn.onClick.AddListener(() =>
        {
            currentBagType = BagType.Medicine;
            RequestBagData(BagType.Medicine);
        });
        
        sortBtn.onClick.AddListener(() =>
        {
            // 发送整理背包请求到服务器
            C_2_S_SortBagData request = new C_2_S_SortBagData
            {
                BagType = currentBagType,
                SortRule = SortRule.Price // 按价格排序
            };
            NetManager.Instance.SendNetMessage(NetID.C_To_S_SortBagData_Message, request.ToByteArray());
        });
        
        // 一键出售按钮
        if (sellBtn != null)
        {
            sellBtn.onClick.AddListener(() =>
            {
                ShowSellPanel();
            });
        }
    }
    
    // 初始化背包格子
    private void InitializeBagSlots()
    {
        // 先清除已存在的格子
        foreach (Transform child in bagParent)
        {
            Destroy(child.gameObject);
        }
        bagItems.Clear();
        
        // 创建30个背包格子
        for (int i = 0; i < 30; i++)
        {
            var bagItem = Instantiate(Resources.Load<BagItem>("BagItem"), bagParent);
            bagItems.Add(bagItem);
        }
    }
    
    // 统一请求背包数据的方法
    private void RequestBagData(BagType bagType)
    {
        C_To_S_Bags c_Data = new C_To_S_Bags
        {
            BagType = bagType
        };
        NetManager.Instance.SendNetMessage(NetID.C_To_S_GetBagData_Message, c_Data.ToByteArray());
    }
    
    // 获取全部背包数据
    private void RequestAllBagData()
    {
        RequestBagData(BagType.All);
    }
    
    // 处理服务器返回的背包数据
    private void S_To_C_GetBagData(object obj)
    {
        object[] data = obj as object[];
        byte[] netData = data[0] as byte[];
        S_To_C_Bags s_data = S_To_C_Bags.Parser.ParseFrom(netData);
        
        // 清空当前数据
        BagDatas.Clear();
        
        // 处理服务器返回的物品列表
        for (int i = 0; i < s_data.GoodsList.Count; i++)
        {
            var goods = s_data.GoodsList[i];
            
            // 查找物品配置
            ShopData shop = ConfigManager.Instance.FindData(goods.ItemID);
            if (shop == null)
            {
                Debug.LogWarning($"未找到物品配置: {goods.ItemID}");
                continue;
            }
            
            // 创建BagData
            BagData bag = new BagData
            {
                ID = i,
                ItemUID = goods.ItemUID,
                ItemID = goods.ItemID,
                Count = goods.Count,
                Level = goods.Level,
                CombatPower = goods.CombatPower,
                UseLevel = goods.UseLevel,
                MaxStack = goods.MaxStack,
                Quality = goods.Quality,
                Description = goods.Description,
                Shop = shop
            };
            
            // 根据背包类型筛选
            switch (s_data.BagType)
            {
                case BagType.All:
                    BagDatas.Add(bag);
                    break;
                    
                case BagType.Material:
                    if (shop.EquipType == "宝箱")
                    {
                        BagDatas.Add(bag);
                    }
                    break;
                    
                case BagType.Weapon:
                    if (shop.EquipType == "武器")
                    {
                        BagDatas.Add(bag);
                    }
                    break;
                    
                case BagType.Armor:
                    if (shop.EquipType != "武器" && shop.EquipType != "宝箱" && shop.EquipType != "药品")
                    {
                        BagDatas.Add(bag);
                    }
                    break;
                    
                case BagType.Medicine:
                    if (shop.EquipType == "药品")
                    {
                        BagDatas.Add(bag);
                    }
                    break;
                    
                default:
                    BagDatas.Add(bag);
                    break;
            }
        }
        
        // 刷新背包UI
        BagRefresh();
    }
    
    // 处理服务器返回的整理数据
    private void S_To_C_SortBagData(object obj)
    {
        object[] data = obj as object[];
        byte[] netData = data[0] as byte[];
        S_2_C_SortBagData s_data = S_2_C_SortBagData.Parser.ParseFrom(netData);
        
        // 显示合并提示
        if (s_data.MergeCount > 0)
        {
            Debug.Log($"整理完成，合并了 {s_data.MergeCount} 个物品");
        }
        
        // 重新请求当前背包类型的数据
        RequestBagData(currentBagType);
    }
    
    // 处理服务器返回的出售结果
    private void S_To_C_SellItem(object obj)
    {
        object[] data = obj as object[];
        byte[] netData = data[0] as byte[];
        S_2_C_SellItem s_data = S_2_C_SellItem.Parser.ParseFrom(netData);
        
        if (s_data.Success)
        {
            Debug.Log($"出售成功，获得金币: {s_data.EarnedGold}");
            // 重新请求背包数据
            RequestBagData(currentBagType);
        }
        else
        {
            Debug.LogError("出售失败");
        }
    }
    
    // 刷新背包UI
    private void BagRefresh()
    {
        for (int i = 0; i < bagItems.Count; i++)
        {
            if (i < BagDatas.Count)
            {
                bagItems[i].OnInit(i, BagDatas[i]);
            }
            else
            {
                bagItems[i].OnInit(i, null);
            }
        }
    }
    
    // 显示物品详情
    public void OpenTips(BagData data)
    {
        if (data == null || data.Shop == null) return;
        
        tipsIcon.sprite = Resources.Load<Sprite>("icon/" + data.Shop.Icon);
        tipsName.text = data.Shop.Name;
        tipsDes.text = data.Description ?? data.Shop.Des;
        
        // 显示物品属性
        if (tipsStats != null)
        {
            string statsText = "";
            if (data.Level > 0) statsText += $"等级: {data.Level}\n";
            if (data.CombatPower > 0) statsText += $"战斗力: {data.CombatPower}\n";
            if (data.UseLevel > 0) statsText += $"使用等级: {data.UseLevel}\n";
            if (!string.IsNullOrEmpty(data.Shop.Damage)) statsText += $"伤害: {data.Shop.Damage}\n";
            if (!string.IsNullOrEmpty(data.Shop.Hp)) statsText += $"生命: {data.Shop.Hp}\n";
            if (!string.IsNullOrEmpty(data.Shop.Power)) statsText += $"力量: {data.Shop.Power}\n";
            if (!string.IsNullOrEmpty(data.Shop.Sale)) statsText += $"出售价格: {data.Shop.Sale}\n";
            
            tipsStats.text = statsText;
        }
        
        tips.gameObject.SetActive(true);
    }
    
    // 隐藏物品详情
    public void HideTips()
    {
        tips.gameObject.SetActive(false);
    }
    
    // 显示出售面板
    private void ShowSellPanel()
    {
        // 实现出售面板逻辑
        Debug.Log("显示出售面板");
    }
    
    // 排序回调函数
    private int Temp(BagData x, BagData y)
    {
        if (int.Parse(x.Shop.Sale) > int.Parse(y.Shop.Sale))
        {
            return -1;
        }
        else if (int.Parse(x.Shop.Sale) < int.Parse(y.Shop.Sale))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    
    // 清理
    private void OnDestroy()
    {
        MessageControll.Instance.RemoveListener(NetID.S_To_C_GetBagData_Message, S_To_C_GetBagData);
        MessageControll.Instance.RemoveListener(NetID.S_To_C_SortBagData_Message, S_To_C_SortBagData);
        MessageControll.Instance.RemoveListener(NetID.S_To_C_SellItem_Message, S_To_C_SellItem);
    }
}