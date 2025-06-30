using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class NPCDatabase
{
    private static Dictionary<int, NPCData> npcDataCache = new();
    private static NPCData LoadFromDB(int id)
    {
        string query = $"SELECT * FROM npc_data WHERE id = '{id}'";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        return new NPCData
        {
            ID = id,
            Name = table.Rows[0]["name"].ToString(),
            Type = StringToNPCType(table.Rows[0]["type"].ToString()),

        };
    }

    public static NPCData GetNPCData(int id)
    {
        if (!npcDataCache.ContainsKey(id))
        {
            npcDataCache[id] = LoadFromDB(id);
        }

        return npcDataCache[id];
    }

    public static int GetIdByName(string name)
    {
        string query = $"SELECT id FROM npc_data WHERE name = '{name}'";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        int id = int.Parse(table.Rows[0]["id"].ToString());
        return id;
    }

    public static List<string> GetBehaviorsByID(int id)
    {
        List<string> behaviors = new();
        string query = $"SELECT * from npc_behaviors WHERE mob_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            behaviors.Add(row["behavior"].ToString());
        }

        return behaviors;
    }

    public static List<string> GetDialogueKeysByID(int id)
    {
        List<string> keys = new();
        string query = $"SELECT * from npc_dialogue WHERE npc_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            keys.Add($"npc_{id}_{row["text_index"]}");
        }

        return keys;
    }

    private static NPCType StringToNPCType(string type)
    {
        return type switch
        {
            "Vendor" => NPCType.Vendor,
            "QuestGiver" => NPCType.QuestGiver,
            _ => NPCType.None,
        };
    }
}
