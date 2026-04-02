namespace BagNet;
using Newtonsoft.Json;
public class ConfigManger:SingLeton<ConfigManger>
{
    public List<ItemData> allList=new List<ItemData>();
    public void Start()
    {
        string str=File.ReadAllText("good.json");
        allList = JsonConvert.DeserializeObject<List<ItemData>>(str);
        Console.WriteLine(allList.Count);
    }
    public ItemData RandomItem()
    {
        return allList[new Random().Next(0, allList.Count)];
    }

    public ItemData FindItem(int id)
    {
        return allList.Find(x => x.Id == id);
    }
}