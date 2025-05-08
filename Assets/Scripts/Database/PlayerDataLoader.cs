using System.Data;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerDataLoader : MonoBehaviour
{
    private static PlayerDataLoader instance;

    public static PlayerDataLoader Instance
    {
        get
        {
            instance ??= FindAnyObjectByType<PlayerDataLoader>();
            return instance;
        }
    }

    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }
    public float MovementSpeed { get; private set; }
    public float DashForce { get; private set; }
    public float DashCD { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }


    private void Awake()
    {
        LoadStatsFromDatabase();
    }


    private void LoadStatsFromDatabase()
    {
        string query = $"SELECT * FROM characters WHERE character_id = {GameManager.Instance.SelCharID}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        if (table.Rows.Count > 0)
        {
            MovementSpeed = float.Parse(table.Rows[0]["movement_speed"].ToString());
            DashForce = float.Parse(table.Rows[0]["dash_force"].ToString());
            DashCD = float.Parse(table.Rows[0]["dash_cd"].ToString());
            MinDamage = int.Parse(table.Rows[0]["min_damage"].ToString());
            MaxDamage = int.Parse(table.Rows[0]["max_damage"].ToString());
            MaxHealth = int.Parse(table.Rows[0]["max_health"].ToString());
            CurrentHealth = int.Parse(table.Rows[0]["current_health"].ToString());
        }
    }

    public void UpdateMovementSpeed(float newSpeed)
    {
        MovementSpeed = newSpeed;
        SaveStatToDatabase("movement_speed", newSpeed);
    }

    public void UpdateDashForce(float newDashForce)
    {
        DashForce = newDashForce;
        SaveStatToDatabase("dash_force", newDashForce);
    }

    public void UpdateCurrentHealth(int newHealth)
    {
        CurrentHealth = Mathf.Clamp(newHealth, 0, MaxHealth);
        SaveStatToDatabase("current_health", CurrentHealth);
    }

    public void UpdateMaxHealth(int newHealth)
    {
        MaxHealth = newHealth;
        SaveStatToDatabase("current_health", MaxHealth);
    }

    public void UpdateMinDamage(int newMinDamage)
    {
        MinDamage = newMinDamage;
        SaveStatToDatabase("current_health", MinDamage);
    }

    public void UpdateMaxDamage(int newMaxDamage)
    {
        MaxDamage = newMaxDamage;
        SaveStatToDatabase("current_health", MaxDamage);
    }

    private void SaveStatToDatabase(string statName, float value)
    {
        string query = $"UPDATE characters SET {statName} = {value} WHERE character_id = {GameManager.Instance.SelCharID}";
        DBManager.Instance.ExecuteQuery(query);
    }

}
