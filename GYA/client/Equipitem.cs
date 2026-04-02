using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Equipitem : MonoBehaviour,IPointerDownHandler
{
    public Text names,sifunum;
    public Image icon,bk,Image;
    public ItemData itemdate;//通过id获取的背包整体数据
    public EquipTypePB dataPB;//储存格子id  id  和数量
    public void Init(EquipTypePB equipTypePb)
    {
        dataPB=equipTypePb;
        names.text=equipTypePb.Name;
        transform.localPosition=new Vector3(equipTypePb.X,equipTypePb.Y,0);
        Image.gameObject.SetActive(equipTypePb.Id!=0);
        if (equipTypePb.Id==0)return;
        sifunum.gameObject.SetActive(equipTypePb.Sifynum!=1);
        sifunum.text=equipTypePb.Sifynum.ToString();
        itemdate = ConfigManager.Instance.GetData(equipTypePb.Id);
        icon.sprite = Resources.Load<Sprite>(itemdate.Path);
        bk.sprite=Resources.Load<Sprite>(itemdate.bk);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dataPB.Id == 0) return;//非空判断
        int en= BagPlane.instance.KongData();//true为替换 false为装备
        List<BagItemBtnTypeData> buts= ConfigManager.Instance.GetButData(itemdate,en,2);
        var tips=Instantiate(Resources.Load<TipsPlane>("UIS/TipsPlane"),GameObject.Find("Canvas").transform);
        tips.transform.position = eventData.position;
        tips.Init(dataPB,itemdate,buts,en);
    }
}
