using Game;
using Google.Protobuf;
using Newtonsoft.Json;

namespace S;

public partial class BagManager : Singleton<BagManager>
{
    private Dictionary<int, Item> ItemDic = new();

    public void Init()
    {
        var itemList = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText("Item.json"));
        foreach (var item in itemList)
        {
            ItemDic.Add(item.Id,item);
        }
        Console.WriteLine("item配置表数量"+itemList.Count);
        MessageCenter.Ins.AddListener(NetID.C_To_S_Get_Bag_Data_Message,GetBagDataHandle);
        MessageCenter.Ins.AddListener(NetID.C_To_S_Wear_Eq_Message,WearEq);
        MessageCenter.Ins.AddListener(NetID.C_To_S_Add_Bag_Data_Message,AddBagHandle);
        MessageCenter.Ins.AddListener(NetID.C_To_S_Push_Eq_Message,PushEqHandle);
        MessageCenter.Ins.AddListener(NetID.C_To_S_Sell_Item_Message,SellItemHandle);
    }

    public void InitEqCellData(Client client)
    {
        for (int i = 0; i < 7; i++)
        {
            var type = (EqPositionType)i;
            client.eqDic.Add(type,new Item());
        }
    }
    

    public void InitUserData(Client client)
    {
        for (int i = 0; i < 30; i++)
        {
            var bagData = new BagData
            {
                Item = null,
                Count = 0,
                Index = i
            };
            client.bagList.Add(i,bagData);
        }
        var list = ItemDic.Values.ToList();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Type!="装备")
            {
                AddAllBag(client,list[i],5);
            }
            else
            {
                AddAllBag(client,list[i],1);
            }
        }
        InitEqCellData(client);
    }

    
    
}
