using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyLoot : MonoBehaviour
{

    private List<DroppedLoot> dropped;
    private EnemyData enemyData;

    public List<DroppedLoot> Dropped
    {
        get
        {
            return dropped;
        }
        set
        {
            dropped = value;
        }
    }

    public bool BeingLooted { get; set; }

    private void Start()
    {
        enemyData = EnemyDatabase.GetEnemyData(EnemyDatabase.GetIdByName(gameObject.name));
    }

    private void Update()
    {

        if (BeingLooted && !PlayerInLootRange())
        {
            LootManager.Instance.Close();
        }
    }

    public void RemoveFromDropped(DroppedLoot dLoot)
    {
        dropped.Remove(dLoot);

        /*if (dropped.Count == 0)
        {
            Destroy(gameObject, enemyData.RespawnTime * (3f / 4f));
        }*/
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && PlayerInLootRange())
        {
            LootManager.Instance.ShowLootTable(Dropped, this);
        }
    }

    private bool PlayerInLootRange()
    {
        Collider2D col = GetComponent<Collider2D>();

        Collider2D playerCol = PlayerManager.Instance.GetComponent<Collider2D>();

        return col.Distance(playerCol).distance <= 1f;

    }

    private void OnDestroy()
    {
        if (BeingLooted)
        {
            LootManager.Instance.Close();
        }
    }
}
