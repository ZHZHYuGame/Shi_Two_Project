using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Program
{
    static void Main(string[] args)
    {
        NetManager.Getinstance().Start();
        UserManager.Getinstance().Start();
        ConfigManager.Getinstance().Start();
        BagManager.Getinstance().Start();
        Console.ReadKey();
    }
}
