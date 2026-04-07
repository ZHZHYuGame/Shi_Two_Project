using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Games;
using Google.Protobuf;
/// <summary>
/// 聊天管理器
/// </summary>
public class ChatManager:Singleton<ChatManager>
{
    
    public void Start()
    {
        MessageControll.GetInstance().AddListener(NetID.C_To_S_Chat_World_Message, C_To_S_Chat_World_Message_Handle);
    }
    /// <summary>
    /// 服务器处理客户端世界聊天结果
    /// </summary>
    /// <param name="obj"></param>
    private void C_To_S_Chat_World_Message_Handle(object obj)
    {
        object[] objList = obj as object[];
        byte[] data = objList[0] as byte[];
        Client c = objList[1] as Client;

        C_To_S_Chat_World_Message c_Data = C_To_S_Chat_World_Message.Parser.ParseFrom(data);
        S_To_C_Chat_Message s_Data = new S_To_C_Chat_Message();
        switch (c_Data.Type)
        {
            case ChatType.World:    //公聊
                //转发给其他客户端的聊天类型
                s_Data.ChatType = c_Data.Type;
                //转发给其他客户端的说话者
                s_Data.Speaker = c.port.ToString();
                //转发给其他客户端的聊天内容
                s_Data.TextDesc = c_Data.TextDesc;
                foreach (var user in UserManager.GetInstance().userDict)
                {
                    NetManager.GetInstance().SendNetMessage(user.Value.st, NetID.S_To_C_Chat_World_Message, s_Data.ToByteArray());
                }
                break;
            case ChatType.Private:
               
                break;
            case ChatType.Teat:
                break;
            default:
                break;
        }
    }
}
