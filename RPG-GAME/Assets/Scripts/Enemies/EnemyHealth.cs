using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private int id;
    private int health;
    private string enemyName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        enemyName = gameObject.name.Replace("(Clone)", "");
        id = EnemyDataLoader.Instance.GetIdByName(enemyName);
        health = EnemyDataLoader.Instance.GetHealth(id);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnHit(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

}
