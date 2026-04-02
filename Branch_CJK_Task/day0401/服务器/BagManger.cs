using System.Text;
using BagNet.Net;
using Google.Protobuf;
using Message;

namespace BagNet;

public class BagManger:SingLeton<BagManger>
{
    public void Start()
    {
        MessageControll.GetInstance.AddListener(NetID.C_2_S_BagInit,BagInit);
        MessageControll.GetInstance.AddListener(NetID.C_2_S_RefreshUI,RefreshClassify);
        
    }
    private void RefreshClassify(object obj)
    {
        object[] args = obj as object[];
        byte[] data = args[0] as byte[];
        C2S_RefreshUI c_data=C2S_RefreshUI.Parser.ParseFrom(data);
        Client c=args[1] as Client;
        S2C_RefreshUI s_data=new S2C_RefreshUI();
        List<BagItem> items = new List<BagItem>();
        switch (c_data.Type)
        {
            case "全部":
                for (int i = 0; i <c.userData.MainBag.Items.Count; i++)
                {
                    items.Add(c.userData.MainBag.Items[i]);
                }
                break;
            case "武器":
                for (int i = 0; i <c.userData.MainBag.Items.Count; i++)
                {
                    ItemData item=ConfigManger.GetInstance.FindItem(c.userData.MainBag.Items[i].ItemId);
                    if (item.Type == 1)
                    {
                        items.Add(c.userData.MainBag.Items[i]);
                    }
                }
                break;
            case "防具":
                for (int i = 0; i <c.userData.MainBag.Items.Count; i++)
                {
                    ItemData item=ConfigManger.GetInstance.FindItem(c.userData.MainBag.Items[i].ItemId);
                    if (item.Type == 2)
                    {
                        items.Add(c.userData.MainBag.Items[i]);
                    }
                }
                break;
            case "药水":
                for (int i = 0; i <c.userData.MainBag.Items.Count; i++)
                {
                    ItemData item=ConfigManger.GetInstance.FindItem(c.userData.MainBag.Items[i].ItemId);
                    if (item.Type == 4)
                    {
                        items.Add(c.userData.MainBag.Items[i]);
                    }
                }
                break;
            case "材料":
                for (int i = 0; i <c.userData.MainBag.Items.Count; i++)
                {
                    ItemData item=ConfigManger.GetInstance.FindItem(c.userData.MainBag.Items[i].ItemId);
                    if (item.Type == 8)
                    {
                        items.Add(c.userData.MainBag.Items[i]);
                    }
                }
                break;
        }
        s_data.NowItems.AddRange(items);
        NetManger.GetInstance.SendNetMessage(c.st,NetID.S_2_C_RefreshUI,s_data.ToByteArray());
    }

    private void BagInit(object obj)
    {
        var idGen = new SnowflakeIdGenerator(1); // 机器ID固定为1
        object[] args = (object[])obj;
        byte[] content = (byte[])args[0];
        Client c = (Client)args[1];
        Console.WriteLine(c.port+"发送背包请求");
        S2C_GetBackpackData s_data=new S2C_GetBackpackData();
        BagData bdata = new BagData();
        bdata.TotalSlots = 50;
        bdata.UnlockedSlots = 20;
        List<BagItem> items = new List<BagItem>();
        for (int i = 0; i < ConfigManger.GetInstance.allList.Count; i++)
        {
            BagItem bItem = new BagItem();
            long uid = idGen.NextId();
            string uidStr = uid.ToString(); // 作为string传给客户端
            bItem.Uid=uidStr;
            bItem.ItemId=ConfigManger.GetInstance.allList[i].Id;
            bItem.Count = 1;
            bItem.Position = i;
            bItem.IsNew=true;
            if (ConfigManger.GetInstance.allList[i].Type == 1 || ConfigManger.GetInstance.allList[i].Type == 2)
            {
                ItemExtra itemExtra = new ItemExtra();
                itemExtra.EnhanceLevel = 1;
                itemExtra.RefineLevel = 1;
                itemExtra.SocketedGems.AddRange(new List<int>());
                bItem.Extra=itemExtra;
            }
            items.Add(bItem);
        }
        bdata.Items.AddRange(items);
        s_data.MainBag=bdata;
        s_data.Equipped = new EquippedData();
        s_data.QuickDrugs.AddRange(new List<QuickDrugSlot>());
        c.userData.MainBag=s_data.MainBag;
        c.userData.Equipped=s_data.Equipped;
        c.userData.QuickDrugs = null;
        c.userData.Gold = 100;
        NetManger.GetInstance.SendNetMessage(c.st,NetID.S_2_C_BagInit,s_data.ToByteArray());
    }
}