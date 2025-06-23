using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private Collider2D collider;

    private EnemyChase enemyChaseZone;
    private EnemyAnimation enemyAnimation;
    private EnemyLoot enemyLoot;
    private EnemyData data;
    private RespawnData respawnData;

    private int id;
    private int health;
    private float knockbackTime;

    public bool IsAlive
    {
        get
        {
            return health > 0;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        enemyChaseZone = GetComponent<EnemyChase>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        enemyLoot = GetComponent<EnemyLoot>();
        id = EnemyDatabase.GetIdByName(gameObject.name);
        data = EnemyDatabase.GetEnemyData(id);
        respawnData = GetComponent<RespawnData>();
        health = data.MaxHealth;
        
    }

    public int OnHit(int damage, Vector2 knockback)
    {
        if (!IsAlive) return 0;

        enemyAnimation.StartFlash();
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Death();
        }

        if (knockback != Vector2.zero)
        {
            knockbackTime = CalculateKnockbackTime(knockback);
            rb.linearVelocity = knockback;
            enemyChaseZone.CanChase = false;
            StartCoroutine(nameof(RestoreMovement));
        }

        // This may go to another place
        FloatingTextManager.Instance.ShowFloatingText(FloatingTextType.Damage, $"-{damage}", transform.position, new Vector2(0, 0));

        return damage;
    }

    private IEnumerator RestoreMovement()
    {
        yield return new WaitForSeconds(knockbackTime);
        enemyChaseZone.CanChase = true;
        rb.linearVelocity = Vector2.zero;
    }

    private float CalculateKnockbackTime(Vector2 knockback)
    {
        if (knockback == Vector2.zero) return 0f;

        return (float)(0.5f + 0.5f * Math.Tanh((knockback.magnitude / 5f) / 5f));
    }

    private void Death()
    {
        rb.linearVelocity = Vector2.zero;
        collider.isTrigger = true;
        enemyAnimation.DeathAnimation();

        PlayerManager.Instance.GainXp(415);

        enemyLoot.Dropped = LootManager.Instance.CreateLootTableByMobID(id);
        MobRespawnManager.NotifyDeath(respawnData);

        //Destroy(gameObject);
    }


}
