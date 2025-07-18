using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using UnityEngine;

public static class EnemyDatabase 
{
    private static Dictionary<int, EnemyData> enemyDataCache = new();

    private static EnemyData LoadFromDB(int id)
    {
        string query = $"SELECT * FROM mobs_data WHERE id = '{id}'";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        return new EnemyData
        {
            ID = id,
            Name = table.Rows[0]["name"].ToString(),
            MaxHealth = int.Parse(table.Rows[0]["health"].ToString()),
            Level = int.Parse(table.Rows[0]["level"].ToString()),
            MovementSpeed = float.Parse(table.Rows[0]["movement_speed"].ToString()),
            PatrolSpeed = float.Parse(table.Rows[0]["patrol_speed"].ToString()),
            MaxPatrolCD = float.Parse(table.Rows[0]["max_patrol_cd"].ToString()),
            KnockbackForce = float.Parse(table.Rows[0]["knockback_force"].ToString()),
            AttackType = StringToAttackType(table.Rows[0]["attack_type"].ToString()),
            JumpForce = float.Parse(table.Rows[0]["jump_force"].ToString()),
            AttackCD = float.Parse(table.Rows[0]["attack_cd"].ToString()),
            AttackRange = float.Parse(table.Rows[0]["attack_range"].ToString()),
            MinDamage = int.Parse(table.Rows[0]["min_damage"].ToString()),
            MaxDamage = int.Parse(table.Rows[0]["max_damage"].ToString()),
            RespawnTime = int.Parse(table.Rows[0]["respawn_time"].ToString()),

        };
    }

    public static EnemyData GetEnemyData(int id)
    {
        if (!enemyDataCache.ContainsKey(id))
        {
            enemyDataCache[id] = LoadFromDB(id);
        }

        return enemyDataCache[id];
    }

    public static int GetIdByName(string name)
    {
        string query = $"SELECT id FROM mobs_data WHERE name = '{name}'";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int id = int.Parse(table.Rows[0]["id"].ToString());
        return id;
    }

    public static List<string> GetBehaviorsByID(int id)
    {
        List<string> behaviors = new();
        string query = $"SELECT * from mobs_behaviors WHERE mob_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            behaviors.Add(row["behavior"].ToString());
        }

        return behaviors;
    }

    private static AttackType StringToAttackType(string type)
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
}
