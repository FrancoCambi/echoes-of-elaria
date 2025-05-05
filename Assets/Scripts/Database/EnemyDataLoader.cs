using System.Data;
using UnityEngine;

public enum AttackType
{
    Jump, Throw, Melee, None
}
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

    public AttackType StringToAttackType(string type)
    {
        if (type.ToLower() == "jump")
        {
            return AttackType.Jump;
        }
        else if (type.ToLower() == "throw")
        {
            return AttackType.Throw;
        }
        else if (type.ToLower() == "melee")
        {
            return AttackType.Melee;
        }
        
        return AttackType.None;
        

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

    public string GetAttackType(int id)
    {
        string query = $"SELECT attack_type FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        string type = table.Rows[0]["attack_type"].ToString();
        return type;

    }

    public float GetJumpForce(int id)
    {
        string query = $"SELECT jump_force FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        float jumpForce = float.Parse(table.Rows[0]["jump_force"].ToString());
        return jumpForce;

    }

    public float GetKnockbackForce(int id)
    {
        string query = $"SELECT knockback_force FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        float knockbackForce = float.Parse(table.Rows[0]["knockback_force"].ToString());
        return knockbackForce;

    }

    public int GetMinDamage(int id)
    {
        string query = $"SELECT min_damage FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int minDamage = int.Parse(table.Rows[0]["min_damage"].ToString());
        return minDamage;

    }

    public int GetMaxDamage(int id)
    {
        string query = $"SELECT max_damage FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int maxDamage = int.Parse(table.Rows[0]["max_damage"].ToString());
        return maxDamage;

    }

    public int GetAttackCD(int id)
    {
        string query = $"SELECT attack_cd FROM mobs_data WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int attackCD = int.Parse(table.Rows[0]["attack_cd"].ToString());
        return attackCD;

    }
}
