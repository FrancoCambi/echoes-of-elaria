using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyJumpAttack : MonoBehaviour
{
    private Rigidbody2D rb;

    private EnemyAttackZone attackZone;

    private int id;
    private float jumpForce;
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

        attackZone = GetComponentInChildren<EnemyAttackZone>();

        id = EnemyDataLoader.Instance.GetIdByName(gameObject.name);
        jumpForce = EnemyDataLoader.Instance.GetJumpForce(id);
        attackCD = EnemyDataLoader.Instance.GetAttackCD(id);

        attacking = false;
        canAttack = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attackZone.PlayerCollider != null && canAttack && !attacking)
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


}
