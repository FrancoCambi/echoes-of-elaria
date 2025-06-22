using UnityEngine;

public class RespawnData : MonoBehaviour
{
    public int ID { get; private set; }
    public Vector3 RespawnPosition { get; private set; }
    public float RespawnTime { get; private set; }

    public float RespawnTimeElapsed { get; set; }

    public void SetUp(int id, Vector3 respawnPosition, float respawnTime)
    {
        ID = id;
        RespawnPosition = respawnPosition;
        RespawnTime = respawnTime;
        RespawnTimeElapsed = 0f;
    }
}
