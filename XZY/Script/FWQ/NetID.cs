using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 网络消息ID
/// </summary>
public class NetID
{
    /// <summary>
    /// 玩家登录时服务器直接发的消息号
    /// </summary>
    public static int S_2_CGame=1001; 
    /// <summary>
    /// 打开邮件
    /// </summary>
    public static int S_2_COpenMail=1002; 
    /// <summary>
    /// 刷新邮件
    /// </summary>
    public static int S_2_CSXMail=1003; 
    /// <summary>
    /// 获取背包数据
    /// </summary>
    public static int S_2_CSGetBag=1004;
    /// <summary>
    /// 删除单个邮件
    /// </summary>
    public static int S_2_CDelMail = 1005;
    /// <summary>
    /// 删除全部邮件
    /// </summary>
    public static int S_2_CDelAllMail = 1006;
    /// <summary>
    /// 领取单个邮件
    /// </summary>
    public static int S_2_CGetMail = 1007;
    /// <summary>
    /// 领取全部邮件
    /// </summary>
    public static int S_2_CGetAllMail = 1008;
    /// <summary>
    /// 添加邮件
    /// </summary>
    public static int S_2_CAddMail = 1009;
    /// <summary>
    /// 添加失败服务器返回
    /// </summary>
    public static int S_2_CAddlose = 1010;
}
