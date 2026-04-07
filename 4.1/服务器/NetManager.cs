using System.Net;
using System.Net.Sockets;

namespace S;

public class NetManager : Singleton<NetManager>
{
    private Socket st;
    byte[] bufferByte = new byte[1024];
    MyMemoryStream myStream = new MyMemoryStream();
    
    public void Start()
    {
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        st.Bind(new IPEndPoint(IPAddress.Any, 22222));
        st.Listen(10);
        st.BeginAccept(AcceptHandle,null);
        Console.WriteLine("服务器启动");
    }

    private void AcceptHandle(IAsyncResult ar)
    {
        var clientSt = st.EndAccept(ar);
        var IPE = clientSt.RemoteEndPoint as IPEndPoint;

        Client client = new();
        client.st = clientSt;
        client.ip = IPE.Address.ToString();
        client.port = IPE.Port;
        UserManager.Ins.Add(client);
        Console.WriteLine(client.port + "链接到服务器");
        
        clientSt.BeginReceive(bufferByte, 0, bufferByte.Length, SocketFlags.None, ReceiveHandle, client);
        st.BeginAccept(AcceptHandle,null);
    }

    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            var clientSt = ar.AsyncState as Client;
            var len = clientSt.st.EndReceive(ar);
            if (len > 0)
            {
                byte[] allData = new byte[len];
                Buffer.BlockCopy(bufferByte, 0, allData, 0, len);
                myStream.Position = myStream.Length;
                myStream.Write(allData, 0, len);

                while (myStream.Length >= 2)
                {
                    myStream.Position = 0;
                    var dataLen = myStream.ReadUshort();
                    var allLen = dataLen + 2;
                    
                    if (myStream.Length >= allLen)
                    {
                        byte[] idData = new byte[dataLen];
                        myStream.Read(idData, 0, idData.Length);
                        int id = BitConverter.ToInt32(idData, 0);
                        byte[] data = new byte[dataLen - 4];
                        Buffer.BlockCopy(idData, 4, data, 0, dataLen - 4);
                        MessageCenter.Ins.BroadCast(id,clientSt,data);
                        var surplusLen = (int)myStream.Length - allLen;
                        if (surplusLen > 0)
                        {
                            byte[] surplus = new byte[surplusLen];
                            myStream.Read(surplus, 0, surplus.Length);
                            myStream.Position = 0;
                            myStream.SetLength(0);
                            myStream.Write(surplus, 0, surplus.Length);
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
            }
            clientSt.st.BeginReceive(bufferByte, 0, bufferByte.Length, SocketFlags.None, ReceiveHandle, clientSt);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public void SendMessage(Socket s ,int id, byte[] data)
    {
        byte[] idData = BitConverter.GetBytes(id);
        byte[] allData = new byte[idData.Length + data.Length];
        Buffer.BlockCopy(idData, 0, allData, 0, idData.Length);
        Buffer.BlockCopy(data, 0, allData, idData.Length, data.Length);
        byte[] realData = MakeCountData(allData);
        Console.WriteLine("消息id"+id);
        s.BeginSend(realData,0,realData.Length,SocketFlags.None,SendHandle,s);
    }

    private void SendHandle(IAsyncResult ar)
    {
        var s = ar.AsyncState as Socket;
        s.EndSend(ar);
    }

    byte[] MakeCountData(byte[] data)
    {
        using (MyMemoryStream m = new ())
        {
            m.WriteUShort((ushort)data.Length);
            m.Write(data);
            return m.ToArray();
        }
    }
}