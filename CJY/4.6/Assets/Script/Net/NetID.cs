using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetID
{
    /// <summary>
    /// 网络测试消息--客户端请求
    /// </summary>
    public static int C_To_S_Game_Net_Test_Message = 1001;
    /// <summary>
    /// 网络测试消息--服务器反馈
    /// </summary>
    public static int S_To_C_Game_Net_Test_Message = 1002;
    /// <summary>
    /// 客户端向服务器请求对应商城商品的数据网络消息
    /// </summary>
    public static int C_To_S_Shop_Type_Datas_Message = 1003;
    /// <summary>
    /// 服务器向客户端回馈对应商城商品的数据网络消息
    /// </summary>
    public static int S_To_C_Shop_Type_Datas_Message = 1004;
    /// <summary>
    /// 客户端向服务器请求世界聊天网络消息
    /// </summary>
    public static int C_To_S_Chat_World_Message = 1007;
    /// <summary>
    /// 服务器向客户端回馈世界聊天网络消息
    /// </summary>
    public static int S_To_C_Chat_World_Message = 1008;
    /// <summary>
    /// 客户端-私聊
    /// </summary>
    public static int C_To_S_Chat_Private_Message = 1009;
    /// <summary>
    /// 服务器-私聊
    /// </summary>
    public static int S_To_C_Chat_Private_Message = 1010;
    /// <summary>
    /// 角色数据获取-请求
    /// </summary>
    public static int C_To_S_GetUserData_Message = 1011;
    /// <summary>
    /// 角色数据获取-回馈
    /// </summary>
    public static int S_To_C_GetUserData_Message = 1012;
    /// <summary>
    /// 背包数据获取-请求
    /// </summary>
    public static int C_To_S_GetBagData_Message = 1013;
    /// <summary>
    /// 背包数据获取-回馈
    /// </summary>
    public static int S_To_C_GetBagData_Message = 1014;
    // 在 NetID.cs 中添加以下消息ID
    /// <summary>
    /// 整理背包-请求
    /// </summary>
    public static int C_To_S_SortBagData_Message = 1015;
    /// <summary>
    /// 整理背包-回馈
    /// </summary>
    public static int S_To_C_SortBagData_Message = 1016;
    /// <summary>
    /// 出售物品-请求
    /// </summary>
    public static int C_To_S_SellItem_Message = 1017;
    /// <summary>
    /// 出售物品-回馈
    /// </summary>
    public static int S_To_C_SellItem_Message = 1018;
}
