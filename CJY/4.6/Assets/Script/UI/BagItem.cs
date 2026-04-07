using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItem : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Text numText;
    [SerializeField] private Image qualityBg; // 品质背景
    [SerializeField] private Text levelText; // 等级显示
    
    public int id; // 格子ID
    public BagData data; // 物品数据
    
    private float timer = 1;
    
    void Start()
    {
        // 初始化
    }

    void Update()
    {
        // 更新逻辑
    }

    public void OnInit(int index, BagData bag)
    {
        id = index;
        data = bag;
        
        if (bag != null && bag.Shop != null)
        {
            // 设置图标
            if (!string.IsNullOrEmpty(bag.Shop.Icon))
            {
                icon.gameObject.SetActive(true);
                icon.sprite = Resources.Load<Sprite>("icon/" + bag.Shop.Icon);
                
                // 根据品质设置背景色
                SetQualityColor(bag.Quality);
            }
            
            // 设置数量显示
            if (bag.MaxStack > 1)
            {
                numText.gameObject.SetActive(true);
                numText.text = bag.Count.ToString();
            }
            else
            {
                numText.gameObject.SetActive(false);
            }
            
            // 设置等级显示
            if (levelText != null && bag.Level > 0)
            {
                levelText.gameObject.SetActive(true);
                levelText.text = $"Lv.{bag.Level}";
            }
            else if (levelText != null)
            {
                levelText.gameObject.SetActive(false);
            }
        }
        else
        {
            // 清空格子
            icon.gameObject.SetActive(false);
            numText.gameObject.SetActive(false);
            if (qualityBg != null) qualityBg.color = Color.gray;
            if (levelText != null) levelText.gameObject.SetActive(false);
        }
    }
    
    // 根据品质设置背景色
    private void SetQualityColor(int quality)
    {
        if (qualityBg == null) return;
        
        switch (quality)
        {
            case 0: // 白色
                qualityBg.color = Color.white;
                break;
            case 1: // 绿色
                qualityBg.color = Color.green;
                break;
            case 2: // 蓝色
                qualityBg.color = Color.blue;
                break;
            case 3: // 紫色
                qualityBg.color = new Color(0.5f, 0f, 0.5f); // 紫色
                break;
            case 4: // 橙色
                qualityBg.color = new Color(1f, 0.5f, 0f); // 橙色
                break;
            default:
                qualityBg.color = Color.gray;
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (data != null)
        {
            // 处理物品点击（比如显示详情、使用物品等）
            Debug.Log($"点击物品: {data.Shop.Name}, 数量: {data.Count}");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data != null && BagUI.ins != null)
        {
            BagUI.ins.tips.gameObject.SetActive(true);
            BagUI.ins.OpenTips(data);
            BagUI.ins.tips.transform.position = eventData.position;
        }
    }
    
    // 鼠标离开时隐藏提示
    public void OnPointerExit(PointerEventData eventData)
    {
        if (data != null && BagUI.ins != null)
        {
            BagUI.ins.tips.gameObject.SetActive(false);
        }
    }
}