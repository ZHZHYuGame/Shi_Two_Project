using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBag
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NetManager.GetInstance().Start();
            ConfigManager.GetInstance().Start();
            UserManager.GetInstance().Start();
            BagManager.GetInstance().Start();
            Console.ReadKey();
        }
    }
}
