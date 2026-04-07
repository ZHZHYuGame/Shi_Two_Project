using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
    public Image icon;
    public Text text;
    public GameObject Game;
    public Button Btn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData(int items, bool Is)
    {
        Item item = GetItem(items);
        icon.sprite = Resources.Load<Sprite>(item.path);
        text.text = "1";
        if (!Is)
        {
            Game.SetActive(true);
        }
        Btn.onClick.AddListener(() =>
        {
            Tips.instance.GetData(item);
        });
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
