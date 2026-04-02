using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MessageControll :SingLeton<MessageControll>
{
    Dictionary<int,Action<object>> dic=new Dictionary<int, Action<object>>();

    public void AddListener(int id, Action<object> action)
    {
        if (!dic.ContainsKey(id))
        {
            dic.Add(id, action);
        }
    }

    public void DisPatch(int id, params object[] par)
    {
        dic[id]?.Invoke(par);
    }
}
