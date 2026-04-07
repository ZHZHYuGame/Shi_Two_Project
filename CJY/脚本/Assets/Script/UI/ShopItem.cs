using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Text sale;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInit(ShopData shop)
    {
        if (shop==null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            icon.sprite = Resources.Load<Sprite>("icon/" + shop.Icon);
            sale.text = shop.Sale + "";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
