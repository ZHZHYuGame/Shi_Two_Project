// See https://aka.ms/new-console-template for more information

using BagNet;
using BagNet.Net;

internal class Program
{
    public static void Main(string[] args)
    {
       NetManger.GetInstance.Start();
       ConfigManger.GetInstance.Start();
       BagManger.GetInstance.Start();
       // var idGen = new SnowflakeIdGenerator(1); // 机器ID固定为1
       // for (int i = 0; i < 10; i++)
       // {
       //     long uid = idGen.NextId();
       //     string uidStr = uid.ToString(); // 作为string传给客户端
       //     Console.WriteLine(uidStr);
       // }
       
       Console.ReadKey();
    }
}