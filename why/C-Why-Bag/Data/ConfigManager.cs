using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ConfigManager : Singleton<ConfigManager>
{
    public List<ItemData> itemList = new List<ItemData>();
    public void Init()
    {
        itemList = JsonConvert.DeserializeObject<List<ItemData>>(Resources.Load<TextAsset>("Item").text);
        Debug.Log(itemList.Count);
    }
}
