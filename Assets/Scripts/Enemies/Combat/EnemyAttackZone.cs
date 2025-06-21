using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Jump, Throw, Melee, None
}
public class EnemyAttackZone : MonoBehaviour
{
    EnemyData data;

    private bool playerInRange;

    private readonly float checkCooldown = 0.25f;
    private float timeElapsed = 0f;
    public bool PlayerInRange
    {
        get
        {
            return playerInRange;
        }
    }

    private void Awake()
    {
        playerInRange = false;
    }

    private void Start()
    {
        data = EnemyDatabase.GetEnemyData(EnemyDatabase.GetIdByName(gameObject.name));
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= checkCooldown)
        {
            timeElapsed = 0f;
            CheckPlayerInRange();
        }
    }

    private void CheckPlayerInRange()
    {

        if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= data.AttackRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }
}
