using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class AddCellTipsManager:Singlton<AddCellTipsManager>
{


    public void Start()
    {
        MessageControll.GetInstance().AddListener(NetID.C_T_S_BuyAddCellJuan_Message, C_T_S_BuyAddCellJuan_Message_Handle);
        MessageControll.GetInstance().AddListener(NetID.C_T_S_AddBagCell_Message, C_T_S_AddBagCell_Message_Handle);
    }
    /// <summary>
    /// 服务器客户端发送的扩容背包格子请求
    /// </summary>
    /// <param name="obj"></param>
    private void C_T_S_AddBagCell_Message_Handle(object obj)
    {
        object[] objList=obj as object[];
        byte[] data = objList[0] as byte[];
        Client c = objList[1] as Client;
        C_T_S_AddBagCell_Message c_data = C_T_S_AddBagCell_Message.Parser.ParseFrom(data);

        S_T_C_AddBagCell_Message s_data = new S_T_C_AddBagCell_Message();
        //可以加个判断扩容劵是否足够，应该不用，界面上如果不够的话只显示购买按钮
        UserData userData = c.userData;
        //userData.krJuanNum--;
        userData.bagCellNum += 5;//开的格子+5
        userData.addCellNum += 5;//累计解锁的格子+5
        userData.jieSuoCellNum -= 5;//可解锁的格子-5
        userData.krJuanNum -= userData.delJuanNum;//减少消耗的扩容卷
        if(userData.addCellNum%10==0)
        {
            userData.delJuanNum++;//如果累计够10的倍数，消耗的扩容卷数量+1
        }
        s_data.KrJuanNum = userData.krJuanNum;
        s_data.DelJuanNum= userData.delJuanNum;
        NetManager.GetInstance().SendMessage(c.st, NetID.S_T_C_AddBagCell_Message, s_data.ToByteArray());
        //服务器向客户端发送背包格子更新结果
        S_2_C_Bag_Type_Message s_Data = new S_2_C_Bag_Type_Message();
        List<CellData> cellDatas = ConfigManager.GetInstance().GetTypeCellData(GoodsType.All);
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
        s_Data.GoodsType = GoodsType.All;
        NetManager.GetInstance().SendMessage(c.st, NetID.S_T_C_Bag_Type_Message, s_Data.ToByteArray());
    }

    /// <summary>
    /// 服务器处理客户端发送的购买扩充卷请求
    /// </summary>
    /// <param name="obj"></param>
    private void C_T_S_BuyAddCellJuan_Message_Handle(object obj)
    {
        object[] objList=obj as object[];
        byte[] data = objList[0] as byte[];
        Client c=objList[1] as Client;
        C_T_S_BuyAddCellJuan_Message c_data=C_T_S_BuyAddCellJuan_Message.Parser.ParseFrom(data);
        UserData userData = UserManager.GetInstance().allClict[c_data.Port].userData;
        //可以加一个判断钻石是否足够，返回加一个是否成功的结果

        userData.zsNum -= 20;
        userData.krJuanNum += 1;
        S_T_C_BuyAddCellJuan_Message s_data = new S_T_C_BuyAddCellJuan_Message();
        s_data.ZsNum = userData.zsNum;
        s_data.KrJuanNum = userData.krJuanNum;
        NetManager.GetInstance().SendMessage(c.st, NetID.S_T_C_BuyAddCellJuan_Message, s_data.ToByteArray());
    }
}
