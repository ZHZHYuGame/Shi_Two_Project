using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Message;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItemUI : MonoBehaviour, IPointerDownHandler
{
    // UI组件引用
    public Transform canvas;
    public GameObject keep; // 锁定的遮罩
    public Image icon; // 物品图标
    public Image bj; // 背景（品质框）
    public Text num; // 数量文本
    public Text show; // 特殊显示（暂未使用）
    public Transform levelParent; // 等级图标的父节点

    // 数据
    public bool isOpen = false; //是否解锁
    public bool isChuang = false; //是否穿戴
    public int girId; // 格子索引（对应背包位置）
    public BagItem myItem = null; // 当前格子的背包数据
    
    private void Awake()
    {
        // 初始化UI状态
        keep.SetActive(true);
        show.gameObject.SetActive(false);
        bj = GetComponent<Image>(); // 获取背景组件
        ClearLevelIcons(); // 确保没有残留的等级图标
        num.gameObject.SetActive(false); // 默认隐藏数量
        icon.sprite = Resources.Load<Sprite>("bk/" + "0");
        canvas=GameObject.Find("Canvas").GetComponent<Transform>();
    }

    public void ShowKeep(bool isShow)
    {
        isChuang =isShow;
        show.gameObject.SetActive(isChuang);
    }
    /// <summary>
    /// 清除等级图标（避免重复生成）
    /// </summary>
    private void ClearLevelIcons()
    {
        if (levelParent == null) return;
        for (int i = levelParent.childCount - 1; i >= 0; i--)
        {
            Destroy(levelParent.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 初始化格子（首次显示）
    /// </summary>
    public void Init(BagItem item)
    {
        if (item == null) return;
        myItem = item;
        UpdateUI(item);
    }

    /// <summary>
    /// 解锁格子（隐藏遮罩）
    /// </summary>
    public void Open()
    {
        keep.SetActive(false);
        isOpen = true;
    }

    /// <summary>
    /// 刷新格子数据（用于动态更新）
    /// </summary>
    public void RefreshItem(BagItem item)
    {
        if (item == null)
        {
            // 清空格子
            myItem = null;
            icon.sprite = null;
            icon.sprite = Resources.Load<Sprite>("bk/" + "0");
            bj.sprite = Resources.Load<Sprite>("bk/" + "0");
            num.text = "";
            num.gameObject.SetActive(false);
            ClearLevelIcons();
        }
        else
        {
            myItem = item;
            UpdateUI(item);
        }
    }

    /// <summary>
    /// 核心UI更新逻辑（根据物品类型显示图标、数量、等级）
    /// </summary>
    private void UpdateUI(BagItem item)
    {
        ItemData itemData = ConfigManger.GetInstance.GetItemData(item.ItemId);
        if (itemData == null) return;

        // 设置图标和品质背景
        icon.sprite = Resources.Load<Sprite>("icon/" + itemData.Icon);
        bj.sprite = Resources.Load<Sprite>("bk/" + itemData.Quality);
        girId = item.Position;

        // 清空旧的等级图标
        ClearLevelIcons();

        // 装备（Type 1或2）显示等级图标，不显示数量
        if (itemData.Type == 1 || itemData.Type == 2)
        {
            num.gameObject.SetActive(false);
            // 根据品质生成对应数量的星星/等级图标
            for (int i = 0; i < itemData.Quality; i++)
            {
                Instantiate(Resources.Load<GameObject>("leveIcon"), levelParent);
            }
        }
        else
        {
            // 普通道具显示数量
            num.gameObject.SetActive(true);
            num.text = item.Count.ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (myItem == null) return;
        ItemData itemData = ConfigManger.GetInstance.GetItemData(myItem.ItemId);
        bool isJude=BagUI.instance.JudgeItem(itemData.Type);
        GameObject go=Instantiate(Resources.Load<GameObject>("Tips"),canvas);
        go.gameObject.transform.position = Input.mousePosition;
        go.GetComponent<TipsMgr>().Init(myItem,isJude,isChuang);
    }
}