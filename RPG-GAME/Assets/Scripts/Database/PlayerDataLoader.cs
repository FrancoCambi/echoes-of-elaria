using System.Data;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerStatsLoader
{
    private static PlayerStatsLoader instance;

    public static PlayerStatsLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerStatsLoader();
            }
            return instance;
        }
    }


    public float GetMovementSpeed(int id)
    {
        string query = $"SELECT movement_speed FROM characters WHERE character_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        float speed = float.Parse(table.Rows[0]["movement_speed"].ToString());
        return speed;
        
    }

    public float GetDashForce(int id)
    {
        string query = $"SELECT dash_force FROM characters WHERE character_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        float dashForce = float.Parse(table.Rows[0]["dash_force"].ToString());
        return dashForce;

    }

    public float GetDashCD(int id)
    {
        string query = $"SELECT dash_cd FROM characters WHERE character_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        float dashCD = float.Parse(table.Rows[0]["dash_cd"].ToString());
        return dashCD;

    }

    public int GetDamage(int id)
    {
        string query = $"SELECT damage FROM characters WHERE character_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int damage = int.Parse(table.Rows[0]["damage"].ToString());
        return damage;
    }
}
