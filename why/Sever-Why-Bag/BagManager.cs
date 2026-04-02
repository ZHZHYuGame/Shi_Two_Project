using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace NetBag
{
    public class BagManager : Singleton<BagManager>
    {
       
        public void Start()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_GetBagTypeMessage, GetBagTypeHandle_);
        }
        private void GetBagTypeHandle_(object obj)
        {
            object[] t = obj as object[];
            byte[] data = t[0] as byte[];
            Client c = t[1] as Client;
            C_2_S_GetBagType c_Data = C_2_S_GetBagType.Parser.ParseFrom(data);
            UserManager.GetInstance().UserData.user.bagCellList = UserManager.GetInstance().Rearrange();
            var type = c_Data.Type;
            var baglist=GetType(type);
            UserManager.GetInstance().InitializeCellToBag(baglist);
            S_2_C_InitialiazeBag msg = new S_2_C_InitialiazeBag();
            msg.NowBagCount = baglist.Count;
            msg.MaxBagCount = UserManager.GetInstance().UserData.user.MaxBagCount;
            for (int i = 0; i < UserManager.GetInstance().UserData.user.bagCellList.Count; i++)
            {
                msg.CellList.Add(new BagCellDate()
                {
                    SnowID = UserManager.GetInstance().UserData.user.bagCellList[i].bagGood.SnowID,
                    ItemId = UserManager.GetInstance().UserData.user.bagCellList[i].bagGood.ItemData.ID,
                    Num = UserManager.GetInstance().UserData.user.bagCellList[i].bagGood.num,
                    CellId = UserManager.GetInstance().UserData.user.bagCellList[i].cellID,
                    State = (BagCellState)UserManager.GetInstance().UserData.user.bagCellList[i].state,
                    IsUser = UserManager.GetInstance().UserData.user.bagCellList[i].bagGood.isUser
                });
            }
            NetManager.GetInstance().SendNetMessage(c.st, NetID.S_To_C_GetBagTypeMessage, msg.ToByteArray());
        }


        /// <summary>
        /// 获取同类型数组
        /// </summary>
        /// <param name="type"></param>
        public List<BagGoods> GetType(int type)
        {
            var list = UserManager.GetInstance().UserData.user.bagList;
            var newlist = new List<BagGoods>();
            if (type == 0)
            {
                newlist = list;
            }
            else
            {
                for (global::System.Int32 i = 0; i < list.Count; i++)
                {
                    if (list[i].ItemData.PackTab == type)
                    {
                        newlist.Add(list[i]);
                    }
                }
            }
            //排序
            newlist.Sort(new SortItem());
            //整理
            return ClearList(newlist);          
        }
        public List<BagGoods> ClearList(List<BagGoods> list)
        {
            var idGen = new SnowflakeIdGenerator(1);
            var clearList = new List<BagGoods>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ItemData.MaxPile==1)
                {
                    for (global::System.Int32 j = 0; j < list[i].num; j++)
                    {
                        var id = idGen.NextId().ToString();
                        clearList.Add(new BagGoods() {
                            SnowID = id,
                            ItemData = list[i].ItemData,
                            num=1,
                            isUser=false,
                         });
                    }
                }
                else
                {
                    for (global::System.Int32 j = 0; j < list[i].num / list[i].ItemData.MaxPile; j++)
                    {
                        clearList.Add(new BagGoods()
                        {
                            SnowID = list[i].SnowID,
                            ItemData = list[i].ItemData,
                            num = list[i].ItemData.MaxPile,
                            isUser = false,
                        });
                    }
                    clearList.Add(new BagGoods()
                    {
                        SnowID = list[i].SnowID,
                        ItemData = list[i].ItemData,
                        num = list[i].num % list[i].ItemData.MaxPile,
                        isUser = false,
                    });
                }
            }
            return clearList;
        }

        /// <summary>
        /// 按照三个要求的排序方法
        /// </summary>
        public class SortItem : IComparer<BagGoods>
        {
            public int Compare(BagGoods x, BagGoods y)
            {
                int typePriority = y.ItemData.Priority.CompareTo(x.ItemData.Priority);
                if (typePriority != 0) return typePriority;
                int typeLevel = y.ItemData.MinLevel.CompareTo(x.ItemData.MinLevel);
                if (typeLevel != 0) return typeLevel;
                int typeID = y.ItemData.ID.CompareTo(x.ItemData.ID);
                return typeID;

            }
        }


    }
}
