using Games;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FWQ
{
    public class YJBase
    {
        public YJ YJ;
        public int YJID;
        public bool Is;
    }
    internal class Mail
    {
        public Dictionary<Client,List<YJBase>> Dic=new Dictionary<Client, List<YJBase>>();
        public static Mail instance;
        public int ID;
        public static Mail Instance
        {
            get 
            {
                if(instance==null)
                {
                    instance = new Mail();
                }
                return instance; 
            }
        }
        public Mail()
        {
            MessageControll.Instance().AddListener(NetID.S_2_COpenMail,OpenMail);
            MessageControll.Instance().AddListener(NetID.S_2_CSXMail,SXMail);
            MessageControll.Instance().AddListener(NetID.S_2_CGetMail,GetMail);
            MessageControll.Instance().AddListener(NetID.S_2_CDelMail,DelMail);
            MessageControll.Instance().AddListener(NetID.S_2_CGetAllMail,GetAllMail);
            MessageControll.Instance().AddListener(NetID.S_2_CDelAllMail,DelAllMail);
            MessageControll.Instance().AddListener(NetID.S_2_CAddMail,AddMail);

        }
        /// <summary>
        /// 添加一个邮件
        /// </summary>
        /// <param name="obj"></param>

        private void AddMail(object obj)
        {
            object[] Obj = obj as object[];
            Client C = Obj[1] as Client;
            int Int = BitConverter.ToInt32(Obj[0] as byte[]);
            YJ temp = ConfigMgr.Instance().YJList[Int-1];
            YJ Temp = new YJ()
            {
                isAward = temp.isAward,
                content = temp.content,
                emailID = temp.emailID,
                Time = temp.Time,
                title = temp.title,
            };
            Temp.StartTime =DateTime.Now;
            Temp.awardList = new List<Item>();
            if (Temp.isAward)
            {
                
                Random random = new Random();
                for (int i = 0; i < random.Next(1, 4); i++)
                {
                    Temp.awardList.Add(ConfigMgr.Instance().ItemList[random.Next(ConfigMgr.Instance().ItemList.Count)]);
                }
            }
            YJBase Temp2 = new YJBase()
            {
                YJID = ID++,
                Is = false,
                YJ = Temp
            };
            Dic[C].Add(Temp2);
            FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
        }
        public void AddMail(Client C,int Int)
        {
            YJ temp = ConfigMgr.Instance().YJList[Int-1];
            YJ Temp = new YJ()
            {
                isAward = temp.isAward,
                content = temp.content,
                emailID = temp.emailID,
                Time = temp.Time,
                title = temp.title,
            };
            Temp.StartTime = DateTime.Now;
            Temp.awardList = new List<Item>();
            if (Temp.isAward)
            {
                
                Random random = new Random();
                for (int i = 0; i < random.Next(1, 4); i++)
                {
                    Temp.awardList.Add(ConfigMgr.Instance().ItemList[random.Next(ConfigMgr.Instance().ItemList.Count)]);
                }
            }
            YJBase Temp2 = new YJBase()
            {
                YJID = ID++,
                Is = false,
                YJ = Temp
            };
            Dic[C].Add(Temp2);
        }

        /// <summary>
        /// 删除所有无附件且已读的邮件
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void DelAllMail(object obj)
        {
            object[] Obj = obj as object[];
            Client C = Obj[1] as Client;
            int count = 0;
            foreach (var item in Dic[C].ToList())
            {
                count++;
                if (count > 50)
                {
                    break;
                }
                if ((!item.YJ.isAward)&&item.Is)
                {
                    Dic[C].Remove(item);
                    
                }
            }
            FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
        }
        /// <summary>
        /// 领取所有有附件的邮件
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void GetAllMail(object obj)
        {
            object[] Obj = obj as object[];
            Client C = Obj[1] as Client;
            int count = 0;
            int Iscount = 0;
            bool Iss=false;
            foreach (var item in Dic[C].ToList())
            {
                count++;
                if(count>50)
                {
                    break;
                }
                    if(!item.YJ.isAward)
                    {
                       item.Is = true; 
                        continue;
                    }
                    BagMgr.instance.AddBag(C, item.YJ.awardList, out bool Is);
                    if (Is)
                    {
                        item.Is = true;
                       // Console.WriteLine("领取邮件成功");
                        item.YJ.isAward = false;
                    Iscount++;
                       
                    }
                    else
                    {
                        Iss = true;
                       // Console.WriteLine("领取邮件失败");
                    }
                    
            }
            if(Iscount<=0&&Iss)
            {
                FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CAddlose, BitConverter.GetBytes(true));
            }
            FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
        }
        /// <summary>
        /// 删除一个邮件
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void DelMail(object obj)
        {
            object[] Obj = obj as object[];
            Client C = Obj[1] as Client;
            int Int = BitConverter.ToInt32(Obj[0] as byte[]);
            foreach (var item in Dic[C])
            {
                if(item.YJID==Int)
                {
                    Dic[C].Remove(item);
                    FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
                    return;
                }
            }
        }
        /// <summary>
        /// 领取一个邮件
        /// </summary>
        /// <param name="obj"></param>
        private void GetMail(object obj)
        {
            object[] Obj = obj as object[];
            Client C = Obj[1] as Client;
            int Int = BitConverter.ToInt32(Obj[0] as byte[]);
            foreach(var item in Dic[C])
            {
                if(item.YJID==Int)
                {
                    BagMgr.instance.AddBag(C,item.YJ.awardList, out bool Is);
                    if(Is)
                    {
                       // Console.WriteLine("领取邮件成功");
                        item.YJ.isAward = false;
                        FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
                    }
                    else
                    {
                       // Console.WriteLine("领取邮件失败");
                        FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CAddlose,BitConverter.GetBytes(true));
                    }
                    break;
                }
            }
            
        }

        /// <summary>
        /// 刷新UI
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SXMail(object obj)
        {
            object[] Obj = obj as object[];
            Client C = Obj[1] as Client;
            FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
        }

        /// <summary>
        /// 打开邮件
        /// </summary>
        /// <param name="obj"></param>

        private void OpenMail(object obj)
        {
            object[] Obj=obj as object[];
            Client C = Obj[1] as Client;
            int Int=BitConverter.ToInt32(Obj[0] as byte[]);
            foreach(var item in Dic[C])
            {
                if(item.YJID==Int)
                {
                    if(item.Is)
                    {
                        FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
                        return;
                    }
                    else
                    {
                        item.Is = true;
                        FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
                        return;
                    }
                }
            }

        }

        /// <summary>
        /// 玩家进入初始化
        /// </summary>
        /// <param name="C"></param>
        public void Start(Client C)
        {
            Dic.Add(C, new List<YJBase>());
            AddMail(C, 1);
            AddMail(C, 2);
            Dic[C][1].YJ.StartTime = Dic[C][0].YJ.StartTime;
            Console.WriteLine($"客户端{C.port}已登录,发送初始数据");
            FWQManager.Instance().SendNetMessage(C.st, NetID.S_2_CGame, GetBytes(C));
        }
        /// <summary>
        /// 将当前最高50封邮件转换为字节数组
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        public byte[] GetBytes(Client C)
        {
            Dic[C].Sort((a,b) =>
            {
                var temp=b.YJ.StartTime-a.YJ.StartTime;
                if(temp==new TimeSpan())
                {
                    int a1, b1;
                    if(a.Is)
                    {
                        a1 = 2;
                    }
                    else
                    {
                        a1 = 1;
                    }
                    if (b.Is)
                    {
                        b1 = 2;
                    }
                    else
                    {
                        b1 = 1;
                    }
                    if(a1!=b1)
                    {
                        return a1-b1;
                    }
                    else
                    {
                        int a2, b2;
                        if (a.YJ.isAward)
                        {
                            a2 = 1;
                        }
                        else
                        {
                            a2 = 2;
                        }
                        if (b.YJ.isAward)
                        {
                            b2 = 1;
                        }
                        else
                        {
                            b2 = 2;
                        }
                        return a2-b2;
                    }
                }
                else
                {
                    if(temp>new TimeSpan())
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            });
            S_To_C_Mail Temp = new S_To_C_Mail();
            int count = 0;
            foreach (var item in Dic[C])
            {
                count++;
                if(count>50)
                {
                    break;
                }
                MailData Data=new MailData();
                Data.MailID = item.YJID;
                Data.IsRead=item.Is;
                MailBaseData Temps = new MailBaseData()
                {
                    IsAward = item.YJ.isAward,
                    Title = item.YJ.title,
                    EmailID = item.YJ.emailID,
                    Content = item.YJ.content,
                    StartTime = item.YJ.StartTime.ToString()
                };
                foreach(var item2 in item.YJ.awardList)
                {
                    Temps.AwardList.Add(item2.id);
                }
                Data.Data= Temps;
                Temp.MailData.Add(Data);
            }
            return Temp.ToByteArray();
        }
    }
}
