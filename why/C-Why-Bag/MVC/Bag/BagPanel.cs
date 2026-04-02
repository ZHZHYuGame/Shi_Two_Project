using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : MonoBehaviour
{
    // public List<BagGoodsDate>
    public Text goldText,masonryText,rollText,energyText,headText;
    public int nowNum, maxNum;
    public Transform Content;
    public Button oneBtn,twoBtn,threeBtn,fourBtn,fiveBtn;
    // Start is called before the first frame update
    void Start()
    {
        MessageControl.GetInstance().AddListener(NetID.S_To_C_InitizlizeMessage,StartHandler);
        MessageControl.GetInstance().AddListener(NetID.S_To_C_GetBagTypeMessage,GetBagTypeHandler);
        oneBtn.onClick.AddListener(() =>
        {
            C_2_S_GetBagType msg = new C_2_S_GetBagType();
            msg.Type = 0;
            NetManager.GetInstance().SendNetMessage(NetID.C_To_S_GetBagTypeMessage,msg.ToByteArray());
        });
        twoBtn.onClick.AddListener(() =>
        {
            C_2_S_GetBagType msg = new C_2_S_GetBagType();
            msg.Type = 1;
            NetManager.GetInstance().SendNetMessage(NetID.C_To_S_GetBagTypeMessage,msg.ToByteArray());
        });
        threeBtn.onClick.AddListener(() =>
        {
            C_2_S_GetBagType msg = new C_2_S_GetBagType();
            msg.Type = 2;
            NetManager.GetInstance().SendNetMessage(NetID.C_To_S_GetBagTypeMessage,msg.ToByteArray());
        });
        fourBtn.onClick.AddListener(() =>
        {
            C_2_S_GetBagType msg = new C_2_S_GetBagType();
            msg.Type = 4;
            NetManager.GetInstance().SendNetMessage(NetID.C_To_S_GetBagTypeMessage,msg.ToByteArray());
        });
        fiveBtn.onClick.AddListener(() =>
        {
            C_2_S_GetBagType msg = new C_2_S_GetBagType();
            msg.Type = 8;
            NetManager.GetInstance().SendNetMessage(NetID.C_To_S_GetBagTypeMessage,msg.ToByteArray());
        });
    }

    private void GetBagTypeHandler(object obj)
    {
   
        object[] t = obj as object[];
        byte[] data = t[0] as byte[];
        S_2_C_InitialiazeBag s_Data = S_2_C_InitialiazeBag.Parser.ParseFrom(data);
        nowNum = s_Data.NowBagCount;
        maxNum = s_Data.MaxBagCount;
        headText.text = "背包容量"+nowNum+"/"+maxNum; 
        ReFresh(s_Data);
    }

    private void StartHandler(object obj)
    {
        object[] t = obj as object[];
        byte[] data = t[0] as byte[];
        S_2_C_InitialiazeBag s_Data = S_2_C_InitialiazeBag.Parser.ParseFrom(data);
        goldText.text = s_Data.Gold.ToString();
        masonryText.text = s_Data.Masonry.ToString();
        rollText.text = s_Data.Roll.ToString();
        energyText.text = s_Data.Energy.ToString();
        nowNum = s_Data.NowBagCount;
        maxNum = s_Data.MaxBagCount;
        ReFresh(s_Data);
        headText.text = "背包容量"+nowNum+"/"+maxNum; 
    }

    private void ReFresh(S_2_C_InitialiazeBag sData)
    {
        for (int i = 0; i < Content.childCount; i++)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
        Debug.Log(sData.CellList.Count);
        for (int i = 0; i < sData.CellList.Count; i++)
        {
            var item = Instantiate(Resources.Load<GameObject>("BagItem"), Content);
            var bag = item.GetComponent<BagItem>();
            if (sData.CellList[i].State==BagCellState.User)
            {
                var data = ConfigManager.GetInstance().itemList.Find(x => x.ID == sData.CellList[i].ItemId);
                bag.ReFresh(data,sData.CellList[i]);
            }
            else
            {
                bag.ReFresh(null,sData.CellList[i]);
            }
            
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
