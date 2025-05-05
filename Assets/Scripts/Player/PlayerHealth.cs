using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;

    private PlayerAnimation playerAnimation;
    private PlayerMovement playerMovement;

    private int charID;
    private int maxHealth;
    private int currentHealth;
    private float invincibilityTime;
    private float knockbackTime;
    private bool invincible;

    public bool IsAlive
    {
        get
        {
            return currentHealth > 0;
        }
    }

    public bool Invincible
    {
        get
        {
            return invincible;
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();

        charID = GameManager.Instance.SelCharID;
        maxHealth = PlayerDataLoader.Instance.GetMaxHealth(charID);
        currentHealth = PlayerDataLoader.Instance.GetCurrentHealth(charID);
        invincibilityTime = 1f;
        invincible = false;
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        if (invincible) return;

        // Else

        knockbackTime = CalculateKnockbackTime(knockback);

        invincible = true;
        StartCoroutine(nameof(StopInvincibility));

        playerMovement.CanMove = false;
        StartCoroutine(nameof(RestoreMovement));

        currentHealth -= CalculateDamageReceived(damage);
        rb.linearVelocity = knockback;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }

    }

    private IEnumerator StopInvincibility()
    {
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }

    private IEnumerator RestoreMovement()
    {
        yield return new WaitForSeconds(knockbackTime);
        playerMovement.CanMove = true;
        rb.linearVelocity = Vector2.zero;
    }

    private float CalculateKnockbackTime(Vector2 knockback)
    {
        return (float)(0.5f + 0.5f * Math.Tanh((knockback.magnitude / 5f) / 5f));
    }

    private int CalculateDamageReceived(int damage)
    {
        return damage;
    }
}
