using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetManager:SingLeton<NetManager>
{
    Socket socket;
    byte[] receiveDataByte = new byte[1024];
    List<Client> clientsList = new List<Client>();
    private MyMemoryStream myStream = new MyMemoryStream();
    public void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Parse("10.161.25.15"), 10086));
        socket.Listen(5);
        Console.WriteLine("服务器开启");
        socket.BeginAccept(AcceptHandle, null);
    }
    private void AcceptHandle(IAsyncResult ar)
    {
        Socket clientsocket = socket.EndAccept(ar);
        IPEndPoint ip = clientsocket.RemoteEndPoint as IPEndPoint;

        Client client = new Client()
        {
            st = clientsocket,
            ip = ip.Address.ToString(),
            port = ip.Port,
        };
        UserManager.Getinstance().AddClient(client);

        Console.WriteLine($"链接进来的客户端IP={ip.Address} 端口号={ip.Port}");
        //接收客户端发送过来的行为数据
        clientsocket.BeginReceive(receiveDataByte, 0, receiveDataByte.Length, SocketFlags.None, clientReveive, client);
        //再次唤醒接收客户端访问
        socket.BeginAccept(AcceptHandle, null);
    }

    private void clientReveive(IAsyncResult ar)
    {
        try
        {
            Client clientSocket = ar.AsyncState as Client;
            int dataLen = clientSocket.st.EndReceive(ar);
            if (dataLen > 0)
            {
                byte[] r_Bytes = new byte[dataLen];
                Buffer.BlockCopy(receiveDataByte, 0, r_Bytes, 0, dataLen);
                myStream.Position = myStream.Length;
                myStream.Write(r_Bytes, 0, r_Bytes.Length);
                while (myStream.Length >= 2)
                {
                    myStream.Position = 0;
                    ushort titleLen = myStream.ReadUshort();
                    int allLen = titleLen + 2;
                    if (myStream.Length >= allLen)
                    {
                        byte[] tampData = new byte[titleLen];
                        myStream.Read(tampData, 0, tampData.Length);
                        int netID = BitConverter.ToInt32(tampData, 0);
                        byte[] descByte = new byte[tampData.Length - 4];
                        Buffer.BlockCopy(tampData, 4, descByte, 0, descByte.Length);
                        MessageControll.Getinstance().Dispatch(netID, descByte, clientSocket, 1, "", true);

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
                clientSocket.st.BeginReceive(receiveDataByte, 0, receiveDataByte.Length, SocketFlags.None, clientReveive, clientSocket);
            }
            else
            {
                
            }
        }
        catch
        {
            Console.WriteLine("客户端数据解析异常");
        }
    }
    public void SendNetMessage(int id, Socket Cst, byte[] contextData)
    {
        // 将整型消息 ID 序列化为 4 字节数据。
        byte[] idData = BitConverter.GetBytes(id);
        //new一个功能ID与内容的长度的byte[]
        byte[] data = new byte[idData.Length + contextData.Length];
        //将功能ID的byte[]写到data里
        Buffer.BlockCopy(idData, 0, data, 0, idData.Length);
        //将内容的byte[]写到data里(在idData的后面)
        Buffer.BlockCopy(contextData, 0, data, idData.Length, contextData.Length);
        byte[] makeDatas = MakeData(data);
        Console.WriteLine("发消息");
        //异步发送 非阻塞 前三个参数的意思是量makeDatas全部发过去
        Cst.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, TestSendHandle, Cst);
    }
    void TestSendHandle(IAsyncResult ar)//异步发送的结果
    {
        Socket st = ar.AsyncState as Socket;
        st.EndSend(ar);//释放资源避免资源泄漏
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
}