using System;
using UnityEngine;

public static class MobFactory 
{
    public static GameObject SpawnMob(int id, Vector3 position)
    {
        EnemyData data = EnemyDatabase.GetEnemyData(id);
        GameObject mobPrefab = Resources.Load<GameObject>("Prefabs/BaseMob");
        GameObject instance = GameObject.Instantiate(mobPrefab, position, Quaternion.identity);
        instance.name = data.Name;
        instance.GetComponent<RespawnData>().SetUp(id, position, data.RespawnTime);

        foreach (string behavior in EnemyDatabase.GetBehaviorsByID(id))
        {
            Type type = Type.GetType("Enemy" + behavior);
            
            if (type != null && type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                instance.AddComponent(type);
            }
        }

        return instance;
    }
}
