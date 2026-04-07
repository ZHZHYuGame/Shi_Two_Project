namespace S;

public class MessageCenter : Singleton<MessageCenter>
{
    private Dictionary<int, Action<object>> dic = new();
    
    public void BroadCast(int id, params object[] args)
    {
        if (dic.ContainsKey(id))
        {
            dic[id]?.Invoke(args);
        }
    }


    public void AddListener(int id, Action<object> action)
    {
        if (!dic.ContainsKey(id))
        {
            dic.Add(id, action);
        }
    }
}