using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class EqCell : MonoBehaviour
{
    public EqPositionType flag;
    public Item mydata;

    [SerializeField] private Image img, bg;

    public void SetValue(Item item)
    {
        mydata = item;
        if (!string.IsNullOrEmpty(item.Icon))
        {
            img.gameObject.SetActive(true);
            img.sprite = Resources.Load<Sprite>(item.Icon);
            switch (item.Quality)
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
        else
        {
            bg.color = Color.gray;
            img.gameObject.SetActive(false);
        }
    }
    
}
