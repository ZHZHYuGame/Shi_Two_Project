using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Google.Protobuf;
using Message;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    public static BagUI instance;
    //初始化时的文件
    public List<BagItem> nowItems = new List<BagItem>();
    public List<BagItemUI>  bagItems = new List<BagItemUI>();
    public Transform bagParent;
    public Button b1, b2, b3, b4,b5;
    public int maxNum,nowNum;
    public Text cellNum;
    //数据
    public List<EquipItem> equipItems = new List<EquipItem>();
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public bool JudgeItem(int index)
    {
        for (int i = 0; i < equipItems.Count; i++)
        {
            if (equipItems[i].myBagItem != null)
            {
                if (equipItems[i].myData.Type == index)
                {
                    return true;     
                }
            }
        }
        return false;
    }
    void Start()
    {
        ItemData item = new ItemData();
        MessageControll.GetInstance.AddListener(NetID.S_2_C_BagInit,Init);
        MessageControll.GetInstance.AddListener(NetID.S_2_C_RefreshUI,RefreshUI);
        b1.onClick.AddListener(() =>
        {
            TypeRefresh("全部");
        });
        b2.onClick.AddListener(() =>
        {
            TypeRefresh("武器");
        });
        b3.onClick.AddListener(() =>
        {
            TypeRefresh("防具");
        });
        b4.onClick.AddListener(() =>
        {
            TypeRefresh("药水");
        });
        b5.onClick.AddListener(() =>
        {
            TypeRefresh("材料");
        });
        
    }

    private void RefreshUI(object obj)
    {
        object[] arr = (object[])obj;
        byte[] data = (byte[])arr[0];
        S2C_RefreshUI s_data=S2C_RefreshUI.Parser.ParseFrom(data);
        int count = Math.Min(nowNum, s_data.NowItems.Count);
        for (int i = 0; i < count; i++)
        {
            bagItems[i].RefreshItem(s_data.NowItems[i]);
        }
        // 剩余格子（如果有）应刷新为 null，表示空槽
        for (int i = count; i < nowNum; i++)
        {
            bagItems[i].RefreshItem(null);
        }
    }

    public void TypeRefresh(string str)
    {
        C2S_RefreshUI c_data=new C2S_RefreshUI();
        c_data.Type = str;
        NetManger.GetInstance.SendMessage(NetID.C_2_S_RefreshUI,c_data.ToByteArray());
    }
    private void Init(object obj)
    {
        object[] args = (object[])obj;
        byte[] data = (byte[])args[0];
        S2C_GetBackpackData s_data=S2C_GetBackpackData.Parser.ParseFrom(data);
        maxNum = s_data.MainBag.TotalSlots;
        nowNum = s_data.MainBag.UnlockedSlots;
        for(int i=0;i<s_data.MainBag.TotalSlots;i++)
        {
            GameObject item=Instantiate(Resources.Load<GameObject>("BagItem"),bagParent);
            bagItems.Add(item.GetComponent<BagItemUI>());
        }
        for (int i = 0; i < s_data.MainBag.UnlockedSlots; i++)
        {
            bagItems[i].Open();
        }
        for (int i = 0; i < s_data.MainBag.Items.Count; i++)
        {
            bagItems[i].Init(s_data.MainBag.Items[i]);
            nowItems.Add(s_data.MainBag.Items[i]);
        }
        cellNum.text=nowItems.Count + "/" +nowNum;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
