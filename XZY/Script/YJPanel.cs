using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class YJPanel : MonoBehaviour
{
    
    void Start()
    {
        MessageControll.Instance().AddListener(NetID.S_2_CAddlose,AA);
    }

    private void AA(object obj)
    {
        Instantiate(Resources.Load("TC"),GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
