using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BagManager:Singlton<BagManager>
{

    public void Start()
    {
        MessageControll.GetInstance().AddListener(NetID.C_T_S_Bag_Type_Message, C_T_S_Bag_Type_Message_Handle);
    }

    /// <summary>
    /// 服务器处理客户端发送的显示类型
    /// </summary>
    /// <param name="obj"></param>
    private void C_T_S_Bag_Type_Message_Handle(object obj)
    {
        object[] objList= obj as object[];
        byte[] data=objList[0] as byte[];
        Client c=objList[1] as Client;
        C_2_S_Bag_Type_Message c_data = C_2_S_Bag_Type_Message.Parser.ParseFrom(data);
        UserData userData = c.userData;
        
        S_2_C_Bag_Type_Message s_Data = new S_2_C_Bag_Type_Message();
        
        
        List<CellData> cellDatas = ConfigManager.GetInstance().GetTypeCellData(c_data.GoodsType);
        
        cellDatas.Sort(SortCellDatas);
        List<CellsData> cellsDatas = new List<CellsData>();
        foreach (var item in cellDatas)
        {
                CellsData cellsData = new CellsData();
                cellsData.GridId = item.gridId;
                cellsData.GoodId = item.goodId;
                cellsData.NowNum = item.nowNum;
                cellsData.CellState = 2;
                s_Data.CellsData.Add(cellsData);
        }
        
        s_Data.BagCellNum = userData.bagCellNum;
        s_Data.JieSuoCellNum = userData.jieSuoCellNum;
        s_Data.MaxCellNum = userData.maxCellNum;
        s_Data.GoodsType = c_data.GoodsType;
        NetManager.GetInstance().SendMessage(c.st, NetID.S_T_C_Bag_Type_Message, s_Data.ToByteArray());

    }

    private int SortCellDatas(CellData x, CellData y)
    {
        var goodDict = ConfigManager.GetInstance().goods.ToDictionary(g => g.id);

        goodDict.TryGetValue(x.goodId, out Good gooda);
        goodDict.TryGetValue(y.goodId, out Good goodb);
        //按照quality降序排列
        int qualityCompare = goodb.quality.CompareTo(gooda.quality);
        if (qualityCompare != 0) return qualityCompare;
        return goodb.wearType.CompareTo(gooda.wearType);


    }
}
