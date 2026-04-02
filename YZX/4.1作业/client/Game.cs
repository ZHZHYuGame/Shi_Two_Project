using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public static Game Instance;
    void Awake()
    {
        //Application.dataPath
        //Application.streamingAssetsPath
        //Application.persistentDataPath
        Instance = this;
        NetManager.GetInstance().Start();
        ConfigManager.GetInstance().Start();
    }
    public int port;//本用户的端口号
    public int krjNum;//本用户有的背包扩容卷
    public int delJuanNum;//需要消耗的扩容卷数量
    public int coinNum;//拥有的金币数量
    public Text coinText;
    public int zsNum;//拥有钻石数量
    public Text zsText;
    public int nowCellNum;//目前格子数量
    public int maxCellNum;//最大格子数量
    public Button OpenBagBtn;
    public Image BagPanel;//背包面板
    public Image eqTipsPanel;//装备提示面板
    // Start is called before the first frame update
    void Start()
    {
        OpenBagBtn.onClick.AddListener(() =>
        {
            BagPanel.gameObject.SetActive(true);
        });


        MessageControll.GetInstance().AddListener(NetID.S_T_C_User_Message, S_T_C_User_Message_Hanlde);
    }

    private void S_T_C_User_Message_Hanlde(object obj)
    {
        object[] objList = obj as object[];
        byte[] data = objList[0] as byte[];
        S_2_C_User_Message s_Data = S_2_C_User_Message.Parser.ParseFrom(data);

        port = s_Data.Port;
        krjNum = s_Data.KrJuanNum;
        delJuanNum = s_Data.DelJuanNum;
        coinNum = s_Data.CoinNum;
        coinText.text = coinNum.ToString();
        zsNum = s_Data.ZsNum;
        zsText.text = zsNum.ToString();
        Debug.Log(port);
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.GetInstance().Update();
    }
}
