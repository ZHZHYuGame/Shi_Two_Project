using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetID 
{
    // 获取端口号
    public const int C_To_S_Get_User_Port_Message = 1001;
    public const int S_To_C_Get_User_Port_Message = 1002;
    // 获取背包数据
    public const int C_To_S_Get_Bag_Data_Message = 1003;
    public const int S_To_C_Get_Bag_Data_Message = 1004;
    // 穿戴背包数据
    public const int C_To_S_Wear_Eq_Message = 1005;
    public const int S_To_C_Wear_Eq_Message = 1006;
    // 添加背包
    public const int C_To_S_Add_Bag_Data_Message = 1007;
    public const int S_To_C_Add_Bag_Data_Message = 1008;
    // 脱下装备  服务器的和穿戴用一个信息号了
    public const int C_To_S_Push_Eq_Message = 1009;
    // 卖装备  因为直接重新渲染格子就行 所以和获取背包数据用一个信息号
    public const int C_To_S_Sell_Item_Message = 1010;
    // 获取账户金币的方法  因为我们这个是发给userManager的所以需要一个新的消息号用来通知背包接取money
    public const int S_To_C_Get_User_Money_Message = 1011;
    public const int User_To_Bag_Set_Money_Message = 1012;
}
