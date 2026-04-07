using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Games;
public class ConfigManager : Singleton<ConfigManager>
{
    public List<ShopJsonData> shopJsonDatas = new List<ShopJsonData>();

    public void Start()
    {
        // 这里应该是加载配置文件的代码
        // 例如：LoadShopConfig();
        // 从文件系统加载 good.json
        string configPath = "C:\\Users\\ASUS\\Downloads\\ShiSan_Server (1)\\ShiSan_Server\\good.json"; // 相对路径，可以根据实际位置调整

        if (File.Exists(configPath))
        {
            string jsonString = File.ReadAllText(configPath);
            shopJsonDatas = JsonConvert.DeserializeObject<List<ShopJsonData>>(jsonString);
            Console.WriteLine($"成功加载 {shopJsonDatas.Count} 个物品配置");
        }
        else
        {
            Console.WriteLine($"配置文件不存在: {configPath}");
            // 可以在这里创建默认配置或抛出异常
        }
    }

    public List<CShopGoodsData> RandomShopTypeShopDatas()
    {
        // 实现随机商店数据
        return new List<CShopGoodsData>();
    }
    //public int itemID;
    //public string itemName;
    //public string icon;
    //public string des;
    //public int maxStack;
    //public int rarity;
    //public string equipType;
    //public int gridID;
    //public long itemUID;       // 物品唯一ID
    //public int count;          // 物品数量
    //public int level;          // 物品等级
    //public int quality;        // 物品品质
    //public int combatPower;    // 战力值
    //public int useLevel;       // 使用等级
    //public string description; // 物品描述
    public List<CBagGoodsData> RandomBagDatas()
    {
        List<CBagGoodsData> list = new List<CBagGoodsData>();
        for (int i = 0; i < shopJsonDatas.Count; i++)
        {
            list.Add(new CBagGoodsData()
            {
                itemID = int.Parse(shopJsonDatas[i].id),
                itemName = shopJsonDatas[i].name,
                icon = shopJsonDatas[i].icon,
                des = shopJsonDatas[i].Des,
                maxStack = 99,
                rarity = 0,
                count=1,
                level=20,
                quality=1,
                useLevel=1,
                description = shopJsonDatas[i].Des,
                combatPower = int.Parse(shopJsonDatas[i].power),
                equipType = shopJsonDatas[i].equipType,
                gridID = i
            });
        }
        Console.WriteLine(list.Count);
        return list;
    }
}

// 商店物品数据类
public class CShopGoodsData
{
    public int gridID;
    public int goodID;
    public int goodLevel;
    public string goodPower;
    public int goodBuyCount;
    public int goodMaxBuyCount;
}

// 背包物品数据类
public class CBagGoodsData
{
    public int itemID;
    public string itemName;
    public string icon;
    public string des;
    public int maxStack;
    public int rarity;
    public string equipType;
    public int gridID;
    public long itemUID;       // 物品唯一ID
    public int count;          // 物品数量
    public int level;          // 物品等级
    public int quality;        // 物品品质
    public int combatPower;    // 战力值
    public int useLevel;       // 使用等级
    public string description; // 物品描述
}

// 商店配置数据类
public class ShopJsonData
{
    public string id;
    public string name;
    public string icon;
    public string inventoryType;
    public string equipType;
    public string sale;
    public string starLevel;
    public string quality;
    public string damage;
    public string hp;
    public string power;
    public string Des;
}