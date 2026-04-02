using System.Net.Sockets;
using Game;
using Google.Protobuf;

namespace S;

public class UserManager:Singleton<UserManager>
{
    private Dictionary<int, Client> dic = new();

    public void Start()
    {
        MessageCenter.Ins.AddListener(NetID.C_To_S_Get_User_Port_Message,GetPort);
    }

    private void GetPort(object obj)
    {
        object[] objs = obj as object[];
        Client client = objs[0] as Client;
        S_To_C_Get_User_Port_Message s_data = new();
        s_data.Port = client.port;
        NetManager.Ins.SendMessage(client.st,NetID.S_To_C_Get_User_Port_Message, s_data.ToByteArray());
    }

    public void Add(Client client)
    {
        if (!dic.ContainsKey(client.port))
        {
            dic.Add(client.port, client);
            BagManager.Ins.InitUserData(client);
        }
    }
}


public class Client
{
    public string ip;
    public int port;
    public Socket st;

    public string bagType;
    public List<BagData> allList = new();
    public Dictionary<int, BagData> bagList = new();

    public Dictionary<EqPositionType, Item> eqDic = new();

}