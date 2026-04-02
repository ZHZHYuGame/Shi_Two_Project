using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter:Singleton<MessageCenter>
{
    private Dictionary<int, Action<object>> dic = new();

    public void BroadCast(int id,params object[] args)
    {
        if (dic.ContainsKey(id))
        {
            dic[id](args);
        }
    }


    public void AddListener(int id, Action<object> call)
    {
        if (!dic.ContainsKey(id))
        {
            dic.Add(id, call);
        }
    }
}
