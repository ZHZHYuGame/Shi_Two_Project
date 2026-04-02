namespace BagNet;

public class SingLeton<T> where T :class,new()
{
    private static T instance;

    public static T GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}