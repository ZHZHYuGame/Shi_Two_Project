using System.Net.Sockets;

namespace BagNet;

public class Client
{
    public Socket st;
    public string ip;
    public int port;
    public UserData userData=new UserData();
}