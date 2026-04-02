using Game;
using Google.Protobuf;

namespace S;

public partial class BagManager 
{
    private void WearEq(object obj)
    {
        object[] objs = obj as object[];
        var client = objs[0] as Client;
        var data = objs[1] as byte[];
        
        C_To_S_Wear_Eq_Message c_data = C_To_S_Wear_Eq_Message.Parser.ParseFrom(data);
        S_To_C_Wear_Eq_Message s_data = new();

        if (client.bagList.TryGetValue(c_data.Data.Index, out var item))
        {
            if (c_data.Data.Item.Id == item.Item.Id)
            {
                Console.WriteLine($"{client.port} 想穿戴 {item.Item.Name} 装备 , 下标为 {item.Index}");
                client.eqDic[item.Item.Position] = item.Item;
                client.allList.RemoveAt(c_data.Data.AllIndex);
                UpdateAllList(client);
                s_data.Result = ResultType.Yes;
                s_data.ItemList.AddRange(client.eqDic.Values.ToArray());
            }
            else
            {
                s_data.Result = ResultType.No;
            }
        }
        else
        {
            s_data.Result = ResultType.No;
        }
        NetManager.Ins.SendMessage(client.st, NetID.S_To_C_Wear_Eq_Message,s_data.ToByteArray());
        if (s_data.Result == ResultType.Yes)
        {
            UpdateBagCell(client);
        }
    }

    private void PushEqHandle(object obj)
    {
        object[] objs = obj as object[];
        var client = objs[0] as Client;
        var data = objs[1] as byte[];
        
        C_To_S_Push_Eq_Message c_data = C_To_S_Push_Eq_Message.Parser.ParseFrom(data);
        
        var item = client.eqDic[c_data.EqItem.Position];
        AddAllBag(client,item,1);
        S_To_C_Get_Bag_Data_Message s_Data = new();
        s_Data.BagList.AddRange(client.bagList.Values);
        NetManager.Ins.SendMessage(client.st,NetID.S_To_C_Add_Bag_Data_Message,s_Data.ToByteArray());
        
        S_To_C_Wear_Eq_Message s_data = new();
        client.eqDic[c_data.EqItem.Position] = new();
        s_data.ItemList.AddRange(client.eqDic.Values);
        s_data.Result = ResultType.Yes;
        NetManager.Ins.SendMessage(client.st, NetID.S_To_C_Wear_Eq_Message,s_data.ToByteArray());
    }
}