using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RawImg : MonoBehaviour,IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        GameManager.Ins.player.transform.Rotate(Vector3.up * 3 * eventData.delta.x);
    }
}
