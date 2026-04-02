using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SingLeton<T> where T:class,new()
{
    public static T ins;
    private static object obj = new object();
    public static T Getinstance()
    {
        if (ins == null)
        {
            lock (obj)
            {
                if (ins == null)
                {
                    ins = new T();
                }
            }    
        }
        return ins;
    }
}
