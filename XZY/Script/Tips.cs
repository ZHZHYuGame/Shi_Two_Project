using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    public Button CloseBtn;
    public Image icon;
    public Text name, des;
    public static Tips instance;

    private void Awake()
    {
        instance = this;
        CloseBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            
        });
        gameObject.SetActive(false);
    }

    public void GetData(Item item)
    {
        gameObject.SetActive(true);
        icon.sprite = Resources.Load<Sprite>(item.path);
        name.text = item.name;
        des.text = item.des;
    }
}
