using System.Collections;
using System.Collections.Generic;
using Games;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class OnePanel : MonoBehaviour
{
    public Text AllNum, IsGetNum;
    public Transform CellBase,TwoPanel;
    public Button AllGetBtn, AllDelBtn;
    public List<MailData> list=new List<MailData>();
    void Start()
    {
        MessageControll.Instance().AddListener(NetID.S_2_CGame,GetData);
        AllGetBtn.onClick.AddListener(() =>
        {
            NetManager.Instance().SendNetMessage(NetID.S_2_CGetAllMail);
        });
        AllDelBtn.onClick.AddListener(() =>
        {
            NetManager.Instance().SendNetMessage(NetID.S_2_CDelAllMail);
        });
    }

    private void GetData(object obj)
    {
        S_To_C_Mail Data = S_To_C_Mail.Parser.ParseFrom((obj as object[])[0] as byte[]);
        list.Clear();
        foreach (var item in Data.MailData)
        {
            list.Add(item);
        }

        UISX();
        if (ThreePanel.Instance.Mail != null)
        {
            foreach (var item in list)
            {
                if (item.MailID == ThreePanel.Instance.Mail.MailID)
                {
                    ThreePanel.Instance.GetData(item);
                    break;
                }
            }
        }
    }

    private void UISX()
    {
        for (int i = 0; i < CellBase.childCount; i++)
        {
            Destroy(CellBase.GetChild(i).gameObject);
        }

        int count = 0;
        foreach (var item in list)
        {
            if (item.Data.IsAward)
            {
                count++;
            }
            var temp= Instantiate(Resources.Load<EmilCell>("EmilCell"),CellBase);
            temp.GetData(item);
        }
        if (list.Count==0)
        {
            TwoPanel.gameObject.SetActive(true);
        }
        else
        {
            TwoPanel.gameObject.SetActive(false);
        }
        
        AllNum.text = list.Count + "/50";
        IsGetNum.text = count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
}
