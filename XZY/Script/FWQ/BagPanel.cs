using System.Collections;
using System.Collections.Generic;
using Games;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : MonoBehaviour
{
    public List<BagBaseData> list = new List<BagBaseData>();
    public Transform CellBase;
    void Start()
    {
        MessageControll.Instance().AddListener(NetID.S_2_CSGetBag,GetData);
    }

    private void GetData(object obj)
    {
        list.Clear();
        S_To_C_Bag Temp = S_To_C_Bag.Parser.ParseFrom((obj as object[])[0] as byte[]);
        print(Temp.BagData.Count);
        foreach (var item in Temp.BagData)
        {
            BagBaseData Data = new BagBaseData();
            Data.IsExist = item.IsExist;
            Data.ItemID = item.ItemID;
            Data.ItemNum = item.ItemNum;
            list.Add(Data);
        }
        UISX();
    }

    private void UISX()
    {
        for (int i = 0; i < CellBase.childCount; i++)
        {
            Destroy(CellBase.GetChild(i).gameObject);
        }

        foreach (var item in list)
        {
            BagCell temp = Instantiate(Resources.Load<BagCell>("BagCell"), CellBase);
            temp.GetData(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
