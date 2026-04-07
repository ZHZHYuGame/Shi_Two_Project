using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RawImageMgr : MonoBehaviour,IDragHandler
{
    public GameObject player;
    void Start()
    {
        player=GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        player.transform.Rotate(0,eventData.delta.x*-3f,0);
    }
}
