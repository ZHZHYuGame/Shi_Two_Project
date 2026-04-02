using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class AddCellTips : MonoBehaviour
{
    public Text haveNum;//拥有数量/消耗数量
    public Button qxbtn, kcBtn, gmBtn;//取消按钮，扩充按钮，购买按钮
    // Start is called before the first frame update
    void Start()
    {
        haveNum.text = Game.Instance.krjNum + "/" + Game.Instance.delJuanNum;
        qxbtn.onClick.AddListener(() =>
        {
            transform.gameObject.SetActive(false);
        });

        IsBuy();
        gmBtn.onClick.AddListener(BuyAddCellJuan);
        kcBtn.onClick.AddListener(KcBagCell);
        MessageControll.GetInstance().AddListener(NetID.S_T_C_BuyAddCellJuan_Message, S_T_C_BuyAddCellJuan_Message_Handle);
        MessageControll.GetInstance().AddListener(NetID.S_T_C_AddBagCell_Message, S_T_C_AddBagCell_Message_Handle);
    }
    /// <summary>
    /// 客户端处理服务器返回扩展结果
    /// </summary>
    /// <param name="obj"></param>
    private void S_T_C_AddBagCell_Message_Handle(object obj)
    {
        object[] objList = obj as object[];
        byte[] data = objList[0] as byte[];
        S_T_C_AddBagCell_Message s_data = S_T_C_AddBagCell_Message.Parser.ParseFrom(data);
        Game.Instance.krjNum = s_data.KrJuanNum;
        Game.Instance.delJuanNum = s_data.DelJuanNum;
        haveNum.text = Game.Instance.krjNum + "/" + Game.Instance.delJuanNum;
        IsBuy();
    }

    /// <summary>
    /// 客户端发送扩充请求
    /// </summary>
    private void KcBagCell()
    {
        if (Game.Instance.nowCellNum == Game.Instance.maxCellNum)
        {
            Debug.Log("背包格子已全部解锁");
            return;
        }
        C_T_S_AddBagCell_Message c_data = new C_T_S_AddBagCell_Message();
        c_data.Port = Game.Instance.port;
        NetManager.GetInstance().SendNetMessage(NetID.C_T_S_AddBagCell_Message, c_data.ToByteArray());
    }

    /// <summary>
    /// 客户端处理购买扩充卷回馈结果
    /// </summary>
    /// <param name="obj"></param>
    private void S_T_C_BuyAddCellJuan_Message_Handle(object obj)
    {
        object[] objList = obj as object[];
        byte[] data = objList[0] as byte[];
        S_T_C_BuyAddCellJuan_Message s_Data = S_T_C_BuyAddCellJuan_Message.Parser.ParseFrom(data);
        Game.Instance.krjNum = s_Data.KrJuanNum;

        Game.Instance.zsNum = s_Data.ZsNum;
        Game.Instance.zsText.text = Game.Instance.zsNum.ToString();
        haveNum.text = Game.Instance.krjNum + "/" + Game.Instance.delJuanNum;
        IsBuy();
    }

    private void BuyAddCellJuan()
    {
        C_T_S_BuyAddCellJuan_Message c_data = new C_T_S_BuyAddCellJuan_Message();
        c_data.Port = Game.Instance.port;
        NetManager.GetInstance().SendNetMessage(NetID.C_T_S_BuyAddCellJuan_Message, c_data.ToByteArray());
    }
    /// <summary>
    /// 判断扩充卷是否足够，足够显示扩充，不足限制购买
    /// </summary>
    public void IsBuy()
    {
        if (Game.Instance.krjNum >= Game.Instance.delJuanNum)
        {
            kcBtn.gameObject.SetActive(true);
            gmBtn.gameObject.SetActive(false);
        }
        else
        {
            kcBtn.gameObject.SetActive(false);
            gmBtn.gameObject.SetActive(true);
        }
    }
}
