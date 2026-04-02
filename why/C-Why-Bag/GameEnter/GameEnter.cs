using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;

public class GameEnter : MonoBehaviour
{
    
    private void Awake()
    {
        ConfigManager.GetInstance().Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        NetManager.GetInstance().Start();   
        Invoke("StartData",1);
    }

    void StartData()
    {
        C_2_S_InitialiazeBag msg=new C_2_S_InitialiazeBag();
        msg.Type = 0;
        NetManager.GetInstance().SendNetMessage(NetID.C_To_S_InitizlizeMessage,msg.ToByteArray());
    }
    // Update is called once per frame
    void Update()
    {
        NetManager.GetInstance().Update();
    }
}
