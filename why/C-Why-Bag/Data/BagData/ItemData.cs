using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public int ID;                  // 道具唯一ID
    public string Name;             // 道具名称
    public int Type;                // 道具类型 (关联ItemType表)
    public int PackTab;             // 背包页签掩码: 1武器 2防具 4药水 8材料
    public int Mass;                // 品质: 1灰 2绿 3蓝 4紫 5橙
    public int Priority;            // 排序优先级，越大越靠前
    public int MaxPile;             // 堆叠上限，0表示不可堆叠
    public string Msg;              // 道具描述
    public int Sell;                // 出售价格，0不可出售
    public string Path;             // 图标资源路径
    public int MinLevel;            // 使用/装备所需最低等级
    public int ObtainID;            // 获取途径ID，关联ItemObtain表
    public int TakeOut;             // 副本中是否可带出: 0不可 1可
}
public class ItemObtainData
{
    public int ID;                  // 途径ID
    public string Name;             // 途径名称（如“商店购买”）
    public int ObtainType;          // 类型: 1商店 2玩法 3副本掉落
    public int FuncOpenID;          // 关联的功能开放ID
    public int LevelLimit;          // 等级限制
    public string JumpUI;           // 跳转的界面名称
}
