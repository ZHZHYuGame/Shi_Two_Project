using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
using UnityEngine.UI;

public class BagCell : MonoBehaviour
{
    public Image icon;
    public Text text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData(BagBaseData item)
    {
        if (item.IsExist)
        {
            Item temp = GetItem(item.ItemID);
            icon.sprite = Resources.Load<Sprite>(temp.path);
            text.text = item.ItemNum.ToString();
        }
        else
        {
            text.enabled = false;
        }
    }
    public Item GetItem(int ID)
    {
        foreach (var item in ConfigManager.Instance().Itemlist)
        {
            if (item.id == ID)
            {
                return item;
            }
        }
        return null;
    }
}
