using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf;
using Message;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPanel : MonoBehaviour
{
    public Button yes, no;
    public UnityEngine.UI.Text show;
    public int nowNum;
    private void OnEnable()
    {
        
    }

    public void Init(int num)
    {
        show.text =1+"/"+num;
        nowNum = num;
    }
    // Start is called before the first frame update
    void Start()
    {
        yes.onClick.AddListener(() =>
        {
            SendMessage();
        });
        no.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void SendMessage()
    {
        if (nowNum < 1)
        {
            Debug.Log("库存不足");
        }
        else
        {
            C2S_Dilatationl c_data=new C2S_Dilatationl();
            c_data.Num = 5;
            NetManger.GetInstance.SendMessage(NetID.C_2_S_Dilatation,c_data.ToByteArray());
            
        }
        gameObject.SetActive(false);
    }
}
