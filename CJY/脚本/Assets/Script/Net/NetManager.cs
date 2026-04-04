using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Games;
using Google.Protobuf;
/// <summary>
/// 客户端网络框架
/// </summary>
public class NetManager : Singleton<NetManager>
{
    Socket st;

    byte[] receiveData = new byte[100];
    /// <summary>
    /// 用来存储数据(粘包)
    /// </summary>
    private MyMemoryStream myStream = new MyMemoryStream();

    Queue<byte[]> byteQue = new Queue<byte[]>();
    public void Start()
    {
        Debug.Log(123);
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        st.BeginConnect("10.161.25.111", 10086, ConnectHandle, null);
    }

    private void ConnectHandle(IAsyncResult ar)
    {
        st.EndConnect(ar);
        //在空闲的线程中做服务器发送的数据接收处理
        st.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, ReceiveHandle, null);

        Debug.Log("客户端连接服务器成功");

    }

    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            int dataLen = st.EndReceive(ar);
            //接收客户端数据成功
            if (dataLen > 0)
            {
                //与客户端同步数据组成，数据拆分的结构、数据对应位置数据类型
                byte[] r_Bytes = new byte[dataLen];

                Buffer.BlockCopy(receiveData, 0, r_Bytes, 0, dataLen);
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
                        myStream.Read(tampData, 0, tampData.Length);
                        this.byteQue.Enqueue(tampData);

                        //int netId = BitConverter.ToInt32(tampData, 0);

                        //byte[] desc = new byte[tampData.Length - 4];
                        //Buffer.BlockCopy(tampData, 4, desc, 0, desc.Length);
                        //MessageControll.GetInstance().Dispach(netId, desc);

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

                st.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, ReceiveHandle, null);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void SendNetMessage(int netID, string data = null)
    {
        byte[] netIDdata = BitConverter.GetBytes(netID);

        st.BeginSend(netIDdata, 0, netIDdata.Length, SocketFlags.None, SendHandle, null);
    }
    /// <summary>
    /// 客户端发送数据(字节流)，PB
    /// </summary>
    /// <param name="id">游戏功能ID</param>
    /// <param name="contextData">游戏要发送的内容</param>
    public void SendNetMessage(int id, byte[] contextData)
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
        st.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, Client_Send_To_Net_Handle, null);
    }

    private void Client_Send_To_Net_Handle(IAsyncResult ar)
    {
        try
        {
            st.EndSend(ar);
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void SendHandle(IAsyncResult ar)
    {
        st.EndSend(ar);
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

    public void Update()
    {
        while (byteQue.Count > 0)
        {
            byte[] data = byteQue.Dequeue();

            int netID = BitConverter.ToInt32(data, 0);
            byte[] desc = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, desc, 0, desc.Length);
            MessageControll.Instance.Dispatch(netID, desc);
        }
    }
}
