using System.Data;
using UnityEngine;

public class NPCSpawnManager : MonoBehaviour
{
    private void Start()
    {
        SpawnFromDB();
    }

    private void SpawnFromDB()
    {
        string query = $"SELECT * FROM npc_spawn";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            int id = int.Parse(row["npc_id"].ToString());
            int x = int.Parse(row["x"].ToString());
            int y = int.Parse(row["y"].ToString());

            NPCFactory.SpawnNPC(id, new Vector3(x, y, 0));
        }
    }
}
