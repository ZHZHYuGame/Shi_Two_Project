using Game;
using Google.Protobuf;

namespace S;

public partial class BagManager 
{
    private void AddBagHandle(object obj)
    {
        object[] objs = obj as object[];
        var client = objs[0] as Client;
        var data = objs[1] as byte[];
        C_To_S_Add_Bag_Message c_data = C_To_S_Add_Bag_Message.Parser.ParseFrom(data);
        AddAllBag(client,c_data.Item,c_data.Count);
        S_To_C_Get_Bag_Data_Message s_Data = new();
        s_Data.BagList.AddRange(client.bagList.Values);
        NetManager.Ins.SendMessage(client.st,NetID.S_To_C_Add_Bag_Data_Message,s_Data.ToByteArray());
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
        UpdateBagList(client);
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