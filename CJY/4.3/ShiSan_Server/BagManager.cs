using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConfigManager;

namespace ShiSan_Server
{
    public class BagManager:Singleton<BagManager>
    {
        public void Start()
        {
            MessageControll.GetInstance().AddListener(NetID.C_To_S_GetBagData_Message, C_To_S_GetBagDatas_Message_Handle);
        }

        private void C_To_S_GetBagDatas_Message_Handle(object obj)
        {
            object[] parList = obj as object[];
            byte[] netData = parList[0] as byte[];
            C_To_S_Bags c_Data= C_To_S_Bags.Parser.ParseFrom(netData);

            //获取随机背包数据
            List<CBagGoodsData> bagGoodsList = ConfigManager.GetInstance().RandomBagDatas();
            foreach(var good in bagGoodsList)
            {
                Console.WriteLine(good.equipType);
            }
            S_To_C_Bags BagType_Bag=new S_To_C_Bags();
            switch (c_Data.BagType)
            {
                case BagType.All:
                    foreach (var good in bagGoodsList)
                    {
                        Console.WriteLine(good.itemName);
                        BagGoodsData data = new BagGoodsData();
                        data.ItemID = good.itemID;
                        data.GridID = good.gridID;
                        data.EquipType = good.equipType;
                        data.Des = good.des;
                        data.Icon = good.icon;
                        data.ItemName = good.itemName;
                        data.Rarity = good.rarity;
                        data.MaxStack = good.maxStack;
                        BagType_Bag.GoodsList.Add(data);
                    }
                    foreach (var user in UserManager.GetInstance().userDict)
                    {
                        NetManager.GetInstance().SendNetMessage(user.Value.st, NetID.S_To_C_GetBagData_Message, BagType_Bag.ToByteArray());
                    }
                    break;
                case BagType.Weapon:
                    foreach (var good in bagGoodsList)
                    {
                        if (good.equipType == "武器")
                        {
                            BagGoodsData data = new BagGoodsData();
                            data.ItemID = good.itemID;
                            data.GridID = good.gridID;
                            data.EquipType = good.equipType;
                            data.Des = good.des;
                            data.Icon = good.icon;
                            data.ItemName = good.itemName;
                            data.Rarity = good.rarity;
                            data.MaxStack = good.maxStack;
                            BagType_Bag.GoodsList.Add(data);
                        }
                    }
                    foreach (var user in UserManager.GetInstance().userDict)
                    {
                        NetManager.GetInstance().SendNetMessage(user.Value.st, NetID.S_To_C_GetBagData_Message, BagType_Bag.ToByteArray());
                    }
                    break;
                case BagType.Material:
                    foreach (var good in bagGoodsList)
                    {
                        if (good.equipType == "宝箱")
                        {
                            BagGoodsData data = new BagGoodsData();
                            data.ItemID = good.itemID;
                            data.GridID = good.gridID;
                            data.EquipType = good.equipType;
                            data.Des = good.des;
                            data.Icon = good.icon;
                            data.ItemName = good.itemName;
                            data.Rarity = good.rarity;
                            data.MaxStack = good.maxStack;
                            BagType_Bag.GoodsList.Add(data);
                        }
                    }
                    foreach (var user in UserManager.GetInstance().userDict)
                    {
                        NetManager.GetInstance().SendNetMessage(user.Value.st, NetID.S_To_C_GetBagData_Message, BagType_Bag.ToByteArray());
                    }
                    break;
                case BagType.Armor:
                    foreach (var good in bagGoodsList)
                    {
                        if (good.equipType != "武器" && good.equipType!="药品" && good.equipType!="宝箱")
                        {
                            BagGoodsData data = new BagGoodsData();
                            data.ItemID = good.itemID;
                            data.GridID = good.gridID;
                            data.EquipType = good.equipType;
                            data.Des = good.des;
                            data.Icon = good.icon;
                            data.ItemName = good.itemName;
                            data.Rarity = good.rarity;
                            data.MaxStack = good.maxStack;
                            BagType_Bag.GoodsList.Add(data);
                        }
                    }
                    foreach (var user in UserManager.GetInstance().userDict)
                    {
                        NetManager.GetInstance().SendNetMessage(user.Value.st, NetID.S_To_C_GetBagData_Message, BagType_Bag.ToByteArray());
                    }
                    break;
                case BagType.Medicine:
                    foreach (var good in bagGoodsList)
                    {
                        if (good.equipType == "药品")
                        {
                            BagGoodsData data = new BagGoodsData();
                            data.ItemID = good.itemID;
                            data.GridID = good.gridID;
                            data.EquipType = good.equipType;
                            data.Des = good.des;
                            data.Icon = good.icon;
                            data.ItemName = good.itemName;
                            data.Rarity = good.rarity;
                            data.MaxStack = good.maxStack;
                            BagType_Bag.GoodsList.Add(data);
                        }
                    }
                    foreach (var user in UserManager.GetInstance().userDict)
                    {
                        NetManager.GetInstance().SendNetMessage(user.Value.st, NetID.S_To_C_GetBagData_Message, BagType_Bag.ToByteArray());
                    }
                    break;
            }
        }
    }
}
