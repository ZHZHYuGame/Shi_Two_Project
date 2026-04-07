using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Google.Protobuf;
using Message;
using UnityEngine;
using UnityEngine.UI;

public class SellPanel : MonoBehaviour
{
    public Button add,reduce,yes,no;
    public Text numText, sumText;
    BagItem myItem;
    public int num, sum;
    public void Init(BagItem item)
    { 
        ItemData data = ConfigManger.GetInstance.GetItemData(item.ItemId);
        myItem = item;
        num = 1;
        sum = num * data.SellPrice;
        numText.text = num.ToString();
        sumText.text = sum.ToString();
        add.onClick.AddListener(() =>
        {
            if (num >=item.Count)
            {
                return;
            }
            num++;
            sum = num * data.SellPrice;
            numText.text = num.ToString();
            sumText.text = sum.ToString();
        });
        reduce.onClick.AddListener(() =>
        {
            if (num <= 0)
            {
                return;
            }
            num--;
            sum = num * data.SellPrice;
            numText.text = num.ToString();
            sumText.text = sum.ToString();
        });
        yes.onClick.AddListener(() =>
        {
            C2S_GXUI c_datas=new C2S_GXUI();
            C2S_SellGood c_data=new C2S_SellGood();
            c_data.Uid=item.Uid;
            c_data.Num = num;
            c_datas.ChangeMoney = sum;
            NetManger.GetInstance.SendMessage(NetID.C_2_S_SellGood, c_data.ToByteArray());
            NetManger.GetInstance.SendMessage(NetID.C_2_S_Money, c_datas.ToByteArray());
            gameObject.SetActive(false);
        });
        no.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
