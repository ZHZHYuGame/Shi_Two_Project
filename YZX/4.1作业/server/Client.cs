using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 客户端信息
/// </summary>
public class Client
{
    public Socket st;
    public string ip;
    public int port;//端口号
    /// <summary>
    /// 玩家角色信息
    /// </summary>
    public UserData userData;

}
