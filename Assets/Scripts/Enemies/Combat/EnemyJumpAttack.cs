using System.Collections;
using UnityEngine;

public class EnemyJumpAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    private EnemyAttackZone attackZone;
    private EnemyHealth enemyHealth;

    private int id;
    private float jumpForce;
    private int minDamage;
    private int maxDamage;
    private float knockbackForce;
    private int attackCD;
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
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        attackZone = GetComponentInChildren<EnemyAttackZone>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        id = EnemyDataLoader.Instance.GetIdByName(gameObject.name);
        jumpForce = EnemyDataLoader.Instance.GetJumpForce(id);
        minDamage = EnemyDataLoader.Instance.GetMinDamage(id);
        maxDamage = EnemyDataLoader.Instance.GetMaxDamage(id);
        knockbackForce = EnemyDataLoader.Instance.GetKnockbackForce(id);
        attackCD = EnemyDataLoader.Instance.GetAttackCD(id);

        attacking = false;
        canAttack = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyHealth.IsAlive && attackZone.PlayerCollider != null && canAttack && !attacking)
        {
            StartCoroutine(nameof(JumpAttack));
        }
    }

    private IEnumerator JumpAttack()
    {
        attacking = true;
        canAttack = false;

        Vector3 directionVector = (attackZone.PlayerCollider.transform.position - transform.position).normalized;
        rb.linearVelocity = directionVector * jumpForce;

        yield return new WaitForSeconds(attackCD);
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
            Vector2 knockback = direction * knockbackForce;
            int damage = Random.Range(minDamage, maxDamage + 1);
            damageable.OnHit(damage, knockback);
            capsuleCollider.enabled = false;
            capsuleCollider.enabled = true;
        }
    }



}
