using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagCell : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private Image star;
    [SerializeField] private Transform con;
    [SerializeField] private Image icon, bg;
    [SerializeField] private Text countText;


    public BagData myData;
    
    public void SetValue(BagData bag)
    {
        myData = bag;
        UpdateCell();
    }

    void UpdateCell()
    {
        if (myData.Item == null)
        {
            icon.gameObject.SetActive(false);
            bg.color = Color.gray;
            countText.gameObject.SetActive(false);
            con.gameObject.SetActive(false);
        }
        else
        {
            
            if (myData.Item.Type == "装备")
            {
                con.gameObject.SetActive(true);
                countText.gameObject.SetActive(false);
                for (int i = con.childCount - 1; i >= 0; i--)
                {
                    // Destroy(con.transform.GetChild(i).gameObject);
                    con.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < myData.Item.Quality; i++)
                {
                    // Instantiate(star, con);
                    con.GetChild(i).gameObject.SetActive(true);
                }
            }
            else
            {
                con.gameObject.SetActive(false);
                countText.gameObject.SetActive(true);
                countText.text = myData.Count.ToString();
            }
            icon.gameObject.SetActive(true);
            icon.sprite = Resources.Load<Sprite>(myData.Item.Icon);
            switch (myData.Item.Quality)
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (myData.Item == null) return;
        
        GameManager.Ins.bagManager.tips.gameObject.SetActive(true);
        GameManager.Ins.bagManager.tips.SetData(myData);
    }
    
}
