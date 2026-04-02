using Game;
using Google.Protobuf;

namespace S;

public partial class BagManager 
{
    private void UpdateAllList(Client client)
    {
        var list = client.allList;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].AllIndex = i;
        }
    }
    
    void UpdateBagCell(Client client)
    {
        S_To_C_Get_Bag_Data_Message s_Data = new();
        s_Data.Type = client.bagType;
        UpdateBagList(client);
        s_Data.BagList.AddRange(client.bagList.Values);
        Console.WriteLine("给客户端背包发送信息");
        NetManager.Ins.SendMessage(client.st,NetID.S_To_C_Get_Bag_Data_Message,s_Data.ToByteArray());
    }
    
    private void UpdateBagList(Client client)
    {
        var allList = client.allList;
        SortBag(client);
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
}