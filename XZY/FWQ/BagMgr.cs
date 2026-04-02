using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWQ
{
    internal class BagMgr
    {
        public Dictionary<Client, List<BagCellData>> Dic = new Dictionary<Client, List<BagCellData>>();
        public static BagMgr instance;
        public int MaxNum=5;//可叠加物品最大叠加数量
        public static BagMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BagMgr();
                }
                return instance;
            }
        }
        public BagMgr()
        {
           // MessageControll.Instance().AddListener(NetID.S_2_COpenMail, OpenMail);
        }
        /// <summary>
        /// 背包初始化
        /// </summary>
        /// <param name="C"></param>
        public void Start(Client C)
        {
            Dic.Add(C,new List<BagCellData>());
            for (int i = 0; i < 2; i++)
            {
                Dic[C].Add(new BagCellData() { Count = 0,Item=null});
            }
            S_C(C);
        }

        //背包添加数据
        public void AddBag(Client C, List<Item> list)
        {


        }
        //判断背包是否能够添加这些数据
        /*public bool IsAddBag(Client C, List<Item> list)
        {
            int count = 0;
            foreach (var item in Dic[C])
            {
                if(item.Item==null)
                {
                    count++;
                }
            }
            if (count<list.Count)
            {
                List<BagCellData> templist = Dic[C];
                int DJCount = 0;
                foreach (var item in templist)
                {
                    if (item.Item==null||item.Item.type == 1)
                    {
                        continue;
                    }
                    foreach (var items in list)
                    {
                        if(items.id==item.Item.id&&item.Count<=MaxNum)
                        {
                            item.Count++;
                            DJCount++;
                        }
                    }
                }
            }
            else
            {
                return true;
            }
        }*/
        //发送客户端方法
        public void S_C(Client C)
        {
            S_To_C_Bag Bag=new S_To_C_Bag();
            foreach (var item in Dic[C])
            {
                BagBaseData Data = new BagBaseData();
                if (item.Count > 0)
                {
                    Data.IsExist = true;
                    Data.ItemID=item.Item.id;
                    Data.ItemNum=item.Count;
                }
                else
                {
                    Data.IsExist = false;
                }
                Bag.BagData.Add(Data);
            }
           FWQManager.Instance().SendNetMessage(C.st,NetID.S_2_CSGetBag,Bag.ToByteArray());
        }
    }
}
