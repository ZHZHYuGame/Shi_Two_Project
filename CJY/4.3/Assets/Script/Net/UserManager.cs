using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Google.Protobuf;
/// <summary>
/// 角色信息管理
/// </summary>
public class UserManager : Singleton<UserManager>
{

    public int userID;

    public void Start()
    {
        MessageControll.Instance.AddListener(NetID.S_To_C_GetUserData_Message, S_To_C_GetUserData_Message_Handle);
    }

    private void S_To_C_GetUserData_Message_Handle(object obj)
    {
        object[] objList = obj as object[];
        byte[] data = objList[0] as byte[];

        S_To_C_Login_Message s_Data = S_To_C_Login_Message.Parser.ParseFrom(data);

        userID = s_Data.UserID;

        Debug.Log($"客户端ID = {s_Data.UserID}");
    }
}