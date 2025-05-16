using System.Data;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    private static ItemsManager instance;

    public static ItemsManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<ItemsManager>();
            return instance;
        }
    }
    public Item GetItemByID(int id)
    {
        string query = $"SELECT * FROM items WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);
        Item item = null;

        if (table.Rows.Count > 0)
        {
            int _id = int.Parse(table.Rows[0]["id"].ToString());
            string _name = table.Rows[0]["name"].ToString();
            item = new(_id, _name);
        }

        return item;
    }
}
