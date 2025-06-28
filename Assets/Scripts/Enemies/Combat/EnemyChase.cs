using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private Rigidbody2D rb;

    private EnemyJumpAttack enemyAttack;
    private EnemyHealth enemyHealth;
    private EnemyData data;

    private int id;
    private bool canChase;
    private bool playerInSight;

    private readonly float checkCooldown = 0.5f;
    private float timeElapsed = 0f;
    

    public bool CanChase
    {
        get
        {
            return canChase;
        }
        set
        {
            canChase = value;
        }
    }

    public bool PlayerInSight
    {
        get
        {
            return playerInSight;
        }
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyAttack = GetComponent<EnemyJumpAttack>();
        enemyHealth = GetComponent<EnemyHealth>();

        id = EnemyDatabase.GetIdByName(gameObject.name);
        data = EnemyDatabase.GetEnemyData(id);
        canChase = true;
        playerInSight = false;
    }

    private void Update()
    {
        // Checks 2 times per second instead of every frame
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= checkCooldown)
        {
            timeElapsed = 0;
            CheckPlayerInSight();
        }
    }

    private void FixedUpdate()
    {
        if (enemyHealth.IsAlive && canChase && playerInSight && !enemyAttack.Attacking)
        {
            Vector2 direction = (PlayerManager.Instance.transform.position - transform.position).normalized;

            rb.linearVelocity = direction * data.MovementSpeed;
        }
    }

    private void CheckPlayerInSight()
    {

        if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= CalculateChaseDistance())
        {
            playerInSight = true;
        }
        else
        {
            playerInSight = false;
        }
    }

    private float CalculateChaseDistance()
    {
        // TODO: CALCULATE ACCORDING TO PLAYER AND ENEMY LEVEL DIFFERENCE
        return 2f;
    }

}
