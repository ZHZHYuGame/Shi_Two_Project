using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItemUI : MonoBehaviour, IPointerDownHandler
{
    public Image icon, suo, kuang;
    public Text numText;
    public CellsData myCellsdata;
    public Good myGood;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(CellsData cellsData)
    {
        myCellsdata = cellsData;
        switch (cellsData.CellState)
        {
            case 1://没占用
                icon.gameObject.SetActive(false);
                suo.gameObject.SetActive(false);
                numText.gameObject.SetActive(false);
                kuang.gameObject.SetActive(false);
                break;
            case 2://占用
                icon.gameObject.SetActive(true);
                suo.gameObject.SetActive(true);
                kuang.gameObject.SetActive(true);
                foreach (var item in ConfigManager.GetInstance().goods)
                {
                    if (item.id == cellsData.GoodId)
                    {
                        myGood = item;
                    }
                }
                switch (myGood.quality)
                {
                    case 0:
                        kuang.sprite = Resources.Load<Sprite>("icon/else");
                        break;
                    case 1:
                        kuang.sprite = Resources.Load<Sprite>("icon/one");
                        break;
                    case 2:
                        kuang.sprite = Resources.Load<Sprite>("icon/two");
                        break;
                    case 3:
                        kuang.sprite = Resources.Load<Sprite>("icon/three");
                        break;
                    case 4:
                        kuang.sprite = Resources.Load<Sprite>("icon/four");
                        break;
                    case 5:
                        kuang.sprite = Resources.Load<Sprite>("icon/five");
                        break;
                }

                icon.sprite = Resources.Load<Sprite>(myGood.iconPath);
                numText.text = cellsData.NowNum.ToString();
                suo.gameObject.SetActive(false);
                break;
            case 3://锁
                icon.gameObject.SetActive(false);
                kuang.gameObject.SetActive(false);
                suo.gameObject.SetActive(true);
                numText.gameObject.SetActive(false);
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (myCellsdata.CellState == 3)
        {
            BagUI.Instance.addCellTips.gameObject.SetActive(true);
        }
        if (myCellsdata.CellState == 2)
        {
            if (myGood == null) return;
            if (myGood.btnType == 1)
            {
                Game.Instance.eqTipsPanel.gameObject.SetActive(true);
                EqTipsUI.Instance.Init(myCellsdata);
            }


        }
    }
}
