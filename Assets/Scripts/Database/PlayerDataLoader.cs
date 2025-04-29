using System.Data;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerDataLoader
{
    private static PlayerDataLoader instance;

    public static PlayerDataLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerDataLoader();
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

    public int GetMaxHealth(int id)
    {
        string query = $"SELECT max_health FROM characters WHERE character_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int maxHealth = int.Parse(table.Rows[0]["max_health"].ToString());
        return maxHealth;
    }

    public int GetCurrentHealth(int id)
    {
        string query = $"SELECT current_health FROM characters WHERE character_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int currentHealth = int.Parse(table.Rows[0]["current_health"].ToString());
        return currentHealth;
    }
}
