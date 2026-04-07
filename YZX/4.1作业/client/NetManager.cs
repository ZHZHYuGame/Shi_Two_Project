using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
//using Games;
using Google.Protobuf;
/// <summary>
/// 网络管理器类 - 负责处理与服务器的TCP连接、数据发送和接收
/// 采用单例模式设计，确保整个游戏中只有一个网络管理器实例
/// </summary>
public class NetManager : Singlton<NetManager>
{
    // Socket对象，用于与服务器建立TCP连接
    Socket st;

    // 用于存储接收到的数据的缓冲区
    byte[] dataByte = new byte[100];

    /// <summary>
    /// 用来存储数据(粘包)
    /// </summary>
    private MyMemoryStream myStream = new MyMemoryStream();

    // 消息队列，用于存储从服务器接收到的消息
    // 采用队列结构，确保消息处理的顺序性
    Queue<Byte[]> queue = new Queue<byte[]>();


    /// <summary>
    /// 初始化网络连接
    /// 在游戏启动时调用，建立与服务器的TCP连接
    /// </summary>
    public void Start()
    {
        // 创建Socket对象，使用IPv4地址族，流式套接字，TCP协议
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 异步连接到本地服务器（127.0.0.1）的12345端口
        // 连接完成后会调用ConnectHandle回调函数
        st.BeginConnect("10.161.25.230", 12345, ConnectHandle, null);
    }

    /// <summary>
    /// 异步连接回调函数
    /// 当与服务器的连接建立完成后调用
    /// </summary>
    /// <param name="ar">异步操作的结果</param>
    private void ConnectHandle(IAsyncResult ar)
    {
        // 结束异步连接操作
        st.EndConnect(ar);

        // 开始异步接收数据
        // 在接收到数据时会调用ReceiveHandle回调函数
        st.BeginReceive(dataByte, 0, dataByte.Length, SocketFlags.None, ReceiveHandle, null);

        // 输出连接成功的日志
        Debug.Log("客户端连接服务器成功");
    }

    /// <summary>
    /// 异步接收数据回调函数
    /// 当从服务器接收到数据时调用
    /// </summary>
    /// <param name="ar">异步操作的结果</param>
    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            // 结束异步接收操作，获取接收到的数据长度
            int dataLen = st.EndReceive(ar);
            // 如果没有接收到数据，可能是连接已关闭
            if (dataLen == 0)
            {
                Debug.Log("服务器连接已关闭");
                return;
            }
            //接收客户端数据成功
            if (dataLen > 0)
            {
                //与客户端同步数据组成，数据拆分的结构、数据对应位置数据类型
                byte[] r_Bytes = new byte[dataLen];

                Buffer.BlockCopy(dataByte, 0, r_Bytes, 0, dataLen);
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
                        queue.Enqueue(tampData);
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
                // 继续异步接收数据，保持与服务器的通信
                st.BeginReceive(dataByte, 0, dataByte.Length, SocketFlags.None, ReceiveHandle, null);
            }
        }
        catch (System.Exception ex)
        {
            // 捕获并输出接收数据时的异常
            Debug.LogError("接受数据异常：" + ex.Message);
            throw;
        }
    }
    /// <summary>
    /// 测试全聊
    /// </summary>
    /// <param name="netID"></param>
    /// <param name="data"></param>
    public void SendNetMessage(int netID, string data = null)
    {
        // 将消息ID转换为字节数组
        byte[] netIDdata = BitConverter.GetBytes(netID);
        byte[] dataBytes = null;
        int totalLength = netIDdata.Length;
        // 如果有字符串数据，将其转换为字节数组
        if (data != null)
        {
            dataBytes = Encoding.UTF8.GetBytes(data);
            totalLength += dataBytes.Length;
        }
        // 创建一个足够大的字节数组来存储所有数据
        byte[] sendData = new byte[totalLength];
        // 复制消息ID到发送数据
        Buffer.BlockCopy(netIDdata, 0, sendData, 0, netIDdata.Length);
        // 复制字符串数据到发送数据
        if (data != null)
        {
            Buffer.BlockCopy(dataBytes, 0, sendData, netIDdata.Length, dataBytes.Length);
        }
        // 构建消息（添加消息长度）
        byte[] message = new byte[2 + sendData.Length];
        // 添加消息长度（2字节）
        byte[] lengthBytes = BitConverter.GetBytes((ushort)sendData.Length);
        Buffer.BlockCopy(lengthBytes, 0, message, 0, 2);
        // 添加消息内容
        Buffer.BlockCopy(sendData, 0, message, 2, sendData.Length);
        // 异步发送数据到服务器
        st.BeginSend(message, 0, message.Length, SocketFlags.None, SendHandle, null);
    }

    /// <summary>
    /// 发送消息到服务器
    /// 客户端发送数据（字节流），pb
    /// </summary>
    /// <param name="netID">游戏功能ID</param>
    /// <param name="data">游戏要发送的内容</param>
    public void SendNetMessage(int id, byte[] contextData)
    {

        byte[] idByte = BitConverter.GetBytes(id);
        int allLenth = idByte.Length + contextData.Length;
        byte[] allByte = new byte[allLenth];
        Buffer.BlockCopy(idByte, 0, allByte, 0, idByte.Length);
        Buffer.BlockCopy(contextData, 0, allByte, idByte.Length, contextData.Length);

        //功能ID的byte[]
        //byte[] idData = BitConverter.GetBytes(id);
        //new一个功能ID与内容的长度的byte[]
        //byte[] data = new byte[idData.Length + contextData.Length];
        //将功能ID的byte[]写到data里
        //Buffer.BlockCopy(idData, 0, data, 0, idData.Length);
        //将内容的byte[]写到data里(在idData的后面)
        //Buffer.BlockCopy(contextData, 0, data, idData.Length, contextData.Length);
        //制成完整包
        byte[] makeDatas = MakeData(allByte);
        //send message to server
        st.BeginSend(makeDatas, 0, makeDatas.Length, SocketFlags.None, Client_Send_To_Net_Handle, null);
    }


    /// <summary>
    /// 异步发送回调函数
    /// 当数据发送完成后调用
    /// </summary>
    /// <param name="ar">异步操作的结果</param>
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

    /// <summary>
    /// 流结构（1-消息体长度，2-消息主体）
    /// </summary>
    /// <param name="data"></param>
    public byte[] MakeData(byte[] data)
    {
        //一个对象的生命周期
        using (MyMemoryStream ms = new MyMemoryStream())
        {
            ms.WriteUShort((ushort)data.Length);
            ms.Write(data);
            return ms.ToArray();
        }
    }

    /// <summary>
    /// 异步发送回调函数
    /// 当数据发送完成后调用
    /// </summary>
    /// <param name="ar">异步操作的结果</param>
    private void SendHandle(IAsyncResult ar)
    {
        // 结束异步发送操作
        st.EndSend(ar);
    }

    /// <summary>
    /// 网络主线程更新方法
    /// 在Unity的主线程中调用，处理消息队列并分发消息
    /// </summary>
    public void Update()
    {
        // 处理消息队列中的所有消息
        while (queue.Count > 0)
        {
            byte[] data = queue.Dequeue();
            int netID = BitConverter.ToInt32(data, 0);
            byte[] desc = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, desc, 0, desc.Length);
            MessageControll.GetInstance().Dispatch(netID, desc);
        }
    }
}
