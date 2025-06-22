using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyLoot : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    private List<DroppedLoot> dropped;

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

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            LootManager.Instance.ShowLootTable(Dropped);
        }
    }
}
