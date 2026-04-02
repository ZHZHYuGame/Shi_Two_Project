using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class NetManger :SingLeton<NetManger>
{
    Socket st;
    byte[] receiveData=new byte[100];
    private MyMemoryStream myStream = new MyMemoryStream();
    Queue<byte[]> byteQue = new Queue<byte[]>();
    // Start is called before the first frame update
    public void Start()
    {
        st=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        st.BeginConnect("127.0.0.1", 10086, ConnectHandle, null);
    }

    private void ConnectHandle(IAsyncResult ar)
    {
        st.EndConnect(ar);
        st.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, ReceiveHandle,null);
    }

    private void ReceiveHandle(IAsyncResult ar)
    {
        try
        {
            int dataLen = st.EndReceive(ar);
         
            if (dataLen > 0)
            {
                byte[] r_Bytes = new byte[dataLen];

                Buffer.BlockCopy(receiveData, 0, r_Bytes, 0, dataLen);
             
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
                        this.byteQue.Enqueue(tampData);
                       
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

                st.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, ReceiveHandle, null);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void SendMessage(int netID, string data = null)
    {
        byte[] netIDdata=BitConverter.GetBytes(netID);
        byte[] qs=Encoding.UTF8.GetBytes(data);
        byte[] newData=new byte[qs.Length + netIDdata.Length];
        Buffer.BlockCopy(netIDdata, 0, newData, 0, netIDdata.Length);
        Buffer.BlockCopy(qs, 0, newData, netIDdata.Length, qs.Length);
        byte[] makeData = MakeData(newData);
        st.BeginSend(makeData,0,makeData.Length,SocketFlags.None,SendHandle,null);
    }

    public void SendMessage(int netID, byte[] contextData)
    {
        byte[] netIDdata=BitConverter.GetBytes(netID);
        byte[] data=new byte[netIDdata.Length+contextData.Length];
        Buffer.BlockCopy(netIDdata,0,data,0,netIDdata.Length);
        Buffer.BlockCopy(contextData,0,data,netIDdata.Length,contextData.Length);
        byte[] makeDatas = MakeData(data);
        st.BeginSend(makeDatas,0,makeDatas.Length,SocketFlags.None,SendHandle,null);
    }
    private void SendHandle(IAsyncResult ar)
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
    public byte[] MakeData(byte[] data)
    {
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
            Debug.Log($"收到消息 ID: {netID}, 数据长度: {data.Length}");
            byte[] desc = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, desc, 0, desc.Length);
            MessageControll.GetInstance.DisPatch(netID, desc);
        }
    }
}
