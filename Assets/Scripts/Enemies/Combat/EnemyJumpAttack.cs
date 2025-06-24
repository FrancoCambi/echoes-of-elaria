using System.Collections;
using UnityEngine;

public class EnemyJumpAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Collider2D collider;

    private EnemyAttackZone attackZone;
    private EnemyHealth enemyHealth;
    private EnemyData data;

    private int id;

    private bool attacking;
    private bool canAttack;


    public bool Attacking
    {
        get
        {
            return attacking;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        attackZone = GetComponent<EnemyAttackZone>();
        enemyHealth = GetComponent<EnemyHealth>();

        id = EnemyDatabase.GetIdByName(gameObject.name);
        data = EnemyDatabase.GetEnemyData(id);

        attacking = false;
        canAttack = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyHealth.IsAlive && attackZone.PlayerInRange && canAttack && !attacking)
        {
            StartCoroutine(nameof(JumpAttack));
        }
    }

    private IEnumerator JumpAttack()
    {
        attacking = true;
        canAttack = false;

        Vector3 directionVector = (PlayerManager.Instance.transform.position - transform.position).normalized;
        rb.linearVelocity = directionVector * data.JumpForce;

        yield return new WaitForSeconds(data.AttackCD);
        canAttack = true;
    }

    private void StopAttack()
    {
        attacking = false;
        rb.linearVelocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
            Vector2 direction = (Vector2)(col.gameObject.transform.position - transform.position).normalized;
            Vector2 knockback = direction * data.KnockbackForce;
            int damage = Random.Range(data.MinDamage, data.MaxDamage + 1);
            damageable.OnHit(damage, knockback);
            collider.enabled = false;
            collider.enabled = true;
        }
    }



}
