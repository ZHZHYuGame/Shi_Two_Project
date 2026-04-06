using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ShiSan_Server
{
     class Program
    {
        static void Main(string[] args)
        {
            NetManager net = new NetManager();
            //启动网络服务
            net.Start();

            ConfigManager.GetInstance().Start();

            UserManager.GetInstance().Start();

            ChatManager.GetInstance().Start();
            //商城管理器启动
            ShopManager.GetInstance().Start();

            BagManager.GetInstance().Start();

            Console.ReadKey();
        }
    }
}
