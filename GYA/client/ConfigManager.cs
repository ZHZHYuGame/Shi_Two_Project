// 道具基础表 Item

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class ConfigManager:SingLeton<ConfigManager>
{
    public List<ItemData> items = new List<ItemData>();
    public List<BagItemBtnTypeData> butTypeData = new List<BagItemBtnTypeData>();
    public void Start()
    {
        items=JsonConvert.DeserializeObject<List<ItemData>>(Resources.Load<TextAsset>("Biao/Item").text);
        butTypeData=JsonConvert.DeserializeObject<List<BagItemBtnTypeData>>(Resources.Load<TextAsset>("Biao/BagItemBtnType").text);
    }

    public ItemData GetData(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID==id)
            {
                return items[i];
            }
        }
        return null; 
    }

    public List<BagItemBtnTypeData> GetButData(ItemData data,int en,int type)
    {
        List<BagItemBtnTypeData> linshi=new List<BagItemBtnTypeData>();
        for (int i = 0; i < butTypeData.Count; i++)
        {
            //判断符合规则的按钮
            if ((type==1 && butTypeData[i].id<=10)||(type==2 && butTypeData[i].id>10))//判断在那个地方进行Tips
            {
                if (butTypeData[i].reuipType==data.Type)
                {
                    linshi.Add(butTypeData[i]);
                }
                if (butTypeData[i].reuipType==ItemType.通用&& data.Sell!=0)
                {
                    linshi.Add(butTypeData[i]);
                }
            }
        }
        return linshi;
    }
}



public class ItemData
{
    public int ID;                  // 道具唯一ID
    public string Name;             // 道具名称
    public ItemType Type;           // 道具类型 (关联ItemType表)
    public string typeName;         //如果是装备的话加入装备类型名称 武器 盔甲 鞋子等等
    public int PackTab;             // 背包页签掩码: 1武器 2防具 4药水 8材料
    public int Mass;                // 品质: 1灰 2绿 3蓝 4紫 5橙
    public int Priority;            // 排序优先级，越大越靠前
    public int MaxPile;             // 堆叠上限，0表示不可堆叠
    public string Msg;              // 道具描述
    public int Sell;                // 出售价格，0不可出售
    public string Path;             // 图标资源路径
    public int MinLevel;            // 使用/装备所需最低等级
    public int TakeOut;             // 副本中是否可带出: 0不可 1可
    public string bk;
}

public enum ItemType
{
    weapon=1,armor=2,liquid=3,materials=4,通用=5
}

[Serializable]
public class BagItemBtnTypeData
{
    public int id;                  // 按钮类型ID
    public string name;          // 按钮显示文字
    public ItemType reuipType;       // 显示条件（如装备类型等）
}