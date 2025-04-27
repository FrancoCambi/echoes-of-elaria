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
        string query = $"SELECT id FROM mobs_data WHERE name = '{name}'";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int id = int.Parse(table.Rows[0]["id"].ToString());
        return id;
    }

    public int GetHealth(int id)
    {
        string query = $"SELECT health FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int health = int.Parse(table.Rows[0]["health"].ToString());
        return health;

    }

    public int GetMaxPatrolCD(int id)
    {
        string query = $"SELECT max_patrol_cd FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int maxPatrolCD = int.Parse(table.Rows[0]["max_patrol_cd"].ToString());
        return maxPatrolCD;

    }

    public float GetMovementSpeed(int id)
    {
        string query = $"SELECT movement_speed FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        float movementSpeed = float.Parse(table.Rows[0]["movement_speed"].ToString());
        return movementSpeed;

    }

    public float GetPatrolSpeed(int id)
    {
        string query = $"SELECT patrol_speed FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        float patrolSpeed = float.Parse(table.Rows[0]["patrol_speed"].ToString());
        return patrolSpeed;

    }
}
