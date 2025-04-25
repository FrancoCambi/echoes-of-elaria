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
            other.GetComponent<EnemyHealth>().OnHit(PlayerStatsLoader.Instance.GetDamageById(1));
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
