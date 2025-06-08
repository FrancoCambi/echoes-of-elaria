using UnityEngine;

public class SwordHitbox : MonoBehaviour
{

    private BoxCollider2D hitbox;

    private void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            int damage = CalculateDamage();
            int damageDealt = other.GetComponent<EnemyHealth>().OnHit(damage, Vector2.zero);
            PlayerManager.Instance.GainRage(CalculateRagePerAA(damageDealt));
            hitbox.enabled = false;
        }
    }

    private int CalculateRagePerAA(int damageDealt)
    {
        // This probably needs to change
        return (30 * damageDealt) / (4 * PlayerManager.Instance.Level + 20);
    }

    private int CalculateDamage()
    {
        int minDamage = PlayerManager.Instance.MinDamage;
        int maxDamage = PlayerManager.Instance.MaxDamage;
        int rage = PlayerManager.Instance.CurrentRage;

        int randomDamage = Random.Range(minDamage, maxDamage + 1);
        int rageAmplifiedDamage = (int)Mathf.Round(randomDamage * (1 + (float)rage / 500));

        return rageAmplifiedDamage;


    }

}
