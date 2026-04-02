using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetManager : SingLeton<NetManager>
{
    private Socket st;
    byte[] databyte = new byte[100];
    Queue<byte[]> queue = new Queue<byte[]>();
    private MyMemoryStream myStream = new MyMemoryStream();
    public void Start()
    {
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        st.BeginConnect("10.161.25.15", 10086, CounHub, null);
    }
    private void CounHub(IAsyncResult ar)
    {
        st.EndConnect(ar);
        st.BeginReceive(databyte, 0, databyte.Length, SocketFlags.None, ReceiveHandle, null);
        Debug.Log("客户端连接服务器成功");
    }
    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            int dataLen = st.EndReceive(ar);
            if (dataLen > 0)
            {
                byte[] r_Bytes = new byte[dataLen];
                Buffer.BlockCopy(databyte, 0, r_Bytes, 0, dataLen);
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
                        queue.Enqueue(tampData);
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
                st.BeginReceive(databyte, 0, databyte.Length, SocketFlags.None, ReceiveHandle, null);
            }
        }
        catch
        {
            Console.WriteLine("客户端数据解析异常");
        }
    }
    public void SendNetMessage(int netID, byte[] contextData)
    {
        // 将消息ID转换为字节数组
        // BitConverter.GetBytes：将整数转换为4字节的字节数组
        byte[] netdata = BitConverter.GetBytes(netID);
        //new一个功能ID与内容的长度的byte[]
        byte[] data = new byte[netdata.Length + contextData.Length];
        //将功能ID的byte[]写到data里
        Buffer.BlockCopy(netdata, 0, data, 0, netdata.Length);
        //将内容的byte[]写到data里(在idData的后面)
        Buffer.BlockCopy(contextData, 0, data, netdata.Length, contextData.Length);
        byte[] makeDatas = MakeData(data);
        st.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, SendHandle, null);
    }
    private byte[] MakeData(byte[] data)
    {
        //一个对象的生命周期
        using (MyMemoryStream d = new MyMemoryStream())
        {
            d.WriteUShort((ushort)data.Length);
            d.Write(data, 0, data.Length);
            return d.ToArray();
        }
    }
    private void SendHandle(IAsyncResult ar)
    {
        // 结束异步发送操作
        // ar：异步操作的结果对象
        // 如果发送失败，EndSend会抛出异常
        st.EndSend(ar);
    }
    public void Update()
    {
        while (queue.Count > 0)
        {
            byte[] data = queue.Dequeue();
            int netID = BitConverter.ToInt32(data, 0);
            byte[] makeDatas = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, makeDatas, 0, data.Length - 4);
            MessageControll.Instance.Dispatch(netID,makeDatas);
        }
    }
}
