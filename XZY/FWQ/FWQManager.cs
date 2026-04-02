using FWQ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
public class FWQManager 
{
    private Socket st;
    byte[] receiveDataByte=new byte[1024];
    public List<Client> clientsList=new List<Client>();
    static FWQManager instance;
    public static FWQManager Instance()
    {
        if(instance == null)
        {
            instance = new FWQManager();
        }
        return instance;
    }
    
    /// <summary>
    /// 启动服务器
    /// </summary>
    public void Start()
    {
        //三个参数，地址，消息或协议类型，网络类型
        //SocketType.Stream:双向的字节流
        //建立一个基于IPV4地址的TCP流式服务
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //设定一个接收任何一个可以接受客户端访问，并设置端口号
        st.Bind(new IPEndPoint(IPAddress.Any,10085));
        //设定服务器接受客户端访问数量
        Console.WriteLine("连接服务器成功");
        st.Listen(5);
        //接受客户端访问处理
        st.BeginAccept(AcceptHandle,null);
        
    }
    /// <summary>
    /// 接受客户端访问处理
    /// </summary>
    /// <param name="ar"></param>

    private void AcceptHandle(IAsyncResult ar)
    {
        //处理连接进来的客户端
        Socket clientSocket = st.EndAccept(ar);
        IPEndPoint ip = (clientSocket.RemoteEndPoint as IPEndPoint);
       //记录客户端的数据
        Client c = new Client()
        {
            st=clientSocket,
            ip=ip.Address.ToString(),
            port=ip.Port,
        };
        clientsList.Add(c);
        Console.WriteLine("连接进来的客户端:"+ip.Address+"端口号是:"+ip.Port);
        Mail.instance.Start(c);
        BagMgr.instance.Start(c);
        st.BeginAccept(AcceptHandle,null);
        //接收客户端发送过来的行为数据
        clientSocket.BeginReceive(receiveDataByte,0,receiveDataByte.Length, SocketFlags.None, clientReceiveHandle,c);
    }
    /// <summary>
    /// 接收客户端发送过来的行为数据
    /// 1.接收单位为字节流Byte
    /// 2.是客户端的行为触发以数据的形式发送
    /// </summary>
    /// <param name="ar"></param>
    /// <exception cref="NotImplementedException"></exception>

    private void clientReceiveHandle(IAsyncResult ar)
    {
        try
        {
            
            Client client=ar.AsyncState as Client;
            Socket clientSocket = client.st;
            //int Num=clientSocket.R
            //安全处理解析客户端的数据
            int dataNums = clientSocket.EndReceive(ar);
            if (dataNums == 0)
            {
                Console.WriteLine($"端口:{client.port}退出");
                clientSocket.Close();
                clientsList.Remove(client);
                return;
            }
            byte[] cDataBtye=new byte[dataNums];
            Buffer.BlockCopy(receiveDataByte, 0, cDataBtye, 0, dataNums);
            Span<byte> buffer =cDataBtye.AsSpan();
            
            while (buffer.Length > 0)
            {
                //半包
                if (buffer.Length<2)
                {
                    Console.WriteLine("读取了一个长度都没有的包,已退出");
                    break;
                }
                short shor = BitConverter.ToInt16(buffer);
                //半包，不完整，不读
                if(shor>buffer.Length-2)
                {
                    Console.WriteLine("读取了一个半包,已退出");
                    break;
                }
                buffer=buffer.Slice(2);
                int netID = BitConverter.ToInt32(buffer);
                buffer = buffer.Slice(4);
                var item = buffer.Slice(0, shor - 4);
                byte[] data = null;
                if (item != null)
                {
                    data=item.ToArray();
                }
                MessageControll.Instance().Dispatch(netID, data, client);
                buffer=buffer.Slice(shor-4);
            }
            clientSocket.BeginReceive(receiveDataByte, 0, receiveDataByte.Length, SocketFlags.None, clientReceiveHandle, client);
        }
        catch(Exception)
        {
            //异常捕捉
            Console.WriteLine("服务器数据解析异常");
        }
    }

    public void SendNetMessage(Socket st,int id, byte[] data)
    {
        byte[] idData = BitConverter.GetBytes(id);
        short count = (short)(idData.Length + data.Length);
        byte[] countbyte = BitConverter.GetBytes(count);
        byte[] newby = new byte[data.Length + idData.Length + countbyte.Length];
        Buffer.BlockCopy(countbyte, 0, newby, 0, countbyte.Length);
        Buffer.BlockCopy(idData, 0, newby, countbyte.Length, idData.Length);
        Buffer.BlockCopy(data, 0, newby, idData.Length + countbyte.Length, data.Length);
        st.BeginSend(newby, 0, newby.Length, SocketFlags.None, TestSendHandle, st);
    }


    private void TestSendHandle(IAsyncResult ar)
    {
        Socket st = ar.AsyncState as Socket;
        st.EndSend(ar);
    }
}
