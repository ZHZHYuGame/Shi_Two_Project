using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Script;
using UnityEngine;

public class GetData
{
    public int Int;
    public byte[] BY;
}
public class NetManager
{
    static NetManager instance;
    private byte[] dataByte = new byte[1024];
    private Queue<GetData> byteQue = new Queue<GetData>();
    private List<byte> _cacheData = new List<byte>();

    public static NetManager Instance()
    {
        if (instance == null)
        {
            instance = new NetManager();
        }

        return instance;
    }
    private Socket st;
    /// <summary>
    /// 启动服务器
    /// </summary>
    public void Start()
    {
        //三个参数，地址，消息或协议类型，网络类型
        //SocketType.Stream:双向的字节流
        //建立一个基于IPV4地址的TCP流式服务
        st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        st.BeginConnect("10.161.28.181",10085,ConnectHandle,null);
        
    }

    private void ConnectHandle(IAsyncResult ar)
    {
        st.EndConnect(ar);
        st.BeginReceive(dataByte, 0, dataByte.Length, SocketFlags.None, ReceiveHandle, null);
        Debug.Log("客户端连接服务器成功");
    }

    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            int dataLen = st.EndReceive(ar);
            if (dataLen <= 0)
            {
                Debug.Log("连接断开");
                return;
            }

            // ✅ 关键：把本次收到的数据 追加到缓存（解决半包）
            byte[] temp = new byte[dataLen];
            Buffer.BlockCopy(dataByte, 0, temp, 0, dataLen);
            _cacheData.AddRange(temp);

            // 开始解析缓存数据
            while (_cacheData.Count > 0)
            {
                // 1. 不够读长度头
                if (_cacheData.Count < 2)
                    break;

                // 2. 读取长度 count = ID(4) + 数据长度
                short count = BitConverter.ToInt16(_cacheData.ToArray(), 0);

                // 3. 整个包总长度 = 2(头) + count
                int totalPackLen = 2 + count;
                if (_cacheData.Count < totalPackLen)
                {
                    Debug.Log("✅ 真正的半包，等待下一次数据拼接");
                    break;
                }
                // 4. 取出完整的一个包
                byte[] fullPack = _cacheData.GetRange(0, totalPackLen).ToArray();
                // 5. 把这个包从缓存移除（剩下的继续留给下一次循环）
                _cacheData.RemoveRange(0, totalPackLen);

                // 6. 解析包
                Span<byte> packSpan = fullPack.AsSpan();
                packSpan = packSpan.Slice(2); // 跳过长度
                int netID = BitConverter.ToInt32(packSpan);
                packSpan = packSpan.Slice(4);
                byte[] bodyData = packSpan.ToArray();

                // 入队
                byteQue.Enqueue(new GetData()
                {
                    Int = netID,
                    BY = bodyData
                });
            }

            // 继续接收
            st.BeginReceive(dataByte, 0, dataByte.Length, SocketFlags.None, ReceiveHandle, st);
        }
        catch (Exception e)
        {
            Debug.LogError("接收异常：" + e);
        }
    }
    
    public void SendNetMessage(int netID, byte[] netStr = null)
    {
        if (netStr == null)
        {
            byte[] netIDdata = BitConverter.GetBytes(netID);
            short count = (short)(netIDdata.Length);
            byte[] countbyte = BitConverter.GetBytes(count);
            byte[] NewNet = new byte[netIDdata.Length+ countbyte.Length];
            Buffer.BlockCopy(countbyte, 0, NewNet, 0, countbyte.Length);
            Buffer.BlockCopy(netIDdata, 0, NewNet, countbyte.Length, netIDdata.Length);
            st.BeginSend(NewNet, 0, NewNet.Length, SocketFlags.None, SendHandle, null);
        }
        else
        {
            byte[] netIDdata = BitConverter.GetBytes(netID);
            short count = (short)(netIDdata.Length + netStr.Length);
            byte[] countbyte = BitConverter.GetBytes(count);
            byte[] NewNet = new byte[netIDdata.Length + netStr.Length + countbyte.Length];
            Buffer.BlockCopy(countbyte, 0, NewNet, 0, countbyte.Length);
            Buffer.BlockCopy(netIDdata, 0, NewNet, countbyte.Length, netIDdata.Length);
            Buffer.BlockCopy(netStr, 0, NewNet, netIDdata.Length + countbyte.Length, netStr.Length);
            st.BeginSend(NewNet, 0, NewNet.Length, SocketFlags.None, SendHandle, null);
        }
    }
    

    private void SendHandle(IAsyncResult ar)
    {
        st.EndSend(ar);
    }

    public void MakeData(byte[] data)
    {
        
    }

    public void Update()
    {
        while (byteQue.Count > 0)
        { 
            GetData data=byteQue.Dequeue();
           MessageControll.Instance().Dispatch(data.Int,data.BY);
        }
    }
}
