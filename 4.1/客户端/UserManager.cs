using System.Collections;
using System.Collections.Generic;
using Game;
using Google.Protobuf;
using UnityEngine;

public class UserManager : Singleton<UserManager>
{
    public int port;
    
    
    // Start is called before the first frame update
    public void Start()
    {
        MessageCenter.Ins.AddListener(NetID.S_To_C_Get_User_Port_Message,GetPortHandle);
        C_To_S_Get_User_Port_Message c_Data = new();
        NetManager.Ins.SendMessage(NetID.C_To_S_Get_User_Port_Message,c_Data.ToByteArray());
        Debug.Log("发送获得id的请求");
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
