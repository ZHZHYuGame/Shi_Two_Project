namespace FWQ
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ConfigMgr.Instance().Start();
            FWQManager.Instance().Start();
            Mail mail = Mail.Instance;
            BagMgr bag = BagMgr.Instance;
            while (true)
            {
                await Task.Delay(100);
                foreach(var item in Mail.Instance.Dic)
                {
                    foreach(var item2 in item.Value.ToList())
                    {
                        if(!item2.YJ.isAward)
                        {
                            item2.YJ.Time += 0.1f;
                            if(item2.YJ.Time>=15)
                            {
                                item.Value.Remove(item2);
                                foreach (var itemss in FWQManager.Instance().clientsList.ToList())
                                {
                                    if(itemss==item.Key)
                                    FWQManager.Instance().SendNetMessage(item.Key.st, NetID.S_2_CGame, Mail.instance.GetBytes(item.Key));
                                }
                            }
                        }
                    }
                }
                
            }
        }
    }
}
