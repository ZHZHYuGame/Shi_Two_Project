using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Message;
using UnityEngine;
using UnityEngine.UI;

public class EquipItem : MonoBehaviour
{
    public int type;
    public BagItem myBagItem=null;
    public Image icon;
    public Text level;
    public ItemData myData = null;
    // public ItemData myData;
    public void Init(BagItem Item)
    {
        if(Item==null) return;
        myBagItem = Item;
        ItemData data = ConfigManger.GetInstance.GetItemData(Item.ItemId);
        myData = data;
        icon.sprite=Resources.Load<Sprite>("icon/"+data.Icon);
        level.text=data.Quality.ToString()+"+";
    }
    public void Refresh(BagItem Item)
    {
        if (Item == null)
        {
            myBagItem = null;
            myData = null;
            icon.sprite=Resources.Load<Sprite>("bk/0");
            level.text = "";
        }
        else
        {
            myBagItem = Item;
            ItemData data = ConfigManger.GetInstance.GetItemData(Item.ItemId);
            myData = data;
            icon.sprite=Resources.Load<Sprite>("icon/"+data.Icon);
            level.text=data.Quality.ToString()+"+";
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        icon.sprite=Resources.Load<Sprite>("bk/0");
        level.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
