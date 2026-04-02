using Game;

namespace S;

public partial class BagManager 
{
    void SortBag(Client client)
    {
        var list = client.allList;
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if ((list[i].Item.Quality < list[j].Item.Quality)
                    || (list[i].Item.Quality == list[j].Item.Quality && list[i].Count < list[j].Count)
                    || (list[i].Item.Quality == list[j].Item.Quality && list[i].Count == list[j].Count && GetItemTypeWeight(list[i].Item.Type) > GetItemTypeWeight(list[j].Item.Type)))
                {
                    (list[i], list[j]) = (list[j], list[i]);
                }
            }
        }
        UpdateAllList(client);
    }

    int GetItemTypeWeight(string type)
    {
        switch (type)
        {
            case "装备":
                return 0;
            case "药品":
                return 1;
        }
        return -1;
    }
}