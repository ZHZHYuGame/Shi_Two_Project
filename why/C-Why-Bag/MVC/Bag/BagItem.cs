using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    public ItemData myData;

    public Image bg, icon, suo;

    public Text num;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ReFresh(ItemData data, BagCellDate cellData)
    {
        myData = data;
        switch (cellData.State)
        {
            case BagCellState.User:
                bg.color = GetMassColor(data.Mass);
                icon.gameObject.SetActive(true);
                icon.sprite=Resources.Load<Sprite>(data.Path);
                suo.gameObject.SetActive(false);
                if (data.MaxPile != 1)
                {
                    num.gameObject.SetActive(true);
                    num.text = cellData.Num.ToString();
                }
                else
                {
                    num.gameObject.SetActive(false);
                }
                
                break;
            case BagCellState.UnUser:
                bg.color=new Color(1,1,1,1);
                icon.gameObject.SetActive(false);
                num.text = "";
                suo.gameObject.SetActive(false);
                break;
            case BagCellState.Lock:
                bg.color=new Color(1,1,1,1);
                icon.gameObject.SetActive(false);
                num.text = "";
                suo.gameObject.SetActive(true);
                break;
        }
    }

    public Color GetMassColor(int mass)
    {
        if (mass==1)
        {
            return new Color((float)255/255,(float)255/255,(float)255/255,1);
        }
        else if (mass==2)
        {
            return new Color((float)56/255,(float)215/255,(float)41/255,1);
        }
        else if (mass==3)
        {
            return new Color((float)66/255,(float)150/255,(float)254/255,1);
        }
        else if (mass==4)
        {
            return new Color((float)224/255,(float)106/255,(float)228/255,1);
        }else if (mass==5)
        {
            return new Color((float)242/255,(float)151/255,(float)78/255,1);
        }
        return new Color(1,1,1,1);
    }
}
