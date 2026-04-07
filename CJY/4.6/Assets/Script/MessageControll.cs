using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageControll : Singleton<MessageControll>
{

    Dictionary<int, Action<object>> dict = new();

    public void AddListener(int id, Action<object> act)
    {
        if (!dict.ContainsKey(id))
        {
            dict.Add(id, act);
        }
    }

    public void Dispatch(int id, params object[] par)
    {
        dict[id]?.Invoke(par);
    }

    public void RemoveListener(int id, Action<object> act)
    {
        dict.Remove(id);
    }
}
