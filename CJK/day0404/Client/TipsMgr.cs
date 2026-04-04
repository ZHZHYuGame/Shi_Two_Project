using System;
using System.Collections;
using System.Collections.Generic;
using ComponentAce.Compression.Libs.zlib;
using DefaultNamespace;
using Google.Protobuf;
using Message;
using UnityEngine;
using UnityEngine.UI;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

public class TipsMgr : MonoBehaviour
{
    public static TipsMgr instance;

    public UnityEngine.UI.Image icon;
    public GameObject zb;               // "已穿戴" 标记
    public Text name, level, quilaty;
    public Transform parent;            // 品质星星父节点
    public Text attack, fy, speed, hp, ml, sum;
    public Transform propertyParent;    // 可洗练属性列表父节点
    public Text skill;
    public UnityEngine.UI.Image dem1, dem2, dem3;      // 宝石槽位图标
    public Text des, sale;
    public float allSum;
    public Button b1, b2,b3,b7,a3,close;        // b1:装备/替换, b2:卸下
    public GameObject btns, btn2;       // btns:装备按钮组, btn2:非装备按钮组
    public BagItem myItem;
    public bool isC = false;            // 是否已穿戴

    private void Awake()
    {
        instance = this;
    }

    public void Init(BagItem item, bool isChuang)
    {
        myItem = item;
        isC = isChuang;

        ItemData itemData = ConfigManger.GetInstance.GetItemData(item.ItemId);
        if (itemData == null) return;

        // 基础信息
        zb.SetActive(isChuang);
        icon.sprite = Resources.Load<Sprite>("icon/" + itemData.Icon);
        name.text = itemData.Name;
        level.text = "等级 " + itemData.MinLevel;          // 这里改用 MinLevel 更合理
        quilaty.text = "品质 " + itemData.Quality;         // 简化显示

        // 品质星星
        ClearChildren(parent);
        for (int i = 0; i < itemData.Quality; i++)
        {
            Instantiate(Resources.Load<GameObject>("leveIcon"), parent);
        }

        // 装备类（Type 1或2）
        if (itemData.Type == 1 || itemData.Type == 2)
        {
            btn2.SetActive(false);
            btns.SetActive(true);

            // 按钮显示逻辑
            if (isChuang)
            {
                b3.gameObject.SetActive(true);  // 卸下按钮
                b2.gameObject.SetActive(false);
                b1.gameObject.SetActive(false);
            }
            else
            {
                b1.gameObject.SetActive(true);  // 装备/替换按钮
                b2.gameObject.SetActive(false);
                b3.gameObject.SetActive(false);
            }

            // 强化加成系数（每级提升10%）
            int enhanceLevel = item.Extra?.EnhanceLevel ?? 0;
            float factor = 1 + enhanceLevel * 0.1f;

            EquipData baseAttr = itemData.BaseAttr;
            attack.text = "攻击力 " + (baseAttr.attack * factor).ToString("F0");
            fy.text = "防御力 " + (baseAttr.defense * factor).ToString("F0");
            speed.text = "速度 " + (baseAttr.speed * factor).ToString("F0");
            hp.text = "血量 " + (baseAttr.hp * factor).ToString("F0");
            ml.text = "魔力 " + (baseAttr.mp * factor).ToString("F0");

            // 计算总加成：基础属性总和 + 洗练属性总和 + 技能数量*3 + 宝石槽位数*5
            float baseSum = baseAttr.attack + baseAttr.defense + baseAttr.speed + baseAttr.hp + baseAttr.mp;
            float washSum = 0;
            if (itemData.WashableAttr != null)
            {
                foreach (var wash in itemData.WashableAttr)
                    washSum += wash.NowData;   // 使用 nowData
            }
            float skillBonus = (itemData.Skills?.Count ?? 0) * 3;
            float gemBonus = (itemData.SocketSlotTypes?.Count ?? 0) * 5;
            allSum = baseSum + washSum + skillBonus + gemBonus;
            sum.text = "总加成: " + allSum.ToString("F0");

            // 可洗练属性显示
            ClearChildren(propertyParent);
            if (itemData.WashableAttr != null)
            {
                foreach (var wash in itemData.WashableAttr)
                {
                    GameObject propObj = Instantiate(Resources.Load<GameObject>("Property"), propertyParent);
                    Text propText = propObj.GetComponentInChildren<Text>();
                    if (propText != null)
                        propText.text = $"{wash.AttrType}: +{wash.NowData}";
                }
            }

            // 附加技能
            if (itemData.Skills != null && itemData.Skills.Count > 0)
                skill.text = itemData.Skills[0].Desc;
            else
                skill.text = "无";

            // 宝石槽位
            List<Gem> gems = itemData.SocketSlotTypes;
            UnityEngine.UI.Image[] slots = { dem1, dem2, dem3 };
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < (gems?.Count ?? 0))
                {
                    Gem gem = gems[i];
                    slots[i].sprite = Resources.Load<Sprite>("Stone/" + gem.icon);
                    slots[i].gameObject.SetActive(true);
                }
                else
                {
                    slots[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // 非装备类
            btns.SetActive(false);
            btn2.SetActive(true);
        }

        des.text = itemData.Msg;
        sale.text = "出售价格 " + itemData.SellPrice;
    }

    void Start()
    {
        close.onClick.AddListener(() => Destroy(gameObject));
        b1.onClick.AddListener(() => { SendApparelRequest(false); Destroy(gameObject); });
        b3.onClick.AddListener(() => { SendApparelRequest(true); Destroy(gameObject); });
        b7.onClick.AddListener(() =>
        {
            BagUI.instance.SellPanel.gameObject.SetActive(true);
            BagUI.instance.SellPanel.gameObject.GetComponent<SellPanel>().Init(myItem);
            Destroy(gameObject);
        });
        a3.onClick.AddListener(() =>
        {
            BagUI.instance.SellPanel.gameObject.SetActive(true);
            BagUI.instance.SellPanel.gameObject.GetComponent<SellPanel>().Init(myItem);
            Destroy(gameObject);
        });
    }

    private void SendApparelRequest(bool isUnequip)
    {
        C2S_Apparel req = new C2S_Apparel();
        req.UId = myItem.Uid;
        req.IsUnequip = isUnequip;

        // if (!isUnequip)
        // {
        //     ItemData itemData = ConfigManger.GetInstance.GetItemData(myItem.ItemId);
        //     if (itemData != null)
        //         req.Slot = GetEquipSlotBySubType(itemData.SubType);
        // }
        ItemData itemData = ConfigManger.GetInstance.GetItemData(myItem.ItemId);
        if (itemData != null)
            req.Slot = GetEquipSlotBySubType(itemData.SubType);
        print(req.Slot);
        print(req.UId);
        // 卸下时 slot 字段服务器可能忽略，但随便填一个
        NetManger.GetInstance.SendMessage(NetID.C_2_S_Apparel, req.ToByteArray());
    }

    private EquipSlot GetEquipSlotBySubType(int subType)
    {
        switch (subType)
        {
            case 101: return EquipSlot.Weapon;
            case 102: return EquipSlot.Head;
            case 103: return EquipSlot.Chest;
            case 104: return EquipSlot.Necklace;
            case 105: return EquipSlot.Ring;
            case 106: return EquipSlot.Legs;
            case 107: return EquipSlot.Boots;
            default: return EquipSlot.Weapon;
        }
    }

    private void ClearChildren(Transform parent)
    {
        if (parent == null) return;
        for (int i = parent.childCount - 1; i >= 0; i--)
            Destroy(parent.GetChild(i).gameObject);
    }
}
