using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            S_To_C_Bags c_Data= S_To_C_Bags.Parser.ParseFrom(netData);

            //得到随机商品数据
            //List<CBagGoodsData> shopGoodList = ConfigManager.GetInstance().RandomShopTypeShopDatas();
            //网络消息序列化
            S_2_C_SpeciaShops s_SpecialShopsMsg = new S_2_C_SpeciaShops();
            //PB里面没有C#对应的数据结构
            //foreach (var goods in shopGoodList)
            //{
            //    ShopGoodsData data = new ShopGoodsData();
            //    data.ShopGoodsID = goods.goodID;
            //    data.Level = goods.goodLevel;
            //    s_SpecialShopsMsg.GoodsList.Add(data);
            //}

            //switch (c_Data.UpdateType)
            //{
            //    case ShopsUpdateType.None:
            //        foreach (var user in UserManager.GetInstance().userDict)
            //        {
            //            NetManager.GetInstance().SendNetMessage(user.Value.st, NetID.S_To_C_Shop_Type_Datas_Message, s_SpecialShopsMsg.ToByteArray());
            //        }
            //        break;
            //    case ShopsUpdateType.FreeUpdate:
            //        s_SpecialShopsMsg.CurrencyNums = --CurrencyNums;
            //        break;
            //    case ShopsUpdateType.GlodUpdate:
            //        //s_SpecialShopsMsg.CurrencyNums = 
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}
