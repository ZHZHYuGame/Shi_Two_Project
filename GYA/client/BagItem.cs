using System.Collections.Generic;
using Games;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItem : MonoBehaviour, IPointerDownHandler
{
    public ItemData itemdate;//通过id获取的背包整体数据
    public BagDataPB dataPB;//储存格子id  id  和数量
    public Text num;
    public Image Image, icon, suo, bk;//整个ui 图片 锁  边框
    public void Init(BagDataPB sDataBag)
    {
        Image.gameObject.SetActive(sDataBag.Id != 0);
        suo.gameObject.SetActive(sDataBag.IsSuo == 1);
        dataPB = sDataBag;
        if (sDataBag.Id == 0) return;
        itemdate = ConfigManager.Instance.GetData(sDataBag.Id);
        num.gameObject.SetActive(sDataBag.Num != 1);
        num.text = sDataBag.Num.ToString();
        icon.sprite = Resources.Load<Sprite>(itemdate.Path);
        bk.sprite = Resources.Load<Sprite>(itemdate.bk);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dataPB.Id == 0) return;//非空判断
        int en= BagPlane.instance.ZhangBei(itemdate.typeName);//true为替换 false为装备
        List<BagItemBtnTypeData> buts= ConfigManager.Instance.GetButData(itemdate,en,1);
        var tips=Instantiate(Resources.Load<TipsPlane>("UIS/TipsPlane"),GameObject.Find("Canvas").transform);
        tips.transform.position = eventData.position;
        tips.Init(dataPB,itemdate,buts,en);
    }
}