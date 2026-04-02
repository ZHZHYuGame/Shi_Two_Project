using System.Net;
using System.Net.Sockets;

namespace BagNet.Net;

public class NetManger:SingLeton<NetManger>
{
    Socket st;

byte[] receiveDataByte = new byte[1024];
private MyMemoryStream myStream = new MyMemoryStream();

List<Client> clientsList = new List<Client>();

public void Start()
{
    st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    st.Bind(new IPEndPoint(IPAddress.Any, 10086));
    st.Listen(10);
    Console.WriteLine("服务器启动成功");
    st.BeginAccept(AcceptHandle, null);
}

private void AcceptHandle(IAsyncResult ar)
{
    Socket clientSocket = st.EndAccept(ar);
    IPEndPoint ip=clientSocket.RemoteEndPoint as IPEndPoint;
    Client c = new Client()
    {
        st =clientSocket,
        ip = ip.Address.ToString(),
        port = ip.Port
    };
    UserManger.GetInstance.AddClientUser(c);
    Console.WriteLine($"连接进来的客户端IP = {ip.Address}  端口号 = {ip.Port}");
    c.st.BeginReceive(receiveDataByte, 0, receiveDataByte.Length, SocketFlags.None,clientReceiveHandle,c);
    st.BeginAccept(AcceptHandle, null);
}

private void clientReceiveHandle(IAsyncResult ar)
{
    try
    {
        Client client = ar.AsyncState as Client;
        int dataLen = client.st.EndReceive(ar);
        if (dataLen > 0)
        {
            byte[] r_Bytes = new byte[dataLen];
            Buffer.BlockCopy(receiveDataByte, 0, r_Bytes, 0, dataLen);
            myStream.Position = myStream.Length;
            myStream.Write(r_Bytes, 0, dataLen);
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
                    myStream.Read(tampData, 0, tampData.Length);

                    int netID = BitConverter.ToInt32(tampData, 0);
                    //内容
                    byte[] descByte = new byte[tampData.Length - 4];
                    Buffer.BlockCopy(tampData, 4, descByte, 0, descByte.Length);
                    MessageControll.GetInstance.DisPatch(netID, descByte, client, 1, "", true);

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
            client.st.BeginReceive(receiveDataByte, 0, receiveDataByte.Length, SocketFlags.None, clientReceiveHandle, client);
        }
        else
        {
            Console.WriteLine($"客户端 {client.port} 断开连接");
            UserManger.GetInstance.RemoveClientUser(client);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"客户端异常断开: {e.Message}");
        Client client = ar.AsyncState as Client;
        if (client != null)
        {
            UserManger.GetInstance.RemoveClientUser(client);
        }
    }
}

public void SendNetMessage(Socket c_Socket, int id, byte[] contextData)
{
    Console.WriteLine(id);
    byte[] idData=BitConverter.GetBytes(id);
    byte[] data=new byte[idData.Length + contextData.Length];
    Buffer.BlockCopy(idData, 0, data, 0, idData.Length);
    Buffer.BlockCopy(contextData, 0, data, idData.Length, contextData.Length);
    byte[] makeDatas = MakeData(data);
    c_Socket.BeginSend(makeDatas,0,makeDatas.Length,SocketFlags.None,Client_Send_To_Net_Handle,c_Socket);
}

private void Client_Send_To_Net_Handle(IAsyncResult ar)
{
    try
    {
        Socket st=ar.AsyncState as Socket;
        st.EndSend(ar);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

/// <summary>
/// 流结构（1-消息体长度，2-消息主体）
/// </summary>
/// <param name="data"></param>
public byte[] MakeData(byte[] data)
{
    //一个对象的生命周期
    using (MyMemoryStream d = new MyMemoryStream())
    {
        d.WriteUShort((ushort)data.Length);
        d.Write(data, 0, data.Length);
        return d.ToArray();
    }
}
}