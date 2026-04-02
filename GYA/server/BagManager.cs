using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class BagManager:SingLeton<BagManager>
{
    public void Start()
    {
        MessageControll.Getinstance().AddListener(NetID.C_To_S_BagApp_Message,BagAppData);
    }
    public void BagAppData(object obj)
    {
        var parent = obj as object[];
        var data = parent[0] as byte[];
        Client c = parent[1] as Client;
        C_To_S_BagApp_Message c_Data=C_To_S_BagApp_Message.Parser.ParseFrom(data);
        int equipDataId=0, bagDataID=0;
        for(int i=0;i<c.userData.equipTypePBs.Count ;i++ )
        {
            if (c.userData.equipTypePBs[i].GridID==c_Data.EquipGrigID)
            {
                equipDataId = i;
            }
        }
        for (int i = 0; i < c.userData.itemDatas.Count; i++)
        {
            if (c.userData.itemDatas[i].GridID == c_Data.DataGridID)
            {
                bagDataID = i;
            }
        }
        int id = c.userData.equipTypePBs[equipDataId].Id;
        int sify= c.userData.equipTypePBs[equipDataId].Sifynum;
        c.userData.equipTypePBs[equipDataId].Id = c.userData.itemDatas[bagDataID].Id;
        c.userData.equipTypePBs[equipDataId].Sifynum = c.userData.itemDatas[bagDataID].Sifynum;
        c.userData.itemDatas[bagDataID].Id=id;
        c.userData.itemDatas[bagDataID].Sifynum=sify;
        S_To_C_BagApp_Message s_Data = new S_To_C_BagApp_Message();
        s_Data.DataGridID = c_Data.DataGridID;
        s_Data.EquipGrigID = c_Data.EquipGrigID;
        s_Data.Bagdata = c.userData.itemDatas[bagDataID];
        s_Data.Equipdata = c.userData.equipTypePBs[equipDataId];
        NetManager.Getinstance().SendNetMessage(NetID.S_To_C_BagApp_Message,c.st,s_Data.ToByteArray());
    }//交换
    public List<BagDataPB> SortBagItems(List<BagDataPB> items)
    {
        var validItems = items.Where(x => x.Id != 0).ToList();
        Console.WriteLine(validItems.Count);
        var emptySlots = items.Where(x => x.Id == 0).ToList();
        var sortedValidItems = validItems.ToList();

        sortedValidItems.Sort((a, b) =>
        {
            ItemData aitem = ConfigManager.Getinstance().GetItemData(a.Id);
            ItemData bitem = ConfigManager.Getinstance().GetItemData(b.Id);
            // ① Priority 从大到小
            int r = bitem.Priority.CompareTo(aitem.Priority);
            if (r != 0) return r;

            // ② 品质 Mass 从大到小
            r = bitem.Mass.CompareTo(aitem.Mass);
            if (r != 0) return r;

            // ③ 精炼等级 SifyNum 从大到小
            r = b.Sifynum.CompareTo(a.Sifynum);
            if (r != 0) return r;

            // ④ SN 从大到小
            r = b.Id.CompareTo(a.Id);
            if (r != 0) return r;

            // ⑤ 满叠在前（Count == MaxPile）
            //bool aFull = a.Count == a.MaxPile;
            //bool bFull = b.Count == b.MaxPile;
            //r = bFull.CompareTo(aFull); // true > false
            //if (r != 0) return r;

            // ⑥ ID 从大到小
            return b.Id.CompareTo(a.Id);
        });
        // 3. 清空原列表，先加排序后的有效道具，再加空格子（保持原顺序）
        items.Clear();
        items.AddRange(sortedValidItems);
        items.AddRange(emptySlots);
        return items;
    }
}
