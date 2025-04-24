using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;

public class DBManager
{
    private string dbPath;
    private IDbConnection connection;

    public DBManager(string dbFileName)
    {
        // Ruta para acceder al archivo .db
        string filePath = Path.Combine(Application.streamingAssetsPath, dbFileName);
        dbPath = $"URI=file:{filePath}";

        // Conexi�n inicial
        connection = new SqliteConnection(dbPath);
        connection.Open();
        Debug.Log("Connected to DB: " + filePath);
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
            table.Columns.Add(reader.GetName(i), typeof(string)); // Todo como string para simplificar
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

    public void Close()
    {
        connection.Close();
        Debug.Log("DB connection closed.");
    }
}
