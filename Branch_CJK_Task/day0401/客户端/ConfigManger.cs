using System.Collections.Generic;
using Unity.VisualScripting;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace
{
    public class ConfigManger:SingLeton<ConfigManger>
    {
        public List<ItemData> allList=new List<ItemData>();

        public void Start()
        {
            allList = JsonConvert.DeserializeObject<List<ItemData>>(Resources.Load<TextAsset>("good").text);
            Debug.Log(allList.Count);
        }

        public ItemData GetItemData(int id)
        {
            return allList.Find(x => x.Id == id);
        }
    }
}