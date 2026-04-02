using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
public class Item
{
    public int id;
    public string name;
    public string des;
    public int type;
    public string path;
}
public class YJ
{
    public int emailID;
    public string title;
    public string content;
    public bool isAward;
    public List<Item> awardList;
    public DateTime StartTime;
    public float Time;
}
public class BagCellData
{
    public Item Item;
    public int Count;
}
public class ConfigMgr
{
    public List<YJ> YJList=new List<YJ>();
    public List<Item> ItemList=new List<Item>();
    static ConfigMgr instance;
    public static ConfigMgr Instance()
    {
        if(instance == null)
        {
            instance = new ConfigMgr();
        }
        return instance;
    }
    public void Start()
    {
        ItemList = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText("Item.json"));
        YJList = JsonConvert.DeserializeObject<List<YJ>>(File.ReadAllText("MailMsg.json"));
        
    }

}
