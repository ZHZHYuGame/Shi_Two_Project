using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Newtonsoft.Json;

public class ConfigManager : Singleton<ConfigManager>
{
    public Dictionary<int,Item> ItemData = new();
    
    public void Init()
    {
        var itemList = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("Item").text);
        foreach (var item in itemList)
        {
            ItemData.Add(item.Id,item);
        }
        Debug.Log("ItemData的数量"+ItemData.Count);
    }
}
