using System.Net.Sockets;

namespace S;

public class UserManager:Singleton<UserManager>
{
    private Dictionary<int, Client> dic = new();

    public void Start()
    {
        
    }
    
    public void Add(Client client)
    {
        if (!dic.ContainsKey(client.port))
        {
            dic.Add(client.port, client);
        }
    }
}


public class Client
{
    public string ip;
    public int port;
    public Socket st;
    
    
    
}