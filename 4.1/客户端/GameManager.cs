using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Google.Protobuf;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BagManager bagManager;

    public static GameManager Ins;

    public GameObject player;

    private void Awake()
    {
        Ins = this;
        ConfigManager.Ins.Init();
    }

    public void OnOpenBagPanel()
    {
        bagManager.OnOpenBagPanel();
    }

    // Start is called before the first frame update
    void Start()
    {
        NetManager.Ins.Start();
        Invoke("SetUserPort",0.5f);
    }

    void SetUserPort()
    {
        UserManager.Ins.Start();
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Ins.Update();
    }
}
