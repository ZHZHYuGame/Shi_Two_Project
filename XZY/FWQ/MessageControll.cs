using System;
using System.Collections.Generic;
    public class MessageControll
    {
        static MessageControll instance;

        public static MessageControll Instance()
        {
            if (instance == null)
            {
                instance = new MessageControll();
            }

            return instance;
        }
        private Dictionary<int, Action<object>> dict = new();

        public void AddListener(int id, Action<object> act)
        {
            if (!dict.ContainsKey(id))
            {
                dict.Add(id, act);
            }
            else
            {
                dict[id] += act;
            }
        }

        public void Dispatch(int id, params object[] par)
        {
            dict[id]?.Invoke(par);
        }

}

