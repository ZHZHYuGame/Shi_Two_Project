using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Games;
using Google.Protobuf;
using Newtonsoft.Json;
public class UserManager:SingLeton<UserManager>
{
    public List<SUserData> sUserDatas = new List<SUserData>();
    public Dictionary<int,Client> allclient=new Dictionary<int,Client>();
    public void Start()
    {
        var str= File.ReadAllText("User.json");
        sUserDatas=JsonConvert.DeserializeObject<List<SUserData>>(str);
        MessageControll.Getinstance().AddListener(NetID.C_To_S_BagData_Message,GetBagData);
    }
    public void AddClient(Client client)
    {
        if(!allclient.ContainsKey(client.port))
        {
            allclient.Add(client.port, client);
        }
    }
    public void GetBagData(object obj)
    {
        var parent= obj as object[];
        var data= parent[0] as byte[];
        Client c= parent[1] as Client;
        C_To_S_BagData_Message c_data=C_To_S_BagData_Message.Parser.ParseFrom(data);
        S_To_C_BagData_Message s_data = new S_To_C_BagData_Message();
        var ent= ConfigManager.Getinstance().sUserDatas;
        c.userData = sUserDatas[0];//用户数据
        if (c_data.Countype == CountType.Two)
        {
            List<BagDataPB> zhengli = BagManager.Getinstance().SortBagItems(c.userData.itemDatas);
            for (int i = 0; i < zhengli.Count; i++)
            {
                zhengli[i].GridID = i;
                s_data.BagList.Add(zhengli[i]);
                c.userData.itemDatas[i]=zhengli[i];
            }
            NetManager.Getinstance().SendNetMessage(NetID.S_To_C_BagData_Message, c.st, s_data.ToByteArray());
        }
        else
        {
            int num = c.userData.kai + c.userData.suo;//判断生成多少个格子
            s_data.Game = c.userData.gameID;//模型id
            s_data.Fighting = (int)c.userData.fighting;

            List<BagDataPB> bagDataPBs = new List<BagDataPB>();
            for (int i = 0; i < c.userData.equipTypePBs.Count; i++)
            {
                EquipTypePB eq = new EquipTypePB();
                eq.GridID = c.userData.equipTypePBs[i].GridID;
                eq.Id = c.userData.equipTypePBs[i].Id;
                eq.Name = c.userData.equipTypePBs[i].Name;
                eq.X = c.userData.equipTypePBs[i].X;
                eq.Y = c.userData.equipTypePBs[i].Y;
                s_data.EquipType.Add(eq);
            }
            for (int i = 0; i < num; i++)
            {
                BagDataPB bagDataPB = new BagDataPB();
                bagDataPB.GridID = i;
                if (i < ent.Count)
                {
                    bagDataPB.Id = ent[i].ID;
                    if (ent[i].Type == ItemType.药水 || ent[i].Type == ItemType.材料)
                    {
                        Random random = new Random();
                        int count = random.Next(2, 6);
                        bagDataPB.Num = count;
                    }
                    else
                    {
                        bagDataPB.Num = 1;
                        bagDataPB.Sifynum = 1;
                    }
                    bagDataPB.IsSuo = 2;//2是没锁
                }
                else if (i >= c.userData.kai)
                {
                    bagDataPB.Id = 0;
                    bagDataPB.IsSuo = 1;//1是锁
                }
                else
                {
                    bagDataPB.Id = 0;
                    bagDataPB.IsSuo = 2;//2是没锁
                }
                bagDataPBs.Add(bagDataPB);
            }
            List<BagDataPB> dataPB = BagManager.Getinstance().SortBagItems(bagDataPBs);
            for (int i = 0; i < dataPB.Count; i++)
            {
                dataPB[i].GridID = i;
                s_data.BagList.Add(dataPB[i]);
                c.userData.itemDatas.Add(dataPB[i]);
            }
            for (int i = 0; i < c.userData.fenlei.Count; i++)
            {
                BagLeiTypePB lei = new BagLeiTypePB();
                lei.Name = c.userData.fenlei[i].Name;
                lei.Icon = c.userData.fenlei[i].Icon;
                s_data.BagTypeList.Add(lei);
            }
            for (int i = 0; i < c.userData.huoBiTypes.Count; i++)
            {
                BagHuoBiType lei = new BagHuoBiType();
                lei.Path = c.userData.huoBiTypes[i].Path;
                lei.Money = c.userData.huoBiTypes[i].Money;
                s_data.BagHuoBiType.Add(lei);
            }
            NetManager.Getinstance().SendNetMessage(NetID.S_To_C_BagData_Message, c.st, s_data.ToByteArray());
        }
    }
}
public class SUserData
{
    public string zh;
    public string mm;
    public List<BagHuoBiType> huoBiTypes = new List<BagHuoBiType>();
    public int gameID;
    public float fighting;
    public List<BagLeiTypePB> fenlei=new List<BagLeiTypePB>();
    public List<BagDataPB> itemDatas = new List<BagDataPB>();
    public List<EquipTypePB> equipTypePBs = new List<EquipTypePB>();
    public int kai, suo;
    
}
