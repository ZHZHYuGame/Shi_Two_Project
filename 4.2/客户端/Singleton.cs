using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new()
{
    static readonly Lazy<T> lazy = new Lazy<T>(() => new T());
    
    public static T Ins => lazy.Value;
}
