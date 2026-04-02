using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


    public class NetManager:Singlton<NetManager>
    {
        //服务器监听
        Socket st;
        //缓冲区
        byte[] receiveDateByte=new byte[1024];
        //当前已连接客户端列表
        List<Client> clientsList = new List<Client>();

        /// <summary>
        /// 用来存储数据(粘包)
        /// </summary>
        private MyMemoryStream myStream = new MyMemoryStream();
        /// <summary>
        /// 启动服务器
        /// </summary>
        public void Start()
        {
            //建立一个基于IPv4地址的TCP流式服务
             st = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //设置一个接受任何一个可以接受客户端访问，并设置端口号      IP地址和端口号
            st.Bind(new IPEndPoint(IPAddress.Parse("10.161.25.230"), 12345));
            //设定服务器接受客户端访问的上限
            st.Listen(10);

            Console.WriteLine("服务器启动成功");
            //接受客户端访问处理
            st.BeginAccept(AcceptHandle,null);
        
        }
        /// <summary>
        /// 接受客户端访问处理
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptHandle(IAsyncResult ar)
        {
           //处理链接进来的客户端
           Socket clientSocket=st.EndAccept(ar);
            //获取客户端的ip地址和端口号
            IPEndPoint ip = clientSocket.RemoteEndPoint as IPEndPoint;
            //记录客户端数据
            Client c=new Client()
            {
                st=clientSocket,
                ip=ip.Address.ToString(),
                port=ip.Port,
                userData=new UserData()
            };
            //c.userData.userBaseData = new UserBaseData();
            clientsList.Add(c);
            
            UserManager.GetInstance().AddClient(c);

            Console.WriteLine($"连接进来的客户端ip:{ip.Address}---端口号:{ip.Port}");
            //接受客户端发送过来的行为数据
            clientSocket.BeginReceive(receiveDateByte, 0,receiveDateByte.Length, SocketFlags.None, ReceiveHandle, c);

            //接受客户端访问处理--服务器持续监听新客户端
            st.BeginAccept(AcceptHandle, null);
        }
        /// <summary>
        /// 接受客户端发送过来的行为数据
        /// 1.接受单位为字节流Byte
        /// 2.是客户端的行为触发 以数据的形式发送
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveHandle(IAsyncResult ar)
        {
            // 从异步状态中获取客户端对象
            // AsyncState存储的是BeginReceive时传递的Client对象
            Client client = ar.AsyncState as Client;
            if (client == null)return;
            try
            {
                int dataLen = client.st.EndReceive(ar);
                //接收客户端数据成功
                if (dataLen > 0)
                {
                    //与客户端同步数据组成，数据拆分的结构、数据对应位置数据类型
                    byte[] r_Bytes = new byte[dataLen];

                    Buffer.BlockCopy(receiveDateByte, 0, r_Bytes, 0, dataLen);
                    //如有剩余未处理的包，则在包的后面进入写入
                    myStream.Position = myStream.Length;
                    //数据已经存进来了
                    myStream.Write(r_Bytes, 0, r_Bytes.Length);
                    //判断是不是到少有一个不完整的包(为什么？因为还没到判断完整包的地方)
                    while (myStream.Length >= 2)
                    {
                        //现在位置在写入数据的长度的位置
                        myStream.Position = 0;
                        //包头的值 = 包体的长度
                        ushort titleLen = myStream.ReadUshort();
                        //包的整体长度
                        int allLen = titleLen + 2;
                        //这里才是判断是不是有一个可以处理的完整的包
                        if (myStream.Length >= allLen)
                        {
                            //这里已经开始读消息的内容(id + 内容)
                            byte[] tampData = new byte[titleLen];
                            //从内存流中读取包体数据 到 tampData 字节数组中。
                            myStream.Read(tampData, 0, tampData.Length);
                            //读取四个字节转换成intID
                            int netID = BitConverter.ToInt32(tampData, 0);
                            //准备存储消息内容
                            byte[] descByte = new byte[tampData.Length - 4];
                            //从 tampData 的第4个字节开始，复制数据到 descByte 数组
                            Buffer.BlockCopy(tampData, 4, descByte, 0, descByte.Length);
                            //将整个 tampData 字节数组（包含消息ID和内容）转换为UTF-8编码的字符串
                            string data=Encoding.UTF8.GetString(descByte);
                            Console.WriteLine(client.port+":"+data);
                            //给每个链接的客户端发送消息
                            //BroadcastMessage( Encoding.UTF8.GetString(descByte));
                            
                            MessageControll.GetInstance().Dispach(netID, descByte, client, 1, "", true);
                            //SendMessage(netID, descByte, client.st);
                            int shLen = (int)myStream.Length - allLen;
                            //还有未处理完的数据包
                            if (shLen > 0)
                            {
                                //存剩余数据
                                byte[] shData = new byte[shLen];
                                myStream.Read(shData, 0, shData.Length);
                                //请空流
                                myStream.Position = 0;
                                myStream.SetLength(0);
                                //将剩余的数据写到缓冲区
                                myStream.Write(shData, 0, shData.Length);
                            }
                            else
                            {
                                //请空流
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
                    client.st.BeginReceive(receiveDateByte, 0, receiveDateByte.Length,
                SocketFlags.None, ReceiveHandle, client);
                }
            }
            catch (Exception ex)
            {
                // 从客户端列表中移除断开连接的客户端
                if (client != null)
                {
                    clientsList.Remove(client);
                    Console.WriteLine($"客户端 {client.ip}:{client.port} 断开连接");
                }

                Console.WriteLine("客户端数据解析异常: " + ex.Message);
            }
        }
        /// <summary>
        /// 给每个链接的客户端发送
        /// </summary>
        /// <param name="content"></param>
        public void BroadcastMessage(string content)
        {
            byte[] contentBytes=Encoding.UTF8.GetBytes(content);
            foreach (var item in clientsList)
            {
                try
                {
                //SendMessage( contentBytes, item.st);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"发送消息异常: {ex.Message}");
                }
                
            }
        }
        
    

        /// <summary>
        /// 服务器向服务器发送数据(字节流)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contextData"></param>
        /// <param name="st"></param>
       public void SendMessage(Socket c_Socket,int id, byte[] contextData)
        {
            //功能ID的byte[]
            byte[] idData = BitConverter.GetBytes(id);

            //new一个功能ID与内容的长度的byte[]
            byte[] data = new byte[idData.Length + contextData.Length];

            //将功能ID的byte[]写到data里
            Buffer.BlockCopy(idData, 0, data, 0, idData.Length);

            //将内容的byte[]写到data里(在idData的后面)
            Buffer.BlockCopy(contextData, 0, data, idData.Length, contextData.Length);
            //
            byte[] makeDatas = MakeData(data);
            //send message to server
            c_Socket.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, SynSend, c_Socket);
        }

        /// <summary>
        /// 转换？(发送的内容转换成包头与包体格式，为什么？处理粘包)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] MakeData(byte[] data)
        {
            //using 决定作用域里的实例的生命周期，运行结束后自行清除
            using (MyMemoryStream myStream = new MyMemoryStream())
            {
                ushort len = (ushort)data.Length;
                myStream.WriteUShort(len);
                myStream.Write(data, 0, data.Length);
                return myStream.ToArray();
            }
        }

        /// <summary>
        /// 发送数据结束
        /// </summary>
        /// <param name="ar"></param>
        private void SynSend(IAsyncResult ar)
        {
            Socket st = ar.AsyncState as Socket;
            try
            {
                st.EndSend(ar);
            
        }
            catch (Exception)
            {

            }
        }
    }

