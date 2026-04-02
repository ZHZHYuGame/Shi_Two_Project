using System;
using System.Collections.Generic;

public class MessageControll : SingLeton<MessageControll>
{
    Dictionary<int, Action<object>> dict = new Dictionary<int, Action<object>>();

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
}