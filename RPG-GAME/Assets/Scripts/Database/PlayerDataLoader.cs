using System.Data;
using UnityEngine;
public class PlayerStatsLoader
{
    private DBManager db;

    public PlayerStatsLoader(DBManager db)
    {
        this.db = db;
    }

    public float GetMovementSpeed()
    {
        string query = "SELECT movement_speed FROM characters WHERE character_id = 1";
        DataTable table = db.ExecuteQuery(query);

        float speed = float.Parse(table.Rows[0]["movement_speed"].ToString());
        return speed;
        
    }
}
