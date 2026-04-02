using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isOne;
    BagPlane bag=null;
    // Start is called before the first frame update
    void Start()
    {
        NetManager.Instance.Start();
        ConfigManager.Instance.Start();
        MessageControll.Instance.AddListener(NetID.S_To_C_BagData_Message,BagPlane);
    }

    public void GuanBi()
    {
        gameObject.SetActive(false);
    }

    private void BagPlane(object obj)
    {
        var parent= obj as object[];
        var data= parent[0] as byte[];
        S_To_C_BagData_Message s_data=S_To_C_BagData_Message.Parser.ParseFrom(data);
        if (bag==null)
        {
            Instantiate(Resources.Load<GameObject>($"{s_data.Game}"));
            bag= Instantiate(Resources.Load<BagPlane>("UIS/BagPlane"),GameObject.Find("Canvas").transform);
        }
        bag.Init(s_data);
    }

    public void BagPlaneTrue()
    {
        C_To_S_BagData_Message pos=new C_To_S_BagData_Message();
        pos.Countype = CountType.One;
        NetManager.Instance.SendNetMessage(NetID.C_To_S_BagData_Message,pos.ToByteArray());
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Instance.Update();
    }
}
