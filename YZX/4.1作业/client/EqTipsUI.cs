using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Games;
public class EqTipsUI : MonoBehaviour
{
    public static EqTipsUI Instance;
    void Awake()
    {
        Instance = this;
    }
    public Transform parent;
    public Image tou, icon;
    public Text nameText, desText, saleText;

    public Dictionary<string, Button> btnDic = new Dictionary<string, Button>();

    public void Init(CellsData cellsData)
    {
        Good good = ConfigManager.GetInstance().ByIdGetGood(cellsData.GoodId);
        switch (good.quality)
        {
            case 1:
                tou.color = new Color(255 / 255, 255 / 255, 255 / 255, 1);
                break;
            case 2:
                tou.color = new Color(56 / 255, 215 / 255, 41 / 255, 1);
                break;
            case 3:
                tou.color = new Color(66 / 255, 150 / 255, 254 / 255, 1);
                break;
            case 4:
                tou.color = new Color(224 / 255, 106 / 255, 228 / 255, 1);
                break;
            case 5:
                tou.color = new Color(242 / 255, 151 / 255, 78 / 255, 1);
                break;
        }
        icon.sprite = Resources.Load<Sprite>(good.iconPath);
        nameText.text = good.name;
        desText.text = good.msg + good.name;
        saleText.text = good.sell.ToString();
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        foreach (var item in ConfigManager.GetInstance().fuctionBtns)
        {
            if (item.type != 2)
            {
                Button button = GameObject.Instantiate(Resources.Load<GameObject>("TipsBtn"), parent).GetComponent<Button>();
                button.transform.GetChild(0).GetComponent<Text>().text = item.btnName;
                btnDic.Add(item.btnName, button);

                switch (item.btnName)
                {
                    case "出售":
                        break;
                    case "装备":

                        break;
                    case "替换":
                        break;
                    case "关闭":
                        button.onClick.AddListener(() =>
                        {
                            transform.gameObject.SetActive(false);
                        });
                        break;
                }
            }
        }
        if (BagUI.Instance.roleEqDic[good.quality].myCellsData == null)
        {
            btnDic["替换"].gameObject.SetActive(false);
            btnDic["装备"].gameObject.SetActive(true);
        }
        else
        {
            btnDic["替换"].gameObject.SetActive(true);
            btnDic["装备"].gameObject.SetActive(false);
        }


    }
}
