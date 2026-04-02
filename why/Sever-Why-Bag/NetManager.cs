using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetBag
{
    public class NetManager : Singleton<NetManager>
    {
        Socket st;
        MyMemoryStream myStream = new MyMemoryStream();
        byte[] reciveDataByte = new byte[1024];
        public List<Client> userDic = new List<Client>();

        /// <summary>
        /// 启动服务器
        /// </summary>
        // Start is called before the first frame update
        public void Start()
        {
            //建立一个基于ipv4地址的Tcp流式服务
            st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //设定一个接收任何一个可以接受客户端访问，并设置端口号
            st.Bind(new IPEndPoint(IPAddress.Parse("10.161.25.136"), 10086));
            //设置服务器接受客户端的访问数量
            st.Listen(5);
            Console.WriteLine("服务器启动成功");
            //接受客户端的访问处理
            st.BeginAccept(AcceptHandle, null);


        }
        /// <summary>
        /// 接受客户端访问处理
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptHandle(IAsyncResult ar)
        {
            //处理连接过来的客户端
            Socket clientSocket = st.EndAccept(ar);
            IPEndPoint ip = clientSocket.RemoteEndPoint as IPEndPoint;
            Client c = new Client()
            {
                st = clientSocket,
                ip = ip.Address.ToString(),
                port = ip.Port,
                user = new UserData()
                {
                    gold = 10000,
                    masonry = 10000,
                    roll = 10,
                    energy = 10000,
                    EndBagCount = 240,
                    MaxBagCount = 100,
                    NowBagCount = 0,
                    GetBagCellCount= 0,
                }
            };
            UserManager.GetInstance().UserData = c;
            Console.WriteLine($"连进来的客户端IP = {ip.Address}端口号 ={ip.Port}");
            //接受客户端返送过来的数据行为
            clientSocket.BeginReceive(reciveDataByte, 0, reciveDataByte.Length, SocketFlags.None, clientReceiveHandle, c);
            //接受客户端访问处理一多线程的再次唤醒
            st.BeginAccept(AcceptHandle, null);
        }
        private void clientReceiveHandle(IAsyncResult ar)
        {
            try
            {
                Client client = ar.AsyncState as Client;
                //安全处理解析客户端数据
                int dataNums = client.st.EndReceive(ar);

                if (dataNums > 0)
                {
                    byte[] cData = new byte[dataNums];
                    Buffer.BlockCopy(reciveDataByte, 0, cData, 0, dataNums);
                    myStream.Position = myStream.Length;
                    myStream.Write(cData, 0, cData.Length);
                    while (myStream.Length > 2)
                    {
                        myStream.Position = 0;
                        ushort titleLen = myStream.ReadUshort();
                        int allLen = titleLen + 2;
                        if (myStream.Length >= allLen)
                        {
                            byte[] tempData = new byte[titleLen];
                            myStream.Read(tempData, 0, tempData.Length);

                            int netID = BitConverter.ToInt32(tempData, 0);
                            byte[] descByte = new byte[tempData.Length - 4];
                            Buffer.BlockCopy(tempData, 4, descByte, 0, descByte.Length);
                            MessageControll.GetInstance().Dispach(netID, descByte, client, 1, "", true);

                            int shLen = (int)myStream.Length - allLen;
                            if (shLen > 0)
                            {
                                byte[] shData = new byte[shLen];
                                myStream.Read(shData, 0, shData.Length);
                                myStream.Position = 0;
                                myStream.SetLength(0);
                                myStream.Write(shData, 0, shData.Length);
                            }
                            else
                            {
                                myStream.Position = 0;
                                myStream.SetLength(0);
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    client.st.BeginReceive(reciveDataByte, 0, reciveDataByte.Length, SocketFlags.None, clientReceiveHandle, client);
                }
            }
            catch (Exception)
            {
                //异常捕捉：解析数据异常报错监听
                Console.WriteLine("客户端数据解析异常");
            }
        }
        public void SendNetMessage(Socket sts, int netID, byte[] contextdata)
        {
            byte[] idData = BitConverter.GetBytes(netID);
            byte[] data = new byte[idData.Length + contextdata.Length];
            Buffer.BlockCopy(idData, 0, data, 0, idData.Length);
            Buffer.BlockCopy(contextdata, 0, data, idData.Length, contextdata.Length);
            byte[] makeDatas = MakeData(data);
            sts.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, SendHandler, sts);
        }

        private byte[] MakeData(byte[] data)
        {
            using (MyMemoryStream myMemoryStream = new MyMemoryStream())
            {
                ushort len = (ushort)data.Length;
                myMemoryStream.WriteUShort(len);
                myMemoryStream.Write(data, 0, data.Length);
                return myMemoryStream.ToArray();
            }
        }

        void SendHandler(IAsyncResult ar)
        {
            try
            {
                Socket sts = (Socket)ar.AsyncState;
                sts.EndSend(ar);
            }
            catch
            {

            }
        }
    } 
}
