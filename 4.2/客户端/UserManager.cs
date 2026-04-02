using System.Collections;
using System.Collections.Generic;
using Game;
using Google.Protobuf;
using UnityEngine;

public class UserManager : Singleton<UserManager>
{
    public int port;
    public float money;
    public int lightning;
    
    // Start is called before the first frame update
    public void Start()
    {
        MessageCenter.Ins.AddListener(NetID.S_To_C_Get_User_Port_Message,GetPortHandle);
        MessageCenter.Ins.AddListener(NetID.S_To_C_Get_User_Money_Message,GetMoneyHandle);
        C_To_S_Get_User_Port_Message c_Data = new();
        NetManager.Ins.SendMessage(NetID.C_To_S_Get_User_Port_Message,c_Data.ToByteArray());
        Debug.Log("发送获得id的请求");
    }
    
      
    private void GetMoneyHandle(object obj)
    {
        object[] objs = obj as object[];
        var data = objs[0] as byte[];

        S_To_C_Set_Money_Message s_data = S_To_C_Set_Money_Message.Parser.ParseFrom(data);
        money = s_data.Money;
        Debug.Log("我的钱++++++++" + money);
        MessageCenter.Ins.BroadCast(NetID.User_To_Bag_Set_Money_Message);
    }

    private void GetPortHandle(object obj)
    {
        object[] objs = obj as object[];
        byte[] bytes = objs[0] as byte[];
        
        S_To_C_Get_User_Port_Message s_Data = S_To_C_Get_User_Port_Message.Parser.ParseFrom(bytes);
        this.port = s_Data.Port;
        Debug.Log("获得自身的客户端端口:::"+port);
    }


}
