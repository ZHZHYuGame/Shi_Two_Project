using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cd : MonoBehaviour
{
    public Animator ain;
    public Button btn;
    public bool flag;
    public Button img;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            flag = true;
            ain.SetTrigger("gongji");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (flag == true)
        {
            img.GetComponent<Image>().fillAmount += 0.2f * Time.deltaTime;
            img.interactable = false;
        }
        if (img.GetComponent<Image>().fillAmount == 1)
        {
            img.GetComponent<Image>().fillAmount = 0;
            img.interactable = true;
            flag = false;
        }
    }
}