namespace FWQ
{
    internal class Program
    {
        static int time=0;
        static async Task Main(string[] args)
        {
            ConfigMgr.Instance().Start();
            FWQManager.Instance().Start();
            Mail mail = Mail.Instance;
            BagMgr bag = BagMgr.Instance;
            while (true)
            {
                await Task.Delay(1000);
                time += 1;
            }
        }
    }
}
