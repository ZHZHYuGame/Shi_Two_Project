using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Games;
using Google.Protobuf;

public class Game : MonoBehaviour
{
    public Button shopBtn,bagBtn;

    // Start is called before the first frame update
    void Start()
    {
        ConfigManager.Instance.Init();
        
        NetManager.Instance.Start();
        
        UserManager.Instance.Start();
        
        Invoke("Request_UserData", 1);
        
        shopBtn.onClick.AddListener(() =>
        {
            ShopUI.ins.gameObject.SetActive(true);
        });
    }

    void Request_UserData()
    {
        C_To_S_Login_Message c_Data = new C_To_S_Login_Message();

        NetManager.Instance.SendNetMessage(NetID.C_To_S_GetUserData_Message, c_Data.ToByteArray());
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Instance.Update();
    }
}
