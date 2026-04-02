using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageControll : Singlton<MessageControll>
{
    Dictionary<int, Action<object>> dict = new();

    public void AddListener(int id, Action<object> act)
    {
        if (!dict.ContainsKey(id))
        {
            //Debug.Log("绑定事件");
            dict.Add(id, act);
        }
    }


    public void Dispatch(int id, params object[] par)
    {
        //Debug.Log("更新时间id：" + id);
        // 如果有参数，传递第一个参数，否则传递id
        if (par != null && par.Length > 0)
        {
            dict[id]?.Invoke(par);
        }
        else
        {
            dict[id]?.Invoke(id);
        }
    }
}
