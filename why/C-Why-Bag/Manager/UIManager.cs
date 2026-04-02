using System.Collections;
using System.Collections.Generic;
using Games;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject bagPanel;
    public Button bagBtn;
    // Start is called before the first frame update
    void Start()
    {
        mainPanel = Instantiate(Resources.Load<GameObject>("PanelPrefab/Main"), GameObject.Find("Canvas").transform);
        
        bagBtn = mainPanel.transform.Find("BagBtn").transform.GetComponent<Button>();
        bagBtn.onClick.AddListener(() =>
        {
            C_2_S_GetBagType msg = new C_2_S_GetBagType();
            msg.Type = 0;
            NetManager.GetInstance().SendNetMessage(NetID.C_To_S_GetBagTypeMessage,msg.ToByteArray());
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
