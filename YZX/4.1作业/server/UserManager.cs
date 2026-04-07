using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserManager:Singlton<UserManager>
{

    
    public Dictionary<int, Client> allClict = new Dictionary<int, Client>();
    public Dictionary<int, UserData> allUserData=new Dictionary<int, UserData>();  
    public void Start()
    {

    }

    internal void AddClient(Client c)
    {
        if (!allClict.ContainsKey(c.port))
        {
            allClict.Add(c.port, c);
            c.userData.UGUI = c.port;
            c.userData.cellDatas = new List<CellData>();
            c.userData.bagCellNum = 20;
            c.userData.jieSuoCellNum = 15;
            c.userData.maxCellNum = c.userData.bagCellNum + c.userData.jieSuoCellNum;
            for (int i=0;i<ConfigManager.GetInstance().goods.Count;i++)
            {
                CellData cellData = new CellData();
                cellData.gridId = i;
                cellData.goodId = ConfigManager.GetInstance().goods[i].id;
                cellData.nowNum = 1;
                c.userData.cellDatas.Add(cellData);
            }
            c.userData.krJuanNum = 0;
            c.userData.delJuanNum = 1;
            c.userData.coinNum = 1000;
            c.userData.zsNum = 1000;
            S_2_C_User_Message s_Data = new S_2_C_User_Message();
            s_Data.Port = c.port;
            s_Data.KrJuanNum=c.userData.krJuanNum;
            s_Data.DelJuanNum = c.userData.delJuanNum;
            s_Data.CoinNum = c.userData.coinNum;
            s_Data.ZsNum= c.userData.zsNum;

            NetManager.GetInstance().SendMessage(c.st, NetID.S_T_C_User_Message, s_Data.ToByteArray());
        }
    }
}

public class UserData
{
    public int UGUI;                    //端口号
    public List<CellData> cellDatas;    //该客户端拥有的物品表
    public int bagCellNum;              //背包打开格子数量20
    public int jieSuoCellNum;           //可解锁格子数量15
    public int maxCellNum;              //全部格子数量
    public int krJuanNum;               //拥有扩容卷数量
    public int delJuanNum;              //需要消耗的扩容卷数量
    public int coinNum;                 //拥有金币数量
    public int zsNum;                   //拥有钻石数量
    public int addCellNum;              //累计已经解锁的格子数量

}
