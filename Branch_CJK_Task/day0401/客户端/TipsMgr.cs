using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Message;
using UnityEngine;
using UnityEngine.UI;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

public class TipsMgr : MonoBehaviour
{
    public static TipsMgr instance;

    private void Awake()
    {
        instance = this;
    }

    public UnityEngine.UI.Image icon; 
    public GameObject zb;//是否穿戴
    public Text name,level,quilaty;
    public Transform parent;//星星
    public Text attack, fy, speed, hp, ml, sum;
    public Transform Property;//随机字段
    public Text skill;
    public UnityEngine.UI.Image dem1, dem2, dem3;
    public Text des,sale;
    public float allSum;
    public Button b1, b2, b3,b4,b5,b6,close;
    public Button a1, a2;
    public GameObject btns,btn2;
    public void Init(BagItem item,bool isJude,bool isChuang)
    {
        ItemData itemData = ConfigManger.GetInstance.GetItemData(item.ItemId);
        zb.gameObject.SetActive(isChuang);
        b3.gameObject.SetActive(isChuang);
        icon.sprite = Resources.Load<Sprite>("icon/" + itemData.Icon);
        name.text = itemData.Name;
        level.text=itemData.Priority.ToString();
        quilaty.text="精炼"+itemData.Quality+"阶";
        for (int i = 0; i < itemData.Quality; i++)
        {
            Instantiate(Resources.Load<GameObject>("leveIcon"),parent);
        }
        if (itemData.Type == 1 || itemData.Type == 2)
        {
            btn2.gameObject.SetActive(false);
            btns.gameObject.SetActive(true);
            if (isJude)
            {
                b2.gameObject.SetActive(true);
                b1.gameObject.SetActive(false);
            }
            else
            {
                b1.gameObject.SetActive(true);
                b2.gameObject.SetActive(false);
            }
            attack.text = "攻击力" + itemData.BaseAttr.attack*item.Extra.EnhanceLevel;
            fy.text = "防御力" + itemData.BaseAttr.defense*item.Extra.EnhanceLevel;
            speed.text="速度"+itemData.BaseAttr.speed*item.Extra.EnhanceLevel;
            hp.text="血量"+itemData.BaseAttr.hp*item.Extra.EnhanceLevel;
            ml.text="魔力"+itemData.BaseAttr.mp*item.Extra.EnhanceLevel;
            allSum=itemData.BaseAttr.attack+itemData.BaseAttr.defense+itemData.BaseAttr.speed+itemData.BaseAttr.hp+itemData.BaseAttr.mp+itemData.WashableAttr.Count*5+itemData.Skills.Count*3+itemData.SocketSlotTypes.Count*5;
            sum.text = "总加成:" + allSum;
            for (int i = 0; i < itemData.WashableAttr.Count; i++)
            {
                Instantiate(Resources.Load<GameObject>("Property"),Property);
            }
            if (itemData.Skills.Count > 0)
            {
                skill.text=itemData.Skills[0].Desc;
            }
            int index=itemData.SocketSlotTypes.Count;
            switch (index)
            {
                case 0:
                    break;
                case 1:
                    dem1.sprite = Resources.Load<Sprite>("Stone/"+itemData.SocketSlotTypes[0].icon);
                    break;
                case 2:
                    dem1.sprite = Resources.Load<Sprite>("Stone/"+itemData.SocketSlotTypes[0].icon);
                    dem2.sprite = Resources.Load<Sprite>("Stone/"+itemData.SocketSlotTypes[1].icon);
                    break;
                case 3:
                    dem1.sprite = Resources.Load<Sprite>("Stone/"+itemData.SocketSlotTypes[0].icon);
                    dem2.sprite = Resources.Load<Sprite>("Stone/"+itemData.SocketSlotTypes[1].icon);
                    dem3.sprite = Resources.Load<Sprite>("Stone/"+itemData.SocketSlotTypes[2].icon);
                    break;
            }
        }
        else
        {
            btns.gameObject.SetActive(false);
            btn2.gameObject.SetActive(true);
        }
        des.text = itemData.Msg;
        sale.text="出售价格"+itemData.SellPrice.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        close.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
