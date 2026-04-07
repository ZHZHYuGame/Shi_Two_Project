using System.Collections;
using System.Collections.Generic;
using Game;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class SellPanel : MonoBehaviour
{
    private BagData mydata;
    [SerializeField] private Text countText,sellText;
    private int count = 1;
    private float sell = 0;

    public void SetValue(BagData mydata)
    {
        this.mydata = mydata;
        count = 1;
        countText.text = count.ToString();
        sell = mydata.Item.Price;
        sellText.text = "出售获得:"+sell;
    }

    public void OnAddBtn()
    {
        count++;
        count = Mathf.Clamp(count, 1, mydata.Count);
        sell = mydata.Item.Price * count;
        countText.text = count.ToString();
        sellText.text = "出售获得:"+sell;
    }
    
    public void OnSubBtn()
    {
        count--;
        count = Mathf.Clamp(count, 1, mydata.Count);
        sell = mydata.Item.Price * count;
        countText.text = count.ToString();
        sellText.text = "出售获得:"+sell;
    }

    public void YesBtnClick()
    {
        C_To_S_Sell_Bag_Message c_data = new();
        c_data.Data = mydata;
        c_data.Count = count;
        NetManager.Ins.SendMessage(NetID.C_To_S_Sell_Item_Message, c_data.ToByteArray());
        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void NoBtnClick()
    {
        gameObject.SetActive(false);
    }
}
