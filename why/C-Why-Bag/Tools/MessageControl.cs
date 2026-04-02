using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageControl : Singleton<MessageControl>
{
    private Dictionary<int, Action<object>> dict = new();

    public void AddListener(int id, Action<object> action)
    {
        if (!dict.ContainsKey(id))
        {
            dict.Add(id,action);
        }
    }

    public void Dispatch(int id,params object[] par)
    {
        if (dict.ContainsKey(id))
        {
            dict[id]?.Invoke(par);
        }
    }
}
