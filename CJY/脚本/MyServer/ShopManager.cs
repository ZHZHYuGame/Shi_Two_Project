using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Games;
/// <summary>
/// 商品业务逻辑管理器
/// </summary>
public class ShopManager:Singleton<ShopManager>
{
    /// <summary>
    /// 默认商品商城的免费次数
    /// </summary>
    int CurrencyNums = 3;
    /// <summary>
    /// 启动商城管理器
    /// 所有功能变化
    /// </summary>
    public void Start()
    {
        MessageControll.GetInstance().AddListener(NetID.C_To_S_Shop_Type_Datas_Message, C_To_S_Shop_Type_Datas_Message_Handle);
    }
    /// <summary>
    /// 处理客户端的商品类型数据请求
    /// </summary>
    /// <param name="obj"></param>
    private void C_To_S_Shop_Type_Datas_Message_Handle(object obj)
    {
        object[] parList = obj as object[];
        byte[] netData = parList[0] as byte[];

        //PB消息的反序列化数据
        C_2_S_SpeciaShops c_Data = C_2_S_SpeciaShops.Parser.ParseFrom(netData);

        //获取物品表的数据，找对应商城类型的所有道具，随机出这次商品数据（设计每天会有一次系统刷新，其余次数为代价刷新）

        //得到随机商品数据
        List<CShopGoodsData> shopGoodList = ConfigManager.GetInstance().RandomShopTypeShopDatas();
        //网络消息序列化
        S_2_C_SpeciaShops s_SpecialShopsMsg = new S_2_C_SpeciaShops();
        //PB里面没有C#对应的数据结构
        foreach (var goods in shopGoodList)
        {
            ShopGoodsData data = new ShopGoodsData();
            data.ShopGoodsID = goods.goodID;
            data.Level = goods.goodLevel;
            s_SpecialShopsMsg.GoodsList.Add(data);
        }

        switch (c_Data.UpdateType)
        {
            case ShopsUpdateType.None:
                foreach (var user in UserManager.GetInstance().userDict)
                {
                    NetManager.GetInstance().SendNetMessage(user.Value.st,NetID.S_To_C_Shop_Type_Datas_Message, s_SpecialShopsMsg.ToByteArray());
                }
                break;
            case ShopsUpdateType.FreeUpdate:
                s_SpecialShopsMsg.CurrencyNums = --CurrencyNums;
                break;
            case ShopsUpdateType.GlodUpdate:
                //s_SpecialShopsMsg.CurrencyNums = 
                break;
            default:
                break;
        }
    }
}

