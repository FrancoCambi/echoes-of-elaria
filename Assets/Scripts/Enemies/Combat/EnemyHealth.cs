using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private new Collider2D collider;

    private EnemyChase enemyChaseZone;
    private EnemyAnimation enemyAnimation;
    private EnemyLoot enemyLoot;
    private EnemyData data;
    private RespawnData respawnData;
    private EnemyCanvas enemyCanvas;

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

    public int Health
    {
        get
        {
            return health;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        enemyChaseZone = GetComponent<EnemyChase>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        enemyLoot = GetComponent<EnemyLoot>();
        respawnData = GetComponent<RespawnData>();
        enemyCanvas = GetComponent<EnemyCanvas>();

        id = EnemyDatabase.GetIdByName(gameObject.name);
        data = EnemyDatabase.GetEnemyData(id);

        health = data.MaxHealth;
        
    }

    private void ReceiveDamage(int damage)
    {
        health -= damage;
        enemyCanvas.ShowCanvasSeconds(5f);
        if (health <= 0)
        {
            health = 0;
            Death();
        }
        FloatingTextManager.Instance.ShowFloatingText(FloatingTextType.Damage, $"-{damage}", transform.position, new Vector2(0, 0));
    }

    private void ApplyKnockback(Vector2 knockback)
    {
        if (knockback != Vector2.zero)
        {
            knockbackTime = CalculateKnockbackTime(knockback);
            rb.linearVelocity = knockback;
            enemyChaseZone.CanChase = false;
            StartCoroutine(nameof(RestoreMovement));
        }
    }

    public int OnHit(int damage, Vector2 knockback)
    {
        if (!IsAlive) return 0;


        enemyAnimation.StartFlash();
        ReceiveDamage(damage);
        ApplyKnockback(knockback);

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
        enemyCanvas.HideCanvas();

        rb.linearVelocity = Vector2.zero;
        collider.isTrigger = true;

        enemyAnimation.DeathAnimation();

        PlayerManager.Instance.GainXp(415);
        enemyLoot.Dropped = LootManager.Instance.CreateLootTableByMobID(id);
        MobRespawnManager.NotifyDeath(respawnData);

        //Destroy(gameObject);
    }


}
