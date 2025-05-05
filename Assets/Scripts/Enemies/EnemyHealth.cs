using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;

    private EnemyChaseZone enemyChaseZone;

    private int id;
    private int health;
    private string enemyName;
    private float knockbackTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyChaseZone = GetComponentInChildren<EnemyChaseZone>();

        enemyName = gameObject.name.Replace("(Clone)", "");
        id = EnemyDataLoader.Instance.GetIdByName(enemyName);
        health = EnemyDataLoader.Instance.GetHealth(id);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        knockbackTime = CalculateKnockbackTime(knockback);

        health -= damage;
        rb.linearVelocity = knockback;

        enemyChaseZone.CanChase = false;
        StartCoroutine(nameof(RestoreMovement));

        if (health <= 0)
        {
            Death();
        }
    }

    private IEnumerator RestoreMovement()
    {
        yield return new WaitForSeconds(knockbackTime);
        enemyChaseZone.CanChase = true;
        rb.linearVelocity = Vector2.zero;
    }

    private float CalculateKnockbackTime(Vector2 knockback)
    {
        return (float)(0.5f + 0.5f * Math.Tanh((knockback.magnitude / 5f) / 5f));
    }

    private void Death()
    {
        Destroy(gameObject);
    }

}
