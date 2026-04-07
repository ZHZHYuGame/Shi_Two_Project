using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumePanel : MonoBehaviour
{
    [SerializeField] private Text consumeText, countText;
    [SerializeField] private Image img;

    public Action noClick, yesClick;

    public void OnSetText(string con,string count,string imgPath)
    {
        consumeText.text = con;
        countText.text = count;
        img.sprite = Resources.Load<Sprite>(imgPath);
    }

    public void OnNoClick()
    {
        noClick?.Invoke();
    }

    public void OnYesClick()
    {
        yesClick?.Invoke();
    }
}
