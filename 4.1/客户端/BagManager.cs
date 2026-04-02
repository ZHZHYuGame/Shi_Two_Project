using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class BagManager : MonoBehaviour
{
    [SerializeField] private BagCell bagCellPrefab;
    [SerializeField] private Transform con;
    [SerializeField] List<Toggle> toggles;
    [SerializeField] private Text cellCountText,atkText;
    [SerializeField] List<EqCell>  eqCellList = new List<EqCell>();

    public int PlayerAtk = 0;
    public TipsPanel tips;
    public int flag = 0;  //当前是什么标签的背包
    public int BagCellCount = 30;
    public List<BagCell> bagCellList = new();
    public List<BagData> bagList = new();
    public Dictionary<EqPositionType, EqCell> eqDic = new();

    private void Awake()
    {
        MessageCenter.Ins.AddListener(NetID.S_To_C_Get_Bag_Data_Message,GetBagData);
        MessageCenter.Ins.AddListener(NetID.S_To_C_Wear_Eq_Message,WearEq);
    }

    private void WearEq(object obj)
    {
        object[] objs = obj as object[];
        var data = objs[0] as byte[];
        
        S_To_C_Wear_Eq_Message s_data = S_To_C_Wear_Eq_Message.Parser.ParseFrom(data);
        switch (s_data.Result)
        {
            case ResultType.Yes:
                UpdateEqCell(s_data.ItemList.ToList());
                Debug.Log("成功穿戴!");
                break;
            case ResultType.No:
                Debug.Log("不能穿戴/穿戴错误");
                break;
        }
    }

    private void UpdateEqCell(List<Item> list)
    {
        PlayerAtk = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (!string.IsNullOrEmpty(list[i].Icon))
            {
                eqDic[list[i].Position].SetValue(list[i]);
                PlayerAtk += list[i].Atk;
            }
        }
        atkText.text = PlayerAtk.ToString();
    }

    /// <summary>
    ///  初始化穿戴的格子的字典
    /// </summary>
    void InitEqCell()
    {
        PlayerAtk = 0;
        for (int i = 0; i < eqCellList.Count; i++)
        {
            eqCellList[i].SetValue(new Item());
            eqDic.Add(eqCellList[i].flag, eqCellList[i]);
        }
        atkText.text = PlayerAtk.ToString();
    }
    
    private void GetBagData(object obj)
    {
        object[] objs = obj as object[];
        var data = objs[0] as byte[];
        Debug.Log("成功获取背包数据");
        S_To_C_Get_Bag_Data_Message s_Data = S_To_C_Get_Bag_Data_Message.Parser.ParseFrom(data);
        var list = s_Data.BagList.ToList();
        UpdateBagDataList(list);
    }

    void UpdateBagDataList(List<BagData> list)
    {
        for (int i = 0; i < bagList.Count; i++)
        {
            if (i >= list.Count)
            {
                bagList[i].Item = null;
                bagList[i].Count = 0;
                bagList[i].AllIndex = -1;
            }
            else
            {
                bagList[i].Item = list[i].Item;
                bagList[i].Count = list[i].Count;
                bagList[i].AllIndex = list[i].AllIndex;
            }
        }
        OnUpdateBagCell(1);
    }

    public void OnCloseBagPanel()
    {
        gameObject.SetActive(false);
    }

    public void OnOpenBagPanel()
    {
        gameObject.SetActive(true);
        OnChangeBagPanel();
    }
    
    /// <summary>
    ///  根据类型更新背包格子
    ///  flag为0就是初始化
    ///  flag为其他就是直接按照当前的背包list数据赋值
    /// </summary>
    /// <param name="flag"></param>
    public void OnUpdateBagCell(int flag)
    {
        if (flag == 0)
        {
            for (int i = 0; i < BagCellCount; i++)
            {
                var bagCell = Instantiate(bagCellPrefab, con);
                bagCell.SetValue(bagList[i]);
                bagCellList.Add(bagCell);
            }
        }
        else
        {
            for (int i = 0; i < bagCellList.Count; i++)
            {
                bagCellList[i].SetValue(bagList[i]);
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InitBagCell();
        InitToggle();
        InitEqCell();
        OnUpdateBagCell(0);
        OnCloseBagPanel();
        Debug.Log("背包......");
    }

    /// <summary>
    /// 初始化背包30个格子
    /// </summary>
    private void InitBagCell()
    {
        for (int i = 0; i < BagCellCount; i++)
        {
            var bagData = new BagData
            {
                Item = null,
                Count = 0,
                Index = i
            };
            bagList.Add(bagData);
        }
    }

    /// <summary>
    /// 初始化分类的标签
    /// </summary>
    private void InitToggle()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    flag = index;
                    OnChangeBagPanel();
                }
            });
        }
    }

    private void OnChangeBagPanel()
    {
        C_To_S_Get_Bag_Data_Message c_Data = new C_To_S_Get_Bag_Data_Message();
        switch (flag)
        {
            case 0:
                c_Data.Type = "全部";
                break;
            case 1:
                c_Data.Type = "装备";
                break;
            case 2:
                c_Data.Type = "药品";
                break;
        }
        NetManager.Ins.SendMessage(NetID.C_To_S_Get_Bag_Data_Message, c_Data.ToByteArray());
    }
    
}
