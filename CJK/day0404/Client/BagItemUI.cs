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
   public GameObject keep;          // 锁定的遮罩
    public Image icon;               // 物品图标
    public Image bj;                 // 背景（品质框）
    public Text num;                 // 数量文本
    public Text show;                // 特殊显示（如“已穿戴”）
    public Transform levelParent;    // 等级/强化图标的父节点

    private int slotIndex;           // 格子索引
    private BagItem myItem;          // 当前格子的道具数据
    private bool isLocked;           // 是否未解锁

    private void Awake()
    {
        keep.SetActive(true);
        show.gameObject.SetActive(false);
        bj = GetComponent<Image>();
        num.gameObject.SetActive(false);
        icon.sprite = Resources.Load<Sprite>("bk/0");
        ClearLevelIcons();
    }

    /// <summary>
    /// 初始化格子（设置索引和解锁状态）
    /// </summary>
    public void InitSlot(int index, bool unlocked)
    {
        slotIndex = index;
        if (unlocked)
            SetLocked(false);
        else
            SetLocked(true);
    }

    /// <summary>
    /// 设置锁定状态
    /// </summary>
    public void SetLocked(bool locked)
    {
        isLocked = locked;
        keep.SetActive(locked);
        if (locked) SetEmpty();
    }

    /// <summary>
    /// 清空格子
    /// </summary>
    public void SetEmpty()
    {
        myItem = null;
        icon.sprite = Resources.Load<Sprite>("bk/0");
        bj.sprite = Resources.Load<Sprite>("bk/0");
        num.text = "";
        num.gameObject.SetActive(false);
        ClearLevelIcons();
        show.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置道具显示
    /// </summary>
    public void SetItem(BagItem item)
    {
        myItem = item;
        UpdateUI(item);
    }

    /// <summary>
    /// 清除等级图标
    /// </summary>
    private void ClearLevelIcons()
    {
        if (levelParent == null) return;
        for (int i = levelParent.childCount - 1; i >= 0; i--)
            Destroy(levelParent.GetChild(i).gameObject);
    }

    /// <summary>
    /// 更新UI显示
    /// </summary>
    private void UpdateUI(BagItem item)
    {
        ItemData itemData = ConfigManger.GetInstance.GetItemData(item.ItemId);
        if (itemData == null) return;

        icon.sprite = Resources.Load<Sprite>("icon/" + itemData.Icon);
        bj.sprite = Resources.Load<Sprite>("bk/" + itemData.Quality);

        ClearLevelIcons();

        if (itemData.Type == 1 || itemData.Type == 2) // 装备
        {
            num.gameObject.SetActive(false);
            // 显示强化等级或精炼等级图标（示例：根据强化等级生成）
            int level = item.Extra?.EnhanceLevel ?? 0;
            for (int i = 0; i < level; i++)
            {
                Instantiate(Resources.Load<GameObject>("leveIcon"), levelParent);
            }
            // 如果已穿戴，显示“已穿戴”标记（需从外部判断）
            // 这里通过 show 文本显示，由外部调用 ShowEquippedMark
        }
        else
        {
            num.gameObject.SetActive(true);
            num.text = item.Count.ToString();
        }
    }

    /// <summary>
    /// 显示已穿戴标记
    /// </summary>
    public void ShowEquippedMark(bool showMark)
    {
        show.gameObject.SetActive(showMark);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isLocked || myItem == null) return;
            ItemData itemData = ConfigManger.GetInstance.GetItemData(myItem.ItemId);
            // 弹出Tips，传递当前道具以及是否已穿戴（这里需要外部获取，可以从 BagUI 查询）
            bool isEquipped = BagUI.instance.IsItemEquipped(myItem.Uid); // 需要在 BagUI 中实现
            GameObject go = Instantiate(Resources.Load<GameObject>("Tips"), BagUI.instance.transform);
            go.transform.position = Input.mousePosition;
            go.GetComponent<TipsMgr>().Init(myItem, isEquipped);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
               if(myItem!=null||isLocked==false)return;
               BagUI.instance.Unlock.SetActive(true);
               BagUI.instance.Unlock.GetComponent<UnlockPanel>().Init(10);
               // BagUI.instance.Unlock.GetComponent<UnlockPanel>().Init(BagUI.instance.);
        }
       
    }
}