using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingLeton<T> where T:class,new()
{
    public static T ins;
    private static object obj = new object();
    public static T Instance
    {
        get
        {
            lock (obj)
            {
                if (ins==null)
                {
                    
                    ins = new T();
                }
            }
            return ins;
        }
    }
}
