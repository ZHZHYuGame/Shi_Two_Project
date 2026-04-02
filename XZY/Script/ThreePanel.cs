using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
using UnityEngine.UI;

public class ThreePanel : MonoBehaviour
{
    public Text ZT, Des,GetBenText;
    public Button CloseBtn,GetBtn;
    public Transform CellBase,FJ;
    public static ThreePanel Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CloseBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            NetManager.Instance().SendNetMessage(NetID.S_2_CSXMail);
        });
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData(MailData item)
    {
        gameObject.SetActive(true);
        ZT.text = item.Data.Title;
        Des.text = item.Data.Content;
        if (item.Data.IsAward)
        {
            GetBenText.text = "提取";
            FJ.gameObject.SetActive(true);
            GetUIFJ(item, true);
        }
        else if(item.Data.AwardList.Count==0)
        {
            FJ.gameObject.SetActive(false);
            GetBenText.text = "删除";
        }
        else
        {
            GetBenText.text = "删除";
            FJ.gameObject.SetActive(true);
            GetUIFJ(item, false);
        }
        GetBtn.onClick.AddListener(() =>
        {
            if (GetBenText.text == "删除")
            {
                NetManager.Instance().SendNetMessage(NetID.S_2_CDelMail,BitConverter.GetBytes(item.MailID));
                gameObject.SetActive(false);
                
            }
            else
            {
                NetManager.Instance().SendNetMessage(NetID.S_2_CGetMail,BitConverter.GetBytes(item.MailID));
            }
        });
    }

    public void GetUIFJ(MailData item,bool Is)
    {
        for (int i = 0; i < CellBase.childCount; i++)
        {
            Destroy(CellBase.GetChild(i).gameObject);
        }
        foreach (var items in item.Data.AwardList)
        {
            ItemCell Cell=Instantiate(Resources.Load<ItemCell>("ItemCell"),CellBase);
            Cell.GetData(items,Is);
        }
    }
}
