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

public class ConfigManager : Singleton<ConfigManager>
{
    public List<ShopData> shops=new List<ShopData>();

    public void Init()
    {
        shops = JsonConvert.DeserializeObject<List<ShopData>>(Resources.Load<TextAsset>("good").text);
    }

    public ShopData FindData(int id)
    {
        ShopData data = null;
        for (int i = 0; i < shops.Count; i++)
        {
            if (int.Parse(shops[i].Id)==id)
            {
                data = shops[i];
            }
        }

        return data;
    }
}
