using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBag
{
    public class UserManager:Singleton<UserManager>
    {

        public Client UserData;
        Random Random = new Random();
        public void Start()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_InitizlizeMessage, Initialize);
        }
         void Initialize(object obj)
        {
          
            object[] t = obj as object[];
            byte[] data = t[0] as byte[];
            Client c = t[1] as Client;
            var idGen = new SnowflakeIdGenerator(1);
            UserData.user.bagList = new List<BagGoods>();
            S_2_C_InitialiazeBag msg= new S_2_C_InitialiazeBag();
            msg.Gold = UserData.user.gold;
            msg.Masonry = UserData.user.masonry;
            msg.Roll = UserData.user.roll;
            msg.Energy = UserData.user.energy;
            msg.MaxBagCount = UserData.user.MaxBagCount;

            //第一次赋值背包格子数据
             UserData.user.bagCellList = Rearrange();
            //读json获取背包数据
            for (int i = 0; i < ConfigManager.GetInstance().bagItem.Count; i++)
            {
                var id = idGen.NextId().ToString();
               UserData.user.bagList.Add(new BagGoods()
                {
                    SnowID = id,
                    ItemData = ConfigManager.GetInstance().bagItem[i],
                    num = Random.Next(1,5),
                });
            }
            //将背包数据赋值给格子
            InitializeCellToBag(UserData.user.bagList);
            for (int i = 0; i <UserData.user.bagCellList.Count; i++)
            {
                msg.CellList.Add(new BagCellDate()
                {
                    SnowID =UserData.user.bagCellList[i].bagGood.SnowID,
                    ItemId = UserData.user.bagCellList[i].bagGood.ItemData.ID,
                    Num = UserData.user.bagCellList[i].bagGood.num,
                    CellId =UserData.user.bagCellList[i].cellID,
                    State = (BagCellState)UserData.user.bagCellList[i].state,
                    IsUser =UserData.user.bagCellList[i].bagGood.isUser
                });
            }

            msg.NowBagCount = UserData.user.bagList.Count;
            NetManager.GetInstance().SendNetMessage(c.st, NetID.S_To_C_InitizlizeMessage, msg.ToByteArray());
        }
        //第一次赋值背包格子数据
        public List<BagCellData> Rearrange()
        {
            var list = new List<BagCellData>();
            for (int i = 0; i < UserData.user.EndBagCount; i++)
            {
                var newData = new BagCellData()
                {
                    cellID = i,
                    state = BagCellStart.locks,
                    bagGood = new BagGoods()
                    {
                        SnowID = "",
                        ItemData = new ItemData() { },
                        num = 0,
                        isUser = false,
                    }
                };
                if (i < UserData.user.MaxBagCount)
                {
                    newData.state = BagCellStart.unUser;
                }
                list.Add(newData);
            }
            return list;
        }
        //将背包数据赋值给格子
        public void InitializeCellToBag(List<BagGoods> list)
        {
             for (int i = 0; i <list.Count; i++)
            {
               UserData.user.bagCellList[i].state = BagCellStart.user;
               UserData.user.bagCellList[i].bagGood = list[i];
            }
        }
    }







}
