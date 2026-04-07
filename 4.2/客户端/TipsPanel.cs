using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : MonoBehaviour
{
    [SerializeField] private Image bg, icon;
    [SerializeField] private Text nameText, typeText, positionText, atkText, desText,priceText;
    [SerializeField] private Transform starCon;
    [SerializeField] private Button replaceBtn, equippedBtn, sellBtn, pushBtn;
    [SerializeField] private SellPanel sellPanel;
    

    public BagData mydata;
    private float timer = 3;
    public Item eqItem;
    
    public void SetData(BagData data = null ,Item item = null)
    {
        timer = 3;
        if (data != null)
        {
            mydata = data;
            UpdateBagPanel();
            transform.position = Input.mousePosition + new Vector3(-500,0,0);
        }
        if (item != null)
        {
            eqItem = item;
            UpdateEqPanel();
            transform.position = Input.mousePosition + new Vector3(500,0,0);
        }
    }

    private void UpdateEqPanel()
    {
        starCon.gameObject.SetActive(true);
        positionText.gameObject.SetActive(true);
        // 按钮
        replaceBtn.gameObject.SetActive(false);
        equippedBtn.gameObject.SetActive(false);
        pushBtn.gameObject.SetActive(true);
        sellBtn.gameObject.SetActive(false);

        // 星星数量
        for (int i = starCon.childCount - 1 ; i >= 0; i--)
        {
            starCon.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < eqItem.Quality; i++)
        {
            starCon.GetChild(i).gameObject.SetActive(true);
        }

        switch (eqItem.Position)
        {
            case EqPositionType.Ring:
                positionText.text = "戒";
                break;
            case EqPositionType.Sword:
                positionText.text = "剑";
                break;
            case EqPositionType.Armour:
                positionText.text = "甲";
                break;
            case EqPositionType.Head:
                positionText.text = "头";
                break;
            case EqPositionType.Bracer:
                positionText.text = "腕";
                break;
            case EqPositionType.Shoes:
                positionText.text = "鞋";
                break;
            case EqPositionType.Necklace:
                positionText.text = "项";
                break;
        }
            
        atkText.text = "攻击力:" + eqItem.Atk;
        nameText.text = eqItem.Name;
        icon.sprite = Resources.Load<Sprite>(eqItem.Icon);
        typeText.text = eqItem.Type;
        desText.text = "这是一个"+eqItem.Name;
        priceText.text = "单价：" + eqItem.Price;
        
        // 背景颜色
        switch (eqItem.Quality)
        {
            case 1:
                bg.color = Color.magenta;
                break;
            case 2:
                bg.color = Color.yellow;
                break;
            case 3:
                bg.color = Color.red;
                break;
            case 4:
                bg.color = Color.green;
                break;
            case 5:
                bg.color = Color.blue;
                break;
        }
    }

    // 出售
    public void OnSellClick()
    {
        sellPanel.gameObject.SetActive(true);
        sellPanel.SetValue(mydata);
    }
    
        
    // 装备
    public void OnEquippedClick()
    {
        C_To_S_Wear_Eq_Message c_data = new();
        c_data.Data = mydata;
        switch (GameManager.Ins.bagManager.flag)
        {
            case 0:
                c_data.Type = "全部";
                break;
            case 1:
                c_data.Type = "装备";
                break;
            case 2:
                c_data.Type = "药品";
                break;
        }
        NetManager.Ins.SendMessage(NetID.C_To_S_Wear_Eq_Message, c_data.ToByteArray());
        gameObject.SetActive(false);
    }

    public void OnPushClick()
    {
        C_To_S_Push_Eq_Message c_data = new();
        c_data.EqItem = eqItem;
        NetManager.Ins.SendMessage(NetID.C_To_S_Push_Eq_Message, c_data.ToByteArray());
        gameObject.SetActive(false);
    }
    

    // 替换
    public void OnReplaceClick()
    {
        C_To_S_Add_Bag_Message c_data = new();
        c_data.Item = GameManager.Ins.bagManager.eqDic[mydata.Item.Position].mydata;
        c_data.Count = 1;
        NetManager.Ins.SendMessage(NetID.C_To_S_Add_Bag_Data_Message, c_data.ToByteArray());
        
        OnEquippedClick();
    }

    private void UpdateBagPanel()
    {
        if (mydata.Item.Type != "装备")
        {
            starCon.gameObject.SetActive(false);
            positionText.gameObject.SetActive(false);
            atkText.text = "数量:" + mydata.Count;
            replaceBtn.gameObject.SetActive(false);
            equippedBtn.gameObject.SetActive(false);
        }
        else
        {
            starCon.gameObject.SetActive(true);
            positionText.gameObject.SetActive(true);
            
            // 按钮
            if (string.IsNullOrEmpty(GameManager.Ins.bagManager.eqDic[mydata.Item.Position].mydata.Name))
            {
                replaceBtn.gameObject.SetActive(false);
                equippedBtn.gameObject.SetActive(true);
            }
            else
            {
                replaceBtn.gameObject.SetActive(true);
                equippedBtn.gameObject.SetActive(false);
            }

            // 星星数量
            for (int i = starCon.childCount - 1 ; i >= 0; i--)
            {
                starCon.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < mydata.Item.Quality; i++)
            {
                starCon.GetChild(i).gameObject.SetActive(true);
            }

            switch (mydata.Item.Position)
            {
                case EqPositionType.Ring:
                    positionText.text = "戒";
                    break;
                case EqPositionType.Sword:
                    positionText.text = "剑";
                    break;
                case EqPositionType.Armour:
                    positionText.text = "甲";
                    break;
                case EqPositionType.Head:
                    positionText.text = "头";
                    break;
                case EqPositionType.Bracer:
                    positionText.text = "腕";
                    break;
                case EqPositionType.Shoes:
                    positionText.text = "鞋";
                    break;
                case EqPositionType.Necklace:
                    positionText.text = "项";
                    break;
            }
            
            atkText.text = "攻击力:" + mydata.Item.Atk;
        }
        
        sellBtn.gameObject.SetActive(true);
        pushBtn.gameObject.SetActive(false);
        nameText.text = mydata.Item.Name;
        icon.sprite = Resources.Load<Sprite>(mydata.Item.Icon);
        typeText.text = mydata.Item.Type;
        desText.text = "这是一个"+mydata.Item.Name;
        priceText.text = "单价：" + mydata.Item.Price;
        
        // 背景颜色
        switch (mydata.Item.Quality)
        {
            case 1:
                bg.color = Color.magenta;
                break;
            case 2:
                bg.color = Color.yellow;
                break;
            case 3:
                bg.color = Color.red;
                break;
            case 4:
                bg.color = Color.green;
                break;
            case 5:
                bg.color = Color.blue;
                break;
        }
    }


    private void Update()
    {
        if (!sellPanel.gameObject.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        mydata = null;
        eqItem = null;
    }
}
