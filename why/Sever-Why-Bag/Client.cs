using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetBag
{
    public class Client
    {
        public Socket st;
        public string ip = "";
        public int port;
        public UserData user;
        
    }
    public class UserData
    {
        public int gold;
        public int masonry;
        public int roll;
        public int energy;
        public int MaxBagCount;
        public int NowBagCount;
        public int EndBagCount;
        public List<BagGoods> bagList=new List<BagGoods>();
        public List<BagCellData> bagCellList=new List<BagCellData>();
    }
    public class BagGoods
    {
        public string SnowID;
        public ItemData ItemData;
        public int num;
        public bool isUser;
    }
    public class BagCellData
    {
        public int cellID;
        public BagCellStart state;
        public BagGoods bagGood;
    }
    public enum BagCellStart
    {
        user =0,
        unUser =1,
        locks = 2,
    }
}
