using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    public static BagUI Instance;
    void Awake()
    {
        Instance = this;
    }
    public Toggle allTog, wqTog, fjTog, ysTog, clTog;
    public Image addCellTips;
    public Button closeBagBtn;//关闭按钮,扩充按钮

    public Transform parent;//背包格子父物体
    public Text userNum;//显示：已占用格子/已开启格子
    /// <summary>
    /// 背包格子集合
    /// </summary>
    public List<BagItemUI> bagItemUIs = new List<BagItemUI>();
    /// <summary>
    /// 当前显示的物品表
    /// </summary>
    public List<CellsData> nowCellsDatas = new List<CellsData>();

    public List<RoleEqCellUI> roleEqCellUIs = new List<RoleEqCellUI>();
    public Dictionary<int, RoleEqCellUI> roleEqDic = new Dictionary<int, RoleEqCellUI>();
    // Start is called before the first frame update
    void Start()
    {

        MessageControll.GetInstance().AddListener(NetID.S_T_C_Bag_Type_Message, S_T_C_Bag_Type_Message_Handle);
        closeBagBtn.onClick.AddListener(() =>
        {
            Game.Instance.BagPanel.gameObject.SetActive(false);
        });
        C_2_S_Bag_Type_Message c_data = new C_2_S_Bag_Type_Message();
        c_data.GoodsType = GoodsType.All;
        c_data.Port = Game.Instance.port;
        NetManager.GetInstance().SendNetMessage(NetID.C_T_S_Bag_Type_Message, c_data.ToByteArray());
        allTog.onValueChanged.AddListener((bool isOn) =>
        {
            C_2_S_Bag_Type_Message c_data = new C_2_S_Bag_Type_Message();
            c_data.GoodsType = GoodsType.All;
            c_data.Port = Game.Instance.port;
            NetManager.GetInstance().SendNetMessage(NetID.C_T_S_Bag_Type_Message, c_data.ToByteArray());
        });
        wqTog.onValueChanged.AddListener((bool isOn) =>
        {
            C_2_S_Bag_Type_Message c_data = new C_2_S_Bag_Type_Message();
            c_data.GoodsType = GoodsType.WuQi;
            c_data.Port = Game.Instance.port;
            NetManager.GetInstance().SendNetMessage(NetID.C_T_S_Bag_Type_Message, c_data.ToByteArray());
        });
        fjTog.onValueChanged.AddListener((bool isOn) =>
        {
            C_2_S_Bag_Type_Message c_data = new C_2_S_Bag_Type_Message();
            c_data.GoodsType = GoodsType.FangJu;
            c_data.Port = Game.Instance.port;
            NetManager.GetInstance().SendNetMessage(NetID.C_T_S_Bag_Type_Message, c_data.ToByteArray());
        });
        ysTog.onValueChanged.AddListener((bool isOn) =>
        {
            C_2_S_Bag_Type_Message c_data = new C_2_S_Bag_Type_Message();
            c_data.GoodsType = GoodsType.YaoShui;
            c_data.Port = Game.Instance.port;
            NetManager.GetInstance().SendNetMessage(NetID.C_T_S_Bag_Type_Message, c_data.ToByteArray());
        });
        clTog.onValueChanged.AddListener((bool isOn) =>
        {
            C_2_S_Bag_Type_Message c_data = new C_2_S_Bag_Type_Message();
            c_data.GoodsType = GoodsType.CaiLiao;
            c_data.Port = Game.Instance.port;
            NetManager.GetInstance().SendNetMessage(NetID.C_T_S_Bag_Type_Message, c_data.ToByteArray());
        });

        for (int i = 0; i < roleEqCellUIs.Count; i++)
        {
            roleEqDic.Add(i + 1, roleEqCellUIs[i]);
        }

    }
    /// <summary>
    /// 客户端处理服务器发送的类型物品数据
    /// </summary>
    /// <param name="obj"></param>
    private void S_T_C_Bag_Type_Message_Handle(object obj)
    {
        object[] objList = obj as object[];
        byte[] data = objList[0] as byte[];
        S_2_C_Bag_Type_Message s_data = S_2_C_Bag_Type_Message.Parser.ParseFrom(data);

        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        bagItemUIs.Clear();
        //是否是全部都要把传过来的格子实例化，里面有数据，
        for (int i = 0; i < s_data.CellsData.Count; i++)
        {
            GameObject bagCellItem = GameObject.Instantiate(Resources.Load<GameObject>("BagItem"), parent);
            BagItemUI bagItemUI = bagCellItem.GetComponent<BagItemUI>();
            bagItemUI.Init(s_data.CellsData[i]);
            bagItemUIs.Add(bagItemUI);
            nowCellsDatas.Add(s_data.CellsData[i]);
        }
        //判断是否是全部类型，全部类型有格子占用和未解锁格子，要多实例化这些格子
        if (s_data.GoodsType == GoodsType.All)
        {
            //int num=5-2
            int num = s_data.BagCellNum - s_data.CellsData.Count;
            //max=10,num=3
            for (int i = s_data.CellsData.Count; i < s_data.MaxCellNum; i++)
            {
                GameObject bagCellItem = GameObject.Instantiate(Resources.Load<GameObject>("BagItem"), parent);
                BagItemUI bagItemUI = bagCellItem.GetComponent<BagItemUI>();
                CellsData cellsData = new CellsData();
                cellsData.GridId = i;
                cellsData.GoodId = -1;
                cellsData.NowNum = 0;
                if (i < s_data.BagCellNum)
                {
                    cellsData.CellState = 1;
                }
                else
                {
                    cellsData.CellState = 3;
                }

                bagItemUI.Init(cellsData);
                bagItemUIs.Add(bagItemUI);
            }
            userNum.gameObject.SetActive(true);
            userNum.text = s_data.CellsData.Count + "/" + s_data.BagCellNum;
            Game.Instance.nowCellNum = s_data.BagCellNum;
            Game.Instance.maxCellNum = s_data.MaxCellNum;
        }
        else
        {
            userNum.gameObject.SetActive(false);
        }

        //进行排序
    }

    // public void SortNowBag(List<CellsData> cellsDatas=null)
    // {
    //     //如果没有传过来需要的集合
    //     if(cellsDatas==null)
    //     {

    //     }
    // }
}
