using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Games;
public class RoleEqCellUI : MonoBehaviour
{
    public Image icon;

    public CellsData myCellsData;

    public void Start()
    {
        icon.gameObject.SetActive(false);
    }
    public void Init(Good good)
    {

    }
}
