using UnityEngine;

public class SwordHitbox : MonoBehaviour
{

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().OnHit(PlayerManager.Instance.MaxDamage, Vector2.zero);
            PlayerManager.Instance.GainRage(10);
        }
    }

    public void ActivateHitbox()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public void DeactivateHitbox()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
