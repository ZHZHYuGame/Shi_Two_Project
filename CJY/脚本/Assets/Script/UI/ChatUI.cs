using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Games;
using Google.Protobuf;

public class ChatUI : MonoBehaviour
{
    public Text textDesc = null;
    public RectTransform chatWorldPanel = null, chatPrivatePanel = null;
    public ScrollRect scrollRect = null;
    public Button WorldBtn = null, PrivateBtn = null;
    public InputField Iftxt = null;
    public Dropdown dropdown = null;

    string worldDescCache;
    string privateDescCache;
    string teamDescCache;

    // Start is called before the first frame update
    void Start()
    {
        MessageControll.Instance.AddListener(NetID.S_To_C_Chat_World_Message, S_To_C_Chat_World_Message_Handle);
        MessageControll.Instance.AddListener(NetID.S_To_C_Chat_Private_Message, S_To_C_Chat_World_Message_Handle);
        WorldBtn.onClick.AddListener(WorldChat);
        PrivateBtn.onClick.AddListener(PrivateChat);
        Iftxt.onEndEdit.AddListener(InputFieldEnd);
        //dropdown.ClearOptions();
        //dropdown.AddOptions(Manager.Instance.userIDs);
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        Debug.Log("选中了索引: " + index + "，文本: " + dropdown.options[index].text);
    }

    private void InputFieldEnd(string str)
    {
        C_To_S_Chat_World_Message c_Data = new C_To_S_Chat_World_Message();
        //c_Data.Type = ChatType.World;
        c_Data.Type = Manager.Instance.chatType;
        c_Data.TextDesc = Iftxt.text;
        //c_Data.s = UserManager.Instance.userID.ToString();
        if (Manager.Instance.chatType == ChatType.World)
        {
            NetManager.Instance.SendNetMessage(NetID.C_To_S_Chat_World_Message, c_Data.ToByteArray());
        }
        else if (Manager.Instance.chatType == ChatType.Private)
        {
            NetManager.Instance.SendNetMessage(NetID.C_To_S_Chat_Private_Message, c_Data.ToByteArray());
        }
        Iftxt.text = string.Empty;
    }

    private void WorldChat()
    {
        Manager.Instance.chatType = ChatType.World;
        chatWorldPanel.gameObject.SetActive(true);
        dropdown.gameObject.SetActive(false);
        chatPrivatePanel.gameObject.SetActive(false);
        scrollRect.content = chatWorldPanel;
    }

    private void PrivateChat()
    {
        Manager.Instance.chatType = ChatType.Private;
        chatWorldPanel.gameObject.SetActive(false);
        dropdown.gameObject.SetActive(true);
        chatPrivatePanel.gameObject.SetActive(true);
        scrollRect.content = chatPrivatePanel;
    }

    /// <summary>
    /// 客户端处理服务器回馈的公聊处理结果
    /// </summary>
    /// <param name="obj"></param>
    private void S_To_C_Chat_World_Message_Handle(object obj)
    {
        object[] objList = obj as object[];
        byte[] data = objList[0] as byte[];

        S_To_C_Chat_World_Message s_Data = S_To_C_Chat_World_Message.Parser.ParseFrom(data);

        switch (s_Data.ChatType)
        {
            case ChatType.World:
                //如果是自己说
                if (UserManager.Instance.userID.ToString() == s_Data.Speaker)
                {
                    //textDesc.text = $"<color=#000000>[世界]我对所有人说：{s_Data.TextDesc}</color>";
                    Text text = Instantiate(Resources.Load<Text>("Text"), chatWorldPanel);
                    text.text = $"<color=#000000>[世界]我对所有人说：{s_Data.TextDesc}</color>";
                }
                else
                {
                    //如果是别人说
                    //textDesc.text = $"<color=#000000>[世界]{s_Data.Speaker}对所有人说：{s_Data.TextDesc}</color>";
                    Text text = Instantiate(Resources.Load<Text>("Text"), chatWorldPanel);
                    text.text = $"<color=#000000>[世界]{s_Data.Speaker}对所有人说：{s_Data.TextDesc}</color>";
                }
                break;
            case ChatType.Private:
                if (UserManager.Instance.userID.ToString() == s_Data.Speaker)
                {
                    //textDesc.text = $"<color = #000000>[私聊]我对{s_Data.ToSpeaker}说：{s_Data.TextDesc}</color>";
                    Text text = Instantiate(Resources.Load<Text>("Text"), chatPrivatePanel);
                    text.text = $"<color=#000000>[私聊]我对{s_Data.Speaker}说：{s_Data.TextDesc}</color>";
                }
                else
                {
                    //textDesc.text = $"<color = #000000>[私聊]{s_Data.Speaker}对我说：{s_Data.TextDesc}</color>";
                    Text text = Instantiate(Resources.Load<Text>("Text"), chatPrivatePanel);
                    text.text = $"<color=#000000>[私聊]{s_Data.Speaker}对我说：{s_Data.TextDesc}</color>";
                }
                break;
            case ChatType.Teat:
                break;
            default:
                break;
        }
    }
}
