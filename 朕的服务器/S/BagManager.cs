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
    }
    

    private void GetBagDataHandle(object obj)
    {
        object[] objs = obj as object[];
        var client = objs[0] as Client;
        var data = objs[1] as byte[];
        Console.WriteLine(client.port + "请求获取背包数据...");
        C_To_S_Get_Bag_Data_Message c_Data = C_To_S_Get_Bag_Data_Message.Parser.ParseFrom(data);
        client.bagType = c_Data.Type;
        UpdateBagCell(client);
    }

    void UpdateBagCell(Client client)
    {
        S_To_C_Get_Bag_Data_Message s_Data = new();
        s_Data.Type = client.bagType;
        switch (client.bagType)
        {
            case "全部":
                AddDicByType(client,client.allList);
                break;
            default:
                var list = GetDataByType(client);
                AddDicByType(client,list);
                break;
        }
        s_Data.BagList.AddRange(client.bagList.Values);
        Console.WriteLine("给客户端背包发送信息");
        NetManager.Ins.SendMessage(client.st,NetID.S_To_C_Get_Bag_Data_Message,s_Data.ToByteArray());
    }
    

    public void InitEqCellData(Client client)
    {
        for (int i = 0; i < 7; i++)
        {
            var type = (EqPositionType)i;
            client.eqDic.Add(type,new Item());
        }
    }

    public List<BagData> GetDataByType(Client client)
    {
        var list = client.allList;
        List<BagData> tempData = new();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item.Type == client.bagType)
            {
                tempData.Add(list[i]);
            }
        }
        return tempData;
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
            AddAllBag(client,list[i],1);
        }
        InitEqCellData(client);
    }


    public void AddAllBag(Client client, Item item, int count)
    {
        var allList = client.allList;
        if (item.Type != "装备")
        {
            bool flag = false;
            for (int i = 0; i < allList.Count; i++)
            {
                if (item.Id == allList[i].Item.Id)
                {
                    allList[i].Count += count;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                var bagData = new BagData();
                bagData.Item = item;
                bagData.Count = count;
                bagData.AllIndex = allList.Count;
                allList.Add(bagData);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                var bagData = new BagData();
                bagData.Item = item;
                bagData.Count = 1;
                bagData.AllIndex = allList.Count;
                allList.Add(bagData);
            }
        }

        switch (client.bagType)
        {
            case "全部":
                AddDicByType(client,allList);
                break;
            default:
                var list = GetDataByType(client);
                AddDicByType(client,list);
                break;
        }
    }

    public void AddDicByType(Client client,List<BagData> list)
    {
        var bagDic = client.bagList;
        for (int i = 0; i < bagDic.Count; i++)
        {
            bagDic[i] = new BagData();
            bagDic[i].Index = i;
            if (i < list.Count)
            {
                bagDic[i].Item = list[i].Item;
                bagDic[i].Count = list[i].Count;
                bagDic[i].AllIndex = list[i].AllIndex;
            }
            else
            {
                bagDic[i].Item = null;
                bagDic[i].Count = 0;
                bagDic[i].AllIndex = -1;
            }
        }
    }
    
}
