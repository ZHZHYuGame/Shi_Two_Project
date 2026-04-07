using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Games;
using Newtonsoft.Json;

public class ConfigManager:Singlton<ConfigManager>
{
    public List<Good> goods = new List<Good>();
    public List<CellData> allCellDatas = new List<CellData>();
    public List<CellData> wuQiCellDatas = new List<CellData>();
    public List<CellData> fangJuCellDatas = new List<CellData>();
    public List<CellData> yaoShuiCellDatas = new List<CellData>();
    public List<CellData> caiLiaoCellDatas = new List<CellData>();

    public void Start()
    {
        int allId = 0, wqId = 0, fjId = 0, ysId = 0, clId = 0;
        string str = File.ReadAllText("goods.json");
        goods=JsonConvert.DeserializeObject<List<Good>>(str);
        Console.WriteLine(goods.Count);
        foreach (var item in goods)
        {
            CellData cellData = new CellData();
            cellData.gridId = allId++;
            cellData.goodId = item.id;
            cellData.nowNum = 1;
            cellData.cellState = 2;
            allCellDatas.Add(cellData);
            switch (item.goodType)
            {
                case 1:
                    CellData wqCellData = new CellData();
                    wqCellData.gridId = wqId++;
                    wqCellData.goodId = item.id;
                    wqCellData.nowNum = 1;
                    wqCellData.cellState = 2;
                    wuQiCellDatas.Add(wqCellData);
                    break;
                case 2:
                    CellData fjCellData = new CellData();
                    fjCellData.gridId = fjId++;
                    fjCellData.goodId = item.id;
                    fjCellData.nowNum = 1;
                    fjCellData.cellState = 2;
                    fangJuCellDatas.Add(fjCellData);
                    break;
                case 3:
                    CellData ysCellData = new CellData();
                    ysCellData.gridId = ysId++;
                    ysCellData.goodId = item.id;
                    ysCellData.nowNum = 1;
                    ysCellData.cellState = 2;
                    yaoShuiCellDatas.Add(ysCellData);
                    break;
                case 4:
                    CellData clCellData = new CellData();
                    clCellData.gridId = clId++;
                    clCellData.goodId = item.id;
                    clCellData.nowNum = 1;
                    clCellData.cellState = 2;
                    caiLiaoCellDatas.Add(clCellData);
                    break;
            }
        }

    }

    public List<CellData> GetTypeCellData(GoodsType goodsType)
    {
        List<CellData> temp;

        switch (goodsType)
        {
            case GoodsType.All:
                temp = allCellDatas;
                break;
            case GoodsType.WuQi:
                temp = wuQiCellDatas;
                break;
            case GoodsType.FangJu:
                temp = fangJuCellDatas;
                break;
            case GoodsType.YaoShui:
                temp = yaoShuiCellDatas;
                break;
            case GoodsType.CaiLiao:
                temp = caiLiaoCellDatas;
                break;
            default:
                temp = null;
                break;
        }
        return temp;
    }
}
/// <summary>
/// 格子类型
/// </summary>
public class CellData
{
    public int gridId;
    public int goodId;
    public int nowNum;
    /// <summary>
    /// //1没占用，2占用，3锁
    /// </summary>
    public int cellState;
}

public class Good
{
    public int id;                      // 道具唯一ID
    public string name;                 // 道具名称
    public string iconPath;             //图片资源
    // 道具类型: 1武器 2防具 3药水 4材料
    public int goodType;
    public int btnType;//物品类型：1装备，2道具
    public int sell;                    // 出售价格，0不可出售
    //0其他，1头,2衣服,3项链,4戒指,5护腿,6鞋子,7武器
    public int wearType;           //装备穿戴部位
    public int quality;                 //物品品质:0无，1灰，2绿 3蓝 4紫 5橙
    public int maxPile;                 // 堆叠上限，0表示不可堆叠
    public string msg;                  //道具描述

}
