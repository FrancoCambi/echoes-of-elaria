using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class DBManager : MonoBehaviour 
{
    private static DBManager instance;
    public static DBManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<DBManager>();
            }
            return instance;
        }
    }

    private IDbConnection connection;

    private string dbPath;

    private void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "template.db");
        dbPath = $"URI=file:{filePath}";

        connection = new SqliteConnection(dbPath);
        connection.Open();
        Debug.Log("Connected to DB: " + filePath);

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "PRAGMA journal_mode=WAL;";
        cmd.ExecuteNonQuery();
    }


    public DataTable ExecuteQuery(string query)
    {
        IDbCommand command = connection.CreateCommand();
        command.CommandText = query;

        IDataReader reader = command.ExecuteReader();
        DataTable table = new DataTable();

        // Cargar columnas
        for (int i = 0; i < reader.FieldCount; i++)
        {
            table.Columns.Add(reader.GetName(i), typeof(string));
        }

        // Cargar filas
        while (reader.Read())
        {
            object[] row = new object[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row[i] = reader.GetValue(i).ToString();
            }
            table.Rows.Add(row);
        }

        reader.Close();
        command.Dispose();

        return table;
    }

    public Task<DataTable> ExecuteQueryAsync(string query)
    {
        return Task.Run(() =>
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;

            IDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();

            // Cargar columnas
            for (int i = 0; i < reader.FieldCount; i++)
            {
                table.Columns.Add(reader.GetName(i), typeof(string));
            }

            // Cargar filas
            while (reader.Read())
            {
                object[] row = new object[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader.GetValue(i).ToString();
                }
                table.Rows.Add(row);
            }

            reader.Close();
            command.Dispose();

            return table;
        });
    }


    private void OnApplicationQuit()
    {
        Close();
    }

    public void Close()
    {
        connection.Close();
        Debug.Log("DB connection closed.");
    }
}
