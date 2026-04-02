using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfigManager:SingLeton<ConfigManager>
{
    public List<ItemData> sUserDatas = new List<ItemData>();
    public void Start()
    {
        var str = File.ReadAllText("Item.json");
        sUserDatas = JsonConvert.DeserializeObject<List<ItemData>>(str);
        Console.WriteLine("长度为:" + sUserDatas.Count);
    }
    public ItemData GetItemData(int id)
    {
        for(int i = 0; i < sUserDatas.Count; i++)
        {
            if (sUserDatas[i].ID == id)
            {
                return sUserDatas[i];
            }
        }
        return null;
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
    武器 = 1, 防具 = 2, 药水 = 3, 材料 = 4
}
