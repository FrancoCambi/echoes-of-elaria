using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseZone : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject parent;
    private List<Collider2D> detectedObjs;

    private EnemyJumpAttack enemyAttack;
    private EnemyHealth enemyHealth;

    private int id;
    private float movementSpeed;
    private bool canChase;

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

    public Collider2D PlayerCollider
    {
        get
        {
            foreach (Collider2D obj in detectedObjs)
            {
                if (obj.gameObject.CompareTag("Player"))
                {
                    return obj;
                }
            }
            return null;
        }
    }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        parent = transform.parent.gameObject;
        detectedObjs = new List<Collider2D>();

        enemyAttack = GetComponentInParent<EnemyJumpAttack>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        id = EnemyDataLoader.Instance.GetIdByName(parent.name);
        movementSpeed = EnemyDataLoader.Instance.GetMovementSpeed(id);
        canChase = true;
    }
    private void FixedUpdate()
    {
        if (enemyHealth.IsAlive && canChase && PlayerCollider != null && !enemyAttack.Attacking)
        {
            Vector2 direction = (PlayerCollider.transform.position - transform.position).normalized;

            rb.linearVelocity = direction * movementSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedObjs.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedObjs.Remove(collision);
    }

}
