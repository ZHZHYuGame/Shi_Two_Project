using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Message;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

public class EquipItem : MonoBehaviour,IPointerDownHandler
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
    public void OnPointerDown(PointerEventData eventData)
    {
        // if (isLocked || myItem == null) return;
        ItemData itemData = ConfigManger.GetInstance.GetItemData(myBagItem.ItemId);
        // 弹出Tips，传递当前道具以及是否已穿戴（这里需要外部获取，可以从 BagUI 查询）
        bool isEquipped = BagUI.instance.IsItemEquipped(myBagItem.Uid); // 需要在 BagUI 中实现
        GameObject go = Instantiate(Resources.Load<GameObject>("Tips"), BagUI.instance.transform);
        go.transform.position = Input.mousePosition;
        go.GetComponent<TipsMgr>().Init(myBagItem, isEquipped);
    }
}
