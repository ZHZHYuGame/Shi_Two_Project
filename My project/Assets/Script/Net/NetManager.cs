using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetManager : Singleton<NetManager>
{
    private Socket st;
    byte[] bufferByte = new byte[1024];
    private Queue<byte[]> que = new();
    private MyMemoryStream myStream = new();
    
    public void Start()
    {
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        st.BeginConnect("127.0.0.1", 22222, ConnectHandle, null);
    }

    private void ConnectHandle(IAsyncResult ar)
    {
        Debug.Log("成功链接服务器");
        st.EndConnect(ar);
        st.BeginReceive(bufferByte, 0, bufferByte.Length, SocketFlags.None, ReceiveHandle, null);
    }

    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            var len = st.EndReceive(ar);
            if (len > 0)
            {
                byte[] data = new byte[len];
                Buffer.BlockCopy(bufferByte, 0, data, 0, len);
                myStream.Position = myStream.Length;
                myStream.Write(data, 0, data.Length);

                while (myStream.Length >= 2)
                {
                    myStream.Position = 0;
                    var dataLen = myStream.ReadUshort();
                    var allLen = dataLen + 2;
                    if (allLen <= myStream.Length)
                    {
                        byte[] oneData = new byte[dataLen];
                        myStream.Read(oneData, 0, oneData.Length);
                        que.Enqueue(oneData);

                        var surplusLen = (int)myStream.Length - allLen;
                        if (surplusLen > 0)
                        {
                            byte[] surplusData = new byte[surplusLen];
                            myStream.Read(surplusData, 0, surplusData.Length);
                            myStream.Position = 0;
                            myStream.SetLength(0);
                            myStream.Write(surplusData, 0, surplusData.Length);
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
            st.BeginReceive(bufferByte, 0, bufferByte.Length, SocketFlags.None, ReceiveHandle, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public void SendMessage(int id, byte[] str)
    {
        byte[] idData = BitConverter.GetBytes(id);
        byte[] allData = new byte[str.Length + idData.Length];
        Buffer.BlockCopy(idData, 0, allData, 0, idData.Length);
        Buffer.BlockCopy(str, 0, allData, idData.Length, str.Length);
        var realData = MakeCountData(allData);
        st.BeginSend(realData, 0, realData.Length, SocketFlags.None, SendHandle, null);
    }

    private void SendHandle(IAsyncResult ar)
    {
        st.EndSend(ar);
    }

    private byte[] MakeCountData(byte[] allData)
    {
        using (MyMemoryStream m = new MyMemoryStream())
        {
            m.WriteUShort((ushort)allData.Length);
            m.Write(allData);
            return m.ToArray();
        }
    }

    public void Update()
    {
        while (que.Count > 0)
        {
            var oneData = que.Dequeue();
            int id = BitConverter.ToInt32(oneData, 0);
            byte[] dataByte = new byte[oneData.Length - 4];
            Buffer.BlockCopy(oneData, 4, dataByte, 0, dataByte.Length);
            MessageCenter.Ins.BroadCast(id, dataByte);
            Debug.Log(id);
        }
    }
}
