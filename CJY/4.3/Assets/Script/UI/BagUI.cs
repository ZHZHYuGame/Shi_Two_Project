using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    public GameObject bagUI;
    public Transform bagParent;
    public List<BagItem> bagItems=new List<BagItem>();
    public Button allBtn,weaponBtn,materialBtn,armorBtn,medicineBtn,sortBtn;
    public static BagUI ins;
    public List<BagData> BagDatas=new List<BagData>();
    public GameObject tips;
    [SerializeField] private Image tipsIcon;
    [SerializeField] private Text tipsName;
    [SerializeField] private Text tipsDes;
    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
    }
    void Start()
    {
        MessageControll.Instance.AddListener(NetID.S_To_C_GetBagData_Message, S_To_C_GetBagData);
        Invoke("Request_BagData", 1);
        allBtn.onClick.AddListener(() =>
        {
            MessageControll.Instance.AddListener(NetID.S_To_C_GetBagData_Message, S_To_C_GetBagData);
            Request_BagData();
        });
        weaponBtn.onClick.AddListener(() =>
        {
            print(1234);
            MessageControll.Instance.AddListener(NetID.S_To_C_GetBagData_Message, S_To_C_GetBagData);
            Weapon_BagData();
        });
        materialBtn.onClick.AddListener(() =>
        {
            MessageControll.Instance.AddListener(NetID.S_To_C_GetBagData_Message, S_To_C_GetBagData);
            Material_BagData();
        });
        armorBtn.onClick.AddListener(() =>
        {
            MessageControll.Instance.AddListener(NetID.S_To_C_GetBagData_Message, S_To_C_GetBagData);
            Armor_BagData();
        });
        medicineBtn.onClick.AddListener(() =>
        {
            MessageControll.Instance.AddListener(NetID.S_To_C_GetBagData_Message, S_To_C_GetBagData);
            Medicine_BagData();
        });
        sortBtn.onClick.AddListener(() =>
        {
            BagDatas.Sort(Temp);
            BagRefresh(BagDatas);
        });
    }

    private int Temp(BagData x, BagData y)
    {
        if (int.Parse(x.Shop.Sale)>int.Parse(y.Shop.Sale))
        {
            return -1;
        }else if (int.Parse(x.Shop.Sale)<int.Parse(y.Shop.Sale))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void Medicine_BagData()
    {
        C_To_S_Bags c_Data=new C_To_S_Bags();
        c_Data.BagType = BagType.Medicine;
        NetManager.Instance.SendNetMessage(NetID.C_To_S_GetBagData_Message, c_Data.ToByteArray());
        print(12345);
    }

    void Armor_BagData()
    {
        C_To_S_Bags c_Data=new C_To_S_Bags();
        c_Data.BagType = BagType.Armor;
        NetManager.Instance.SendNetMessage(NetID.C_To_S_GetBagData_Message, c_Data.ToByteArray());
    }

    void Material_BagData()
    {
        C_To_S_Bags c_Data=new C_To_S_Bags();
        c_Data.BagType = BagType.Material;
        NetManager.Instance.SendNetMessage(NetID.C_To_S_GetBagData_Message, c_Data.ToByteArray());
    }

    void Weapon_BagData()
    {
        C_To_S_Bags c_Data=new C_To_S_Bags();
        c_Data.BagType = BagType.Weapon;
        NetManager.Instance.SendNetMessage(NetID.C_To_S_GetBagData_Message, c_Data.ToByteArray());
    }

    private void S_To_C_GetBagData(object obj)
    {
        object[] data = obj as object[];
        byte[] netData=data[0] as byte[];
        S_To_C_Bags s_data = S_To_C_Bags.Parser.ParseFrom(netData);
        
        for (int i = 0; i < 30; i++)
        {
            var bagItem=Instantiate(Resources.Load<BagItem>("BagItem"), bagParent);
            bagItems.Add(bagItem);
        }
        switch (s_data.BagType)
        {
            case BagType.All:
                BagDatas.Clear();
                for (int i = 0; i < s_data.GoodsList.Count; i++)
                {
                    print(s_data.GoodsList[i].ItemID);
                    ShopData shop = ConfigManager.Instance.FindData(s_data.GoodsList[i].ItemID);
                    BagData bag=new BagData(){ID=i,Shop=shop};
                    BagDatas.Add(bag);
                }
                BagRefresh(BagDatas);
                break;
            case BagType.Material:
                BagDatas.Clear();
                for (int i = 0; i < s_data.GoodsList.Count; i++)
                {
                    print(s_data.GoodsList[i].ItemID);
                    ShopData shop = ConfigManager.Instance.FindData(s_data.GoodsList[i].ItemID);
                    BagData bag=new BagData(){ID=i,Shop=shop};
                    if (bag.Shop.EquipType=="宝箱")
                    {
                        BagDatas.Add(bag);
                    }
                    BagRefresh(BagDatas);
                }
                break;
            case BagType.Weapon:
                BagDatas.Clear();
                for (int i = 0; i < s_data.GoodsList.Count; i++)
                {
                    print(s_data.GoodsList[i].ItemID);
                    ShopData shop = ConfigManager.Instance.FindData(s_data.GoodsList[i].ItemID);
                    BagData bag=new BagData(){ID=i,Shop=shop};
                    if (bag.Shop.EquipType=="武器")
                    {
                        BagDatas.Add(bag);
                    }
                    BagRefresh(BagDatas);
                }
                break;
            case BagType.Armor:
                BagDatas.Clear();
                for (int i = 0; i < s_data.GoodsList.Count; i++)
                {
                    print(s_data.GoodsList[i].ItemID);
                    ShopData shop = ConfigManager.Instance.FindData(s_data.GoodsList[i].ItemID);
                    BagData bag=new BagData(){ID=i,Shop=shop};
                    if (bag.Shop.EquipType!="武器" && bag.Shop.EquipType!="宝箱" && bag.Shop.EquipType!="药品")
                    {
                        BagDatas.Add(bag);
                    }
                    BagRefresh(BagDatas);
                }
                break;
            case BagType.Medicine:
                BagDatas.Clear();
                for (int i = 0; i < s_data.GoodsList.Count; i++)
                {
                    print(s_data.GoodsList[i].ItemID);
                    ShopData shop = ConfigManager.Instance.FindData(s_data.GoodsList[i].ItemID);
                    BagData bag=new BagData(){ID=i,Shop=shop};
                    if (bag.Shop.EquipType=="药品")
                    {
                        BagDatas.Add(bag);
                    }
                    BagRefresh(BagDatas);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void BagRefresh(List<BagData> BagDatas)
    {
        for (int i = 0; i < bagItems.Count; i++)
        {
            if (i<BagDatas.Count)
            {
                bagItems[i].OnInit(i,BagDatas[i]);
            }
            else
            {
                bagItems[i].OnInit(i,null);
            }
        }
    }

    void Request_BagData()
    {
        C_To_S_Bags c_Data=new C_To_S_Bags();
        c_Data.BagType = BagType.All;
        NetManager.Instance.SendNetMessage(NetID.C_To_S_GetBagData_Message, c_Data.ToByteArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenTips(BagData data)
    {
        tipsIcon.sprite = Resources.Load<Sprite>("icon/" + data.Shop.Icon);
        tipsName.text = data.Shop.Name;
        tipsDes.text = data.Shop.Des;
    }
}
