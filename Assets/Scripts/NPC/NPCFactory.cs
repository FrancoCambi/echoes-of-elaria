using System;
using UnityEngine;

public static class NPCFactory 
{
    public static GameObject SpawnNPC(int id, Vector3 position)
    {
        NPCData data = NPCDatabase.GetNPCData(id);
        GameObject npcPrefab = Resources.Load<GameObject>("Prefabs/BaseNPC");
        GameObject instance = GameObject.Instantiate(npcPrefab, position, Quaternion.identity);
        instance.name = data.Name;
        //instance.GetComponent<RespawnData>().SetUp(id, position, data.RespawnTime);

        foreach (string behavior in EnemyDatabase.GetBehaviorsByID(id))
        {
            Type type = Type.GetType(behavior);

            if (type != null && type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                instance.AddComponent(type);
            }
        }

        return instance;
    }
}
