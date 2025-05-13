using System;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get
        {
            instance ??= FindAnyObjectByType<PlayerManager>();
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
    public int MaxRage { get; private set; }
    public int CurrentRage { get; private set; }

    public static event Action<int> OnCurrentHealthChanged;
    public static event Action<int> OnMaxHealthChanged;
    public static event Action<int> OnCurrentRageChanged;
    public static event Action<int> OnMaxRageChanged;


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
            MaxRage = int.Parse(table.Rows[0]["max_rage"].ToString());
            CurrentRage = int.Parse(table.Rows[0]["current_rage"].ToString());
        }
    }

    #region Updaters

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
        OnCurrentHealthChanged?.Invoke(CurrentHealth);
    }

    public void UpdateMaxHealth(int newHealth)
    {
        MaxHealth = newHealth;
        SaveStatToDatabase("current_health", MaxHealth);
        OnMaxHealthChanged?.Invoke(MaxHealth);

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

    public void UpdateMaxRage(int newMaxRage)
    {
        MaxRage = newMaxRage;
        SaveStatToDatabase("max_rage", MaxRage);
    }


    public void UpdateCurrentRage(int newRage)
    {
        CurrentRage = Mathf.Clamp(newRage, 0, MaxRage);
        SaveStatToDatabase("current_rage", CurrentRage);
    }

    #endregion

    #region Manage

    public void TakeDamage(int damage)
    {
        UpdateCurrentHealth(CurrentHealth - damage);
        SaveStatToDatabase("current_health", CurrentHealth);
        OnCurrentHealthChanged?.Invoke(CurrentHealth);
    }

    public void GainRage(int amount)
    {
        UpdateCurrentRage(CurrentRage += amount);
        SaveStatToDatabase("current_rage", CurrentRage);
        OnCurrentRageChanged?.Invoke(CurrentRage);

    }

    #endregion

    private void SaveStatToDatabase(string statName, float value)
    {
        string query = $"UPDATE characters SET {statName} = {value} WHERE character_id = {GameManager.Instance.SelCharID}";
        DBManager.Instance.ExecuteQuery(query);
    }

}
