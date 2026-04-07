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
        public void AddBag(Client C, List<Item> list,out bool Iss)
        {
            bool Is= IsAddBag(C, list);
            if (!Is)
            {
                Iss = false;
                return;
            }
            foreach (var item in list)
            {
                if(item.type==0)
                {
                    foreach (var item2 in Dic[C])
                    {
                        if(item2.Item==null)
                        {
                            continue;
                        }
                        if (item2.Item.id == item.id&&item2.Count+1<=MaxNum)
                        {
                            item2.Count++;
                            Iss = true;
                            goto a;
                        }
                    }
                    foreach (var item2 in Dic[C])
                    {
                        if (item2.Item == null)
                        {
                            item2.Item = item;
                            item2.Count = 1;
                            break;
                        }
                    }
                a:;
                   }
                    else
                    {
                        foreach(var item2 in Dic[C])
                         {
                            if (item2.Item == null)
                            {
                               item2.Item= item;
                               item2.Count = 1;
                            break;
                            }
                         }
                    }
            }
            Iss = true;
            S_C(C);

        }
        //判断背包是否能够添加这些数据
        public bool IsAddBag(Client C, List<Item> list)
        {
            int count = 0;
            foreach (var item in Dic[C])
            {
                if (item.Item == null)
                {
                    count++;
                }
            }
            if (count < list.Count)
            {
                //存放所有能叠加的消耗品格子
                List<BagCellData> templist =new List<BagCellData>();
                foreach(var item in Dic[C])
                {
                    if(item.Item==null)
                    {
                        continue;
                    }
                    if(item.Item.type==0&&item.Count<MaxNum)
                    {
                        templist.Add(item);
                    }
                }
                List<Item> templist2 = new List<Item>();
                int Count = 0;
                foreach (var item in list)
                {
                    if (item.type == 0)
                    {
                        bool Is=false;
                        foreach(var item2 in templist2)
                        {
                            if(item2.id==item.id)
                            {
                                item2.count++;
                                Is = true;
                                Count++;
                                break;
                            }
                        }
                        if(!Is)
                        {
                            item.count = 1;
                            templist2.Add(item);    
                        }
                    }
                }
               
                foreach(var item in templist)
                {
                    foreach(var item2 in templist2)
                    {
                        if(item.Item.id==item2.id&&item.Count+item2.count<=MaxNum)
                        {
                            Count++;
                            break;
                        }
                    }
                }
                if (count + Count < list.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
               
            }
            else
            {
                return true;
            }
        }
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
