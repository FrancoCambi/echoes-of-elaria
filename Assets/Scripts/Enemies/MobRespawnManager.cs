using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using UnityEngine;

public class MobRespawnManager : MonoBehaviour
{

    private static List<RespawnData> respawns = new();

    private void Start()
    {
        SpawnFromDB();
    }

    private void Update()
    {
        for (int i = 0; i < respawns.Count; i++)
        {
            RespawnData respawnData = respawns[i];
            respawnData.RespawnTimeElapsed += Time.deltaTime;
            if (respawnData.RespawnTimeElapsed >= respawnData.RespawnTime)
            {
                respawnData.RespawnTimeElapsed = 0f;
                MobFactory.SpawnMob(respawnData.ID, respawnData.RespawnPosition);
                respawns.Remove(respawnData);
                Destroy(respawnData.gameObject);
            }
        }
    }

    public static void NotifyDeath(RespawnData data)
    {
        respawns.Add(data);
    }

    private void SpawnFromDB()
    {
        string query = $"SELECT * FROM mobs_spawn";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            int id = int.Parse(row["mob_id"].ToString());
            int x = int.Parse(row["x"].ToString());
            int y = int.Parse(row["y"].ToString());

            MobFactory.SpawnMob(id, new Vector3(x, y, 0));
        }
    }
}
