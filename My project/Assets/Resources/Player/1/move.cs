using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class move : MonoBehaviour
{
    public GameObject loadscene;
    public Text text;
    public bool flag;
    public float timer;
    public Slider slider;
    public Button button;
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            loadscene.SetActive(true);
            flag = true;
        });
    }
    private void Update()
    {
        if (flag==true)
        {
            timer += Time.deltaTime;
            if (timer<2.8)
            {
                slider.value += 0.4f * Time.deltaTime;
            }
            if (timer>3)
            {
                timer = 0;
                SceneManager.LoadScene(1);
            }
        }
    }
}
