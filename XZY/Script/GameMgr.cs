using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.Instance().Start();
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Instance().Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NetManager.Instance().SendNetMessage(NetID.S_2_CAddMail,BitConverter.GetBytes(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NetManager.Instance().SendNetMessage(NetID.S_2_CAddMail,BitConverter.GetBytes(2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            NetManager.Instance().SendNetMessage(NetID.S_2_CAddMail, BitConverter.GetBytes(3));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            NetManager.Instance().SendNetMessage(NetID.S_2_CAddMail,BitConverter.GetBytes(4));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            NetManager.Instance().SendNetMessage(NetID.S_2_CAddMail,BitConverter.GetBytes(5));
        }
       
    }
}
