using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ShopData
{
    public string Id;
    public string Name;
    public string Icon;
    public string InventoryType;
    public string EquipType;
    public string Sale;
    public string StarLevel;
    public string Quality;
    public string Damage;
    public string Hp;
    public string Power;
    public string Des;
}

public class BagData
{
    public int ID; // 物品在背包中的位置
    public long ItemUID; // 物品唯一ID
    public int ItemID; // 物品配置ID
    public int Count; // 物品数量
    public ShopData Shop; // 物品配置数据
    public int Level; // 物品等级
    public int CombatPower; // 战斗力
    public int UseLevel; // 使用等级
    public int MaxStack; // 最大堆叠数
    public int Quality; // 品质
    public string Description; // 描述
}

public class ConfigManager : Singleton<ConfigManager>
{
    public List<ShopData> shops = new List<ShopData>();
    public List<BagData> bagItems = new List<BagData>();

    public void Init()
    {
        shops = JsonConvert.DeserializeObject<List<ShopData>>(Resources.Load<TextAsset>("good").text);
    }

    public ShopData FindData(int id)
    {
        ShopData data = null;
        for (int i = 0; i < shops.Count; i++)
        {
            if (int.Parse(shops[i].Id) == id)
            {
                data = shops[i];
            }
        }
        return data;
    }
}