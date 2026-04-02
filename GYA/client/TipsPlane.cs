using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class TipsPlane:MonoBehaviour
{
    public Image bk, icon;
    public Text names, des, sift,pricetext;
    public Transform content;

    public void Init(BagDataPB dataPb, ItemData itemdate, List<BagItemBtnTypeData> buts, int gridID)
    {
        NewMethod(dataPb.Sifynum, itemdate);
        for (int i = 0; i < buts.Count; i++)
        {
            var but = Instantiate(Resources.Load<Button>("UIS/Tipsbut"), content);
            but.transform.Find("name").GetComponent<Text>().text = buts[i].name;
            int c = i;
            but.onClick.AddListener(() =>
            {
                switch (buts[c].name)
                {
                    case "穿戴":
                    case "替换":
                        C_To_S_BagApp_Message c_Data = new C_To_S_BagApp_Message();
                        c_Data.DataGridID = dataPb.GridID;
                        c_Data.EquipGrigID = gridID;
                        NetManager.Instance.SendNetMessage(NetID.C_To_S_BagApp_Message, c_Data.ToByteArray());
                        break;
                }
                Destroy(gameObject);
            });
        }
    }

    public void Init(EquipTypePB dataPb, ItemData itemdate, List<BagItemBtnTypeData> buts, int gridID)
    {
        NewMethod(dataPb.Sifynum, itemdate);
        for (int i = 0; i < buts.Count; i++)
        {
            var but = Instantiate(Resources.Load<Button>("UIS/Tipsbut"), content);
            but.transform.Find("name").GetComponent<Text>().text = buts[i].name;
            int c = i;
            but.onClick.AddListener(() =>
            {
                switch (buts[c].name)
                {
                    case "卸下":
                        C_To_S_BagApp_Message c_Data = new C_To_S_BagApp_Message();
                        c_Data.DataGridID = gridID;
                        c_Data.EquipGrigID = dataPb.GridID;
                        Debug.Log(c_Data.DataGridID+"."+c_Data.EquipGrigID);
                        NetManager.Instance.SendNetMessage(NetID.C_To_S_BagApp_Message, c_Data.ToByteArray());
                        break;
                    case "替换":
                        break;
                }

                Destroy(gameObject);
            });
        }
    }

    private void NewMethod(int sify, ItemData itemdate)
    {
        icon.sprite = Resources.Load<Sprite>(itemdate.Path);
        bk.sprite = Resources.Load<Sprite>(itemdate.bk);
        names.text = itemdate.Name;
        des.text = itemdate.Msg;
        sift.text = sify == 0
            ? itemdate.Type.ToString()
            : $"精炼 {sify} 阶";

        pricetext.text = itemdate.Sell == 0
            ? "不可售"
            : $"出售单价 {itemdate.Sell}";
    }
}