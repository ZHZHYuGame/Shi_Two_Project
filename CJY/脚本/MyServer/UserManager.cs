using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Games;
using Google.Protobuf;
using static ConfigManager;
/// <summary>
/// 角色管理器
/// </summary>
public class UserManager:Singleton<UserManager>
{
    /// <summary>
    /// 管理当前服务器的所有在线玩家
    /// </summary>
    public Dictionary<int, Client> userDict = new Dictionary<int, Client>();

    public object S_To_C_GetUserData_Message_Handle { get; private set; }

    public void Start()
    {
        MessageControll.GetInstance().AddListener(NetID.C_To_S_GetUserData_Message, C_To_S_GetUserData_Message_Handle);
    }
    /// <summary>
    /// 服务器向客户端发送进入游戏后的角色数据信息
    /// </summary>
    /// <param name="obj"></param>
    private void C_To_S_GetUserData_Message_Handle(object obj)
    {
        object[] objList = obj as object[];
        byte[] data = objList[0] as byte[];
        Client c = objList[1] as Client;

        C_To_S_Login_Message c_Data = C_To_S_Login_Message.Parser.ParseFrom(data);

        S_To_C_Login_Message s_Data = new S_To_C_Login_Message();
        s_Data.UserID = c.port;
        NetManager.GetInstance().SendNetMessage(c.st, NetID.S_To_C_GetUserData_Message, s_Data.ToByteArray());
    }

    /// <summary>
    /// 添加客户端玩家
    /// </summary>
    /// <param name="c"></param>
    public void AddClientUser(Client c)
    {
        if (!userDict.ContainsKey(c.port))
        {
            userDict.Add(c.port, c);
        }
    }


}

public class UserData
{
    /// <summary>
    /// 记录角色的基础属性
    /// </summary>
    public UserBaseData userBaseData;
    /// <summary>
    /// 当前任务ID
    /// </summary>
    public int userCurrTaskID;
    /// <summary>
    /// 当前副本进度
    /// </summary>
    public int userFubenID;
    /// <summary>
    /// 普通商城数据（记录购买次数）
    /// </summary>
    public List<CShopGoodsData> everyday_ShopDataList;
    /// <summary>
    /// 特殊商城数据（全部数据记录）
    /// </summary>
    public List<CShopGoodsData> special_shopDataList;

}

public class UserBaseData
{
    /// <summary>
    /// 全局唯一ID
    /// </summary>
    public int GUID;
    /// <summary>
    /// 姓名
    /// </summary>
    public string name;
    /// <summary>
    /// 攻击力
    /// </summary>
    public int attack;
}