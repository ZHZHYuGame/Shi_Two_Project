namespace BagNet;

public class UserManger:SingLeton<UserManger>
{
    public Dictionary<int,Client> userDict = new Dictionary<int, Client>();
    public void AddClientUser(Client c)
    {
        if (!userDict.ContainsKey(c.port))
        {
            userDict.Add(c.port,c);
        }
    }
    public void RemoveClientUser(Client c)
    {
        if (userDict.ContainsKey(c.port))
        {
            userDict.Remove(c.port);
        }
    }
}