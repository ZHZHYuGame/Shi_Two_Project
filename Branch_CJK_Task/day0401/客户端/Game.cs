using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Google.Protobuf;
using Message;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ConfigManger.GetInstance.Start();
        NetManger.GetInstance.Start();
        Invoke("BagInit",0.2f);
    }
    void BagInit()
    {
        C2S_GetBackpackData c_data = new C2S_GetBackpackData();
        NetManger.GetInstance.SendMessage(NetID.C_2_S_BagInit, c_data.ToByteArray());
    }
    // Update is called once per frame
    void Update()
    {
        NetManger.GetInstance.Update();
    }
}
