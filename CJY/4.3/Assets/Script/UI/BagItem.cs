using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItem : MonoBehaviour,IPointerDownHandler,IPointerExitHandler
{
    [SerializeField] private Image icon;
    public int id;
    public int num = 0;
    [SerializeField] private Text numText;
    public BagData data;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInit(int index, BagData bag)
    {
        id = index;
        data = bag;
        if (bag != null)
        {
            print(bag.Shop.Name);
            num = 1;
            numText.gameObject.SetActive(true);
            numText.text = num + "";
            icon.gameObject.SetActive(true);
            icon.sprite = Resources.Load<Sprite>("icon/" + bag.Shop.Icon);
        }
        else
        {
            icon.gameObject.SetActive(false);
            numText.gameObject.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BagUI.ins.tips.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (data != null)
        {
            BagUI.ins.tips.gameObject.SetActive(true);
            BagUI.ins.OpenTips(data);
            BagUI.ins.tips.transform.position=eventData.position;
        }
    }
}
