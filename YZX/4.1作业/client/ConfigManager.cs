using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ConfigManager : Singlton<ConfigManager>
{
    public List<Good> goods = new List<Good>();
    public List<FuctionBtn> fuctionBtns = new List<FuctionBtn>();
    public void Start()
    {
        goods = JsonConvert.DeserializeObject<List<Good>>(Resources.Load<TextAsset>("goods").text);
        fuctionBtns = JsonConvert.DeserializeObject<List<FuctionBtn>>(Resources.Load<TextAsset>("fuctionBtn").text);
    }
    public Good ByIdGetGood(int id)
    {
        for (int i = 0; i < goods.Count; i++)
        {
            if (goods[i].id == id)
            {
                return goods[i];
            }
        }
        return null;
    }

}

public class Good
{
    public int id;                      // 道具唯一ID
    public string name;                 // 道具名称
    public string iconPath;             //图片资源
    public int goodType;// 道具类型: 1武器 2防具 3药水 4材料
    public int btnType;//物品类型：1装备，2道具
    public int sell;   // 出售价格，0不可出售
    public int wearType;//装备穿戴部位:0其他，1头,2衣服,3项链,4戒指,5护腿,6鞋子,7武器
    public int quality; //物品品质:0无，1灰，2绿 3蓝 4紫 5橙
    public int maxPile;                 // 堆叠上限，0表示不可堆叠
    public string msg;                  //道具描述
}

public class FuctionBtn
{
    public int id;
    public string btnName;
    public int type;
}




