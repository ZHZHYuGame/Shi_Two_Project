using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetID
{
    /// <summary>
    /// 客户端向服务器请求物品类型数据
    /// </summary>
    public static int C_T_S_Bag_Type_Message = 1001;
    /// <summary>
    /// 服务器向客户端返回物品类型数据
    /// </summary>
    public static int S_T_C_Bag_Type_Message = 1002;
    /// <summary>
    /// 服务器向客户端发送用户数据
    /// </summary>
    public static int S_T_C_User_Message = 1004;
    /// <summary>
    /// 客户端向服务器发送购买扩充卷请求
    /// </summary>
    public static int C_T_S_BuyAddCellJuan_Message=1005;
    /// <summary>
    /// 服务器向客户端回馈购买扩充卷请求
    /// </summary>
    public static int S_T_C_BuyAddCellJuan_Message=1006;
    /// <summary>
    /// 客户端向服务器发送扩充背包请求
    /// </summary>
    public static int C_T_S_AddBagCell_Message = 1007;
    /// <summary>
    /// 服务器向客户端回馈背包扩充请求
    /// </summary>
    public static int S_T_C_AddBagCell_Message = 1008;
}
