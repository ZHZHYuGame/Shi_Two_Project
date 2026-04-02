using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class BagPlane : MonoBehaviour
{
    public Transform huobiContent, typeContent, bagdataContent,equipContent;
    public Text numtext,fightingtext;
    private int istrue,isfalse;
    public List<Equipitem> Equipitems=new List<Equipitem>();
    public List<BagItem> BagItems=new List<BagItem>();
    public static BagPlane instance;

    private void Awake()
    {
        instance = this;
        MessageControll.Instance.AddListener(NetID.S_To_C_BagApp_Message,TianJia);
    }

    private void TianJia(object obj)
    {
        var parent= obj as object[];
        var data= parent[0] as byte[];
        S_To_C_BagApp_Message s_data=S_To_C_BagApp_Message.Parser.ParseFrom(data);
        for (int i = 0; i < Equipitems.Count; i++)
        {
            if (s_data.EquipGrigID==Equipitems[i].dataPB.GridID)
            {
                Equipitems[i].Init(s_data.Equipdata);
            }
        }

        for (int i = 0; i<BagItems.Count; i++)
        {
            if (s_data.DataGridID==BagItems[i].dataPB.GridID)
            {
                BagItems[i].Init(s_data.Bagdata);
            }
        }
    }//替换 装备 数据更新

    public int KongData()
    {
        for (int i = 0; i<BagItems.Count; i++)
        {
            if (BagItems[i].dataPB.Id == 0)
            {
                return BagItems[i].dataPB.GridID;
            }
        }
        return 0; 
    }//查找空格子
    
    public void Init(S_To_C_BagData_Message sData)
    {
        if (sData.BagTypeList.Count>0)
        {
            fightingtext.text=sData.Fighting.ToString();
            for (int i = 0; i < sData.BagHuoBiType.Count; i++)
            {
                var t=Instantiate(Resources.Load<GameObject>("UIS/Currency"), huobiContent);
                t.transform.Find("icon").GetComponent<Image>().sprite=Resources.Load<Sprite>(sData.BagHuoBiType[i].Path);
                t.transform.Find("num").GetComponent<Text>().text = sData.BagHuoBiType[i].Money.ToString();
            }
            for (int i = 0; i < sData.BagTypeList.Count; i++)
            {
                var t=Instantiate(Resources.Load<Button>("UIS/FenLeiBut"), typeContent);
                t.transform.Find("icon").GetComponent<Image>().sprite=Resources.Load<Sprite>(sData.BagTypeList[i].Icon);
                t.transform.Find("name").GetComponent<Text>().text = sData.BagTypeList[i].Name;
            }
            for (int i = 0; i < sData.EquipType.Count; i++)
            {
                var t=Instantiate(Resources.Load<Equipitem>("UIS/Equipitem"), equipContent);
                t.Init(sData.EquipType[i]);
                Equipitems.Add(t);
            }
        }
        istrue = 0;
        isfalse = 0;
        NewBagitem(sData);
        numtext.text = "物品数量" + istrue + "/" + isfalse;
    }//初始化数据

    private void NewBagitem(S_To_C_BagData_Message sData)
    {
        for (int i = 0; i < bagdataContent.childCount; i++)
        {
            Destroy(bagdataContent.GetChild(i).gameObject);
        }
        BagItems.Clear();
        for (int i = 0; i < sData.BagList.Count; i++)
        {
            if (sData.BagList[i].Id!=0)
            {
                istrue += 1;
            }
            if (sData.BagList[i].IsSuo == 2)
            {
                isfalse += 1;
            }
            var t=Instantiate(Resources.Load<BagItem>("UIS/Bagitem"), bagdataContent);
            t.Init(sData.BagList[i]);
            BagItems.Add(t);
        }
    }//new背包格子

    public int ZhangBei(string itemdateTypeName)
    {
        for (int i = 0; i < Equipitems.Count; i++)
        {
            if (Equipitems[i].names.text==itemdateTypeName)
            {
                return Equipitems[i].dataPB.GridID;
            }
        }
        return 0;
    }//查找装备对应的人物装备格子id

    public void YiJian()
    {
        C_To_S_BagData_Message pos=new C_To_S_BagData_Message();
        pos.Countype = CountType.Two;
        NetManager.Instance.SendNetMessage(NetID.C_To_S_BagData_Message,pos.ToByteArray());
    }//一键出售

    public void FenLeiSend()
    {
        
    }
}
