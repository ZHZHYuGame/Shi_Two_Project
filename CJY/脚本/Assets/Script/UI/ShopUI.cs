using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopUI : MonoBehaviour
{
    public GameObject shopUI;
    public List<ShopData> ShopList=new List<ShopData>();
    public Transform shopParent;
    public Button specialBtn,dailyBtn,normalBtn;
    public static ShopUI ins;
    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
    }

    void Start()
    {
        MessageControll.Instance.AddListener(NetID.S_To_C_Shop_Type_Datas_Message, S_To_C_Shop_Type);
        Invoke("Request_ShopData", 1);
        specialBtn.onClick.AddListener(() =>
        {
            MessageControll.Instance.AddListener(NetID.S_To_C_Shop_Type_Datas_Message, S_To_C_Shop_Type);
        });
        gameObject.SetActive(false);
    }
    void Request_ShopData()
    {
        C_2_S_SpecialShops c_Data=new C_2_S_SpecialShops();
        c_Data.Type = ShopsType.EqShop;
        c_Data.UpdateType = ShopsUpdateType.None;
        NetManager.Instance.SendNetMessage(NetID.C_To_S_Shop_Type_Datas_Message, c_Data.ToByteArray());
    }
    private void S_To_C_Shop_Type(object obj)
    {
        print(111);
        object[] data = obj as object[];
        byte[] netData=data[0] as byte[];
        S_2_C_SpecialShops s_data = S_2_C_SpecialShops.Parser.ParseFrom(netData);
        switch (s_data.ShopUpdateType)
        {
            case ShopsUpdateType.None:
                print(s_data.GoodsList.Count);
                for (int i = 0; i < s_data.GoodsList.Count; i++)
                {
                    print(s_data.GoodsList[i].ShopGoodsID);
                    ShopData shop = ConfigManager.Instance.FindData(s_data.GoodsList[i].ShopGoodsID);
                    ShopList.Add(shop);
                }
                for (int i = 0; i < ShopList.Count; i++)
                {
                    var shopItem=Instantiate(Resources.Load<ShopItem>("ShopItem"), shopParent);
                    shopItem.OnInit(ShopList[i]);
                }
                break;
            case ShopsUpdateType.FreeUpdate:
                break;
            case ShopsUpdateType.GlodUpdate:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
