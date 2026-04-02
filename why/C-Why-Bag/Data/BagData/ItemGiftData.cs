using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiftData
{
    public int ID;                  // 礼包ID
    public int ItemID;              // 对应的道具ID（礼包道具本身）
    public int GiftType;            // 礼包类型: 1固定 2随机 3自选 4限时 5实物
    public string StartTime;        // 开始时间 (格式: yyyy-MM-dd HH:mm:ss)
    public string EndTime;          // 结束时间
    public List<GiftContent> Contents; // 礼包内容列表
}
public class GiftContent
{
    public int ItemID;              // 产出道具ID
    public int Count;               // 数量
    public int Weight;              // 权重（随机礼包使用）
}