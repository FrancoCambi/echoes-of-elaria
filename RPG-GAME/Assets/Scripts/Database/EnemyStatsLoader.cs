using System.Data;
using UnityEngine;

public class EnemyDataLoader
{
    private static EnemyDataLoader instance;
    public static EnemyDataLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyDataLoader();
            }
            return instance;
        }
    }

    public int GetIdByName(string name)
    {
        string query = $"SELECT id FROM mobs_stats WHERE name = '{name}'";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int id = int.Parse(table.Rows[0]["id"].ToString());
        return id;
    }

    public int GetHealthById(int id)
    {
        string query = $"SELECT health FROM mobs_stats WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int health = int.Parse(table.Rows[0]["health"].ToString());
        return health;

    }
}
