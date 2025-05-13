using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;

    private PlayerMovement playerMovement;

    private float invincibilityTime;
    private float knockbackTime;
    private bool invincible;

    public bool IsAlive
    {
        get
        {
            return PlayerManager.Instance.CurrentHealth > 0;
        }
    }

    public bool Invincible
    {
        get
        {
            return invincible;
        }
    }

    public float InvincibilityTime
    {
        get
        {
            return invincibilityTime;
        }
        set
        {
            invincibilityTime = value;
        }
    }

    public float KnockbackTime
    {
        get
        {
            return knockbackTime;
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerMovement = GetComponent<PlayerMovement>();

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
        invincible = true;
        StartCoroutine(nameof(StopInvincibility));

        int receivedDamage = CalculateDamageReceived(damage);
        PlayerManager.Instance.TakeDamage(receivedDamage);

        if (PlayerManager.Instance.CurrentHealth <= 0)
        {
            Death();
        }

        if (knockback != Vector2.zero)
        {
            knockbackTime = CalculateKnockbackTime(knockback);
            playerMovement.CanMove = false;
            StartCoroutine(nameof(RestoreMovement));
            rb.linearVelocity = Vector2.zero;   
            rb.linearVelocity = knockback;
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
        rb.linearVelocity = Vector2.zero;
        playerMovement.CanMove = true;
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
