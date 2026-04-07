using Game;
using Google.Protobuf;

namespace S;

public partial class BagManager 
{
    private void SellItemHandle(object obj)
    {
        object[] objs = obj as object[];
        var client = objs[0] as Client;
        var data = objs[1] as byte[];
        
        C_To_S_Sell_Bag_Message c_data = C_To_S_Sell_Bag_Message.Parser.ParseFrom(data);
        var item = client.allList[c_data.Data.AllIndex];
        item.Count -= c_data.Count;
        var price = item.Item.Price * c_data.Count;
        client.money += price;
        if (item.Count <= 0)
        {
            client.allList.RemoveAt(item.AllIndex);
            UpdateAllList(client);
        }
        UpdateBagCell(client);

        S_To_C_Set_Money_Message s_data = new();
        s_data.Money = client.money;
        NetManager.Ins.SendMessage(client.st,NetID.S_To_C_Get_User_Money_Message,s_data.ToByteArray());
    }
}