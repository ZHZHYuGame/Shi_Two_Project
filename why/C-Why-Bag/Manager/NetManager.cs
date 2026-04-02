using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

 public class NetManager:Singleton<NetManager>
    {
        Socket st=null;
        
        /// <summary>
        /// 启动服务器
        /// </summary>
        // Start is called before the first frame update
        private byte[] receiveDataByte = new byte[1024];
        Queue<Byte[]> queue = new Queue<Byte[]>();
        MyMemoryStream myStream = new MyMemoryStream();
        public void Start()
        {
            //建立一个基于ipv4地址的Tcp流式服务
            st = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            st.BeginConnect("10.161.25.136", 10086, ConnectHandle, null);
        }
 
        private void ConnectHandle(IAsyncResult ar)
        {
            st.EndConnect(ar);
            st.BeginReceive(receiveDataByte,0,receiveDataByte.Length,SocketFlags.None,ReceiveHandle,null);
            Debug.Log("客户端连接服务器成功");
        }
 
 private void ReceiveHandle(IAsyncResult ar)
 {
     try
     {
         int dataName = st.EndReceive(ar);
         if (dataName>0)
        {
         byte[] bytes = new byte[dataName];
         Buffer.BlockCopy(receiveDataByte, 0, bytes, 0, dataName);
         myStream.Position = myStream.Length;
         myStream.Write(bytes, 0, bytes.Length);
         while (myStream.Length>2)
         {
             myStream.Position = 0;
             ushort titleLen = myStream.ReadUshort();
             int allLen = titleLen + 2;
             if(myStream.Length>=allLen)
             {
                 byte[] tempData = new byte[titleLen];
                 myStream.Read(tempData,0,tempData.Length);
                 queue.Enqueue(tempData);
                 // MessageControll.GetInstance().Dispach(netID, descByte, st, 1, "", true);
                 int shLen = (int)myStream.Length - allLen;
                 if(shLen>0)
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
}
 
             // int netId = BitConverter.ToInt32(bytes, 0);
             // MessageControl.GetInstance().Dispatch(NetID.C_To_S_Game_Net_Test_Message, netId);
          
             st.BeginReceive(receiveDataByte, 0, receiveDataByte.Length, SocketFlags.None, ReceiveHandle, null);
         }
         catch (Exception ex)
         {
             Debug.Log("错误: " + ex.Message + "\n" + ex.StackTrace);
         }
        
         
     }
     
     public void SendNetMessage(int netID,byte[] contextData)
     {
         byte[] idData = BitConverter.GetBytes(netID);
         byte[] data = new byte[idData.Length+ contextData.Length];
         Buffer.BlockCopy(idData,0,data,0,idData.Length);
         Buffer.BlockCopy(contextData,0,data,idData.Length,contextData.Length);
         byte[] makeDatas = MakeData(data);
         st.BeginSend(makeDatas,0,makeDatas.Length,SocketFlags.None,SendHandle,null);
     }
 
     private byte[] MakeData(byte[] data)
     {
         using (MyMemoryStream myMemoryStream=new MyMemoryStream())
         {
             ushort len = (ushort)data.Length;
             myMemoryStream.WriteUShort(len);
             myMemoryStream.Write(data, 0, data.Length);
             return myMemoryStream.ToArray();
         }
     }
 
     private void SendHandle(IAsyncResult ar)
     {
         try
         {
             st.EndSend(ar);
         }
         catch(Exception)
         {
              Debug.Log("发送服务器数据失败");   
         }
   
     }
 
    public void Update()
     {
         while (queue.Count>0)
         {
             Debug.Log("服务器返回一条消息");
             byte[] tempData = queue.Dequeue();
             int ID = BitConverter.ToInt32(tempData, 0);
             byte[] descByte = new byte[tempData.Length - 4];
             Buffer.BlockCopy(tempData, 4, descByte, 0, descByte.Length);
             MessageControl.GetInstance().Dispatch(ID,descByte);
         }
     }
 }

