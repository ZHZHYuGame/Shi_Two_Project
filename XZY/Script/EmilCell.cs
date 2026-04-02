using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
using UnityEngine.UI;

public class EmilCell : MonoBehaviour
{
    public Image icon,Isicon;
    public Text nameText, time;
    public Button Btn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData(MailData item)
    {
        nameText.text = item.Data.Title;
        time.text = item.Data.StartTime;
        if (!item.IsRead)
        {
            icon.enabled = false;
        }
        if (!item.Data.IsAward)
        {
            Isicon.enabled = false;
        }
        Btn.onClick.AddListener(() =>
        {
            NetManager.Instance().SendNetMessage(NetID.S_2_COpenMail,BitConverter.GetBytes(item.MailID));
            ThreePanel.Instance.GetData(item);
        });
        
    }
}
