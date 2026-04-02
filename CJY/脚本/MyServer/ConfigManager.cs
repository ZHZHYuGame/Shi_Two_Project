using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Games;
public class ConfigManager:Singleton<ConfigManager>
{
    public List<ShopJsonData> shopJsonDatas = new List<ShopJsonData>(19);

    public void Start()
    {
        shopJsonDatas = JsonConvert.DeserializeObject<List<ShopJsonData>>(File.ReadAllText("C:\\Users\\ASUS\\Downloads\\ShiSan_Server (1)\\ShiSan_Server\\good.json"));
        //MessageControll.GetInstance().AddListener(NetID.C_To_S_Shop_Type_Datas_Message, C_To_S_Shop_Type_Datas_Message_Handle);
        for (int i = 0; i < shopJsonDatas.Count; i++)
        {
            Console.WriteLine(shopJsonDatas[i].name);
        }
    }
    /// <summary>
    /// 随机商品
    /// </summary>
    /// <returns></returns>
    public List<CShopGoodsData> RandomShopTypeShopDatas()
    {

        List<CShopGoodsData> list = new List<CShopGoodsData>();
        for (int i = 0; i < shopJsonDatas.Count; i++)
        {
            list.Add(new CShopGoodsData() { gridID = i, goodID = int.Parse(shopJsonDatas[i].id), goodLevel = 1, goodBuyCount = 0, goodMaxBuyCount = 1 });
        }
        return list;
    }

    //public List<CBagGoodsData> RandomBagDatas()
    //{
    //    List<CBagGoodsData> list= new List<CBagGoodsData>();
    //    for (int i = 0; i < shopJsonDatas.Count; i++)
    //    {
    //        list.Add(new CBagGoodsData() { itemID = i, itemName = shopJsonDatas[i].name, });
    //    }
    //}


public class CShopGoodsData
{
    public int gridID;
    public int goodID;
    public int goodLevel;
    public int goodBuyCount;
    public int goodMaxBuyCount;
}

public class CBagGoodsData
{
    public int itemID;
    public string itemName;
    public BagType bagType;
    public string icon;
    public string des;
    public int maxStack;
    public int rarity;
    public EquipType equipType;
}

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