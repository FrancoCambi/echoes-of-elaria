using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerAnimation playerAnimation;
    private Rigidbody2D rb;

    private int charID;
    private int maxHealth;
    private int currentHealth;

    public bool IsAlive
    {
        get
        {
            return currentHealth > 0;
        }
    }


    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();

        charID = GameManager.Instance.SelCharID;
        maxHealth = PlayerDataLoader.Instance.GetMaxHealth(charID);
        currentHealth = PlayerDataLoader.Instance.GetCurrentHealth(charID);
    }

    void Update()
    {
        
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        playerAnimation.PlayHitAnimation();
        currentHealth -= CalculateDamage(damage);
        rb.AddForce(knockback, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }

    }

    private int CalculateDamage(int damage)
    {
        return damage;
    }
}
