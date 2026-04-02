namespace S;

class Program
{
    static void Main(string[] args)
    {
        NetManager.Ins.Start();
        BagManager.Ins.Init();
        UserManager.Ins.Start();

        Console.ReadKey();
    }
}