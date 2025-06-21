using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;

    private EnemyChase enemyChaseZone;
    private EnemyAttackZone enemyAttackZone;
    private EnemyHealth enemyHealth;
    private EnemyData data;

    private int id;
    private bool canPatrol = true;

    private Vector3 respawnPoint;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();

        enemyAttackZone = GetComponentInChildren<EnemyAttackZone>();
        enemyChaseZone = GetComponentInChildren<EnemyChase>();
        enemyHealth = GetComponent<EnemyHealth>();


        id = EnemyDatabase.GetIdByName(gameObject.name);
        data = EnemyDatabase.GetEnemyData(id);

        respawnPoint = transform.position;
        
    }

    void Update()
    {
        if (enemyHealth.IsAlive && canPatrol && CanKeepPatrolling())
        {
            StartCoroutine(nameof(Wander));
        }
    }

    private IEnumerator Wander()
    {
        canPatrol = false;
        Vector3 wanderPoint = GetRandomPointByMaxDistance(3);
        Vector3 direction = (wanderPoint - transform.position).normalized;

        while (Vector3.Distance(wanderPoint, transform.position) > 0.05f && CanKeepPatrolling())
        {
            rb.linearVelocity = (data.PatrolSpeed * direction);
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        float wanderCd = Random.Range(0, data.MaxPatrolCD + 1);
        yield return new WaitForSeconds(wanderCd);
        canPatrol = true;
    }

    private Vector2 GetRandomPointByMaxDistance(float maxDistance)
    {
        float randomX = Random.Range(-maxDistance, maxDistance);

        float maxY = Mathf.Sqrt(Mathf.Pow(maxDistance, 2) - Mathf.Pow(randomX, 2));

        float randomY = Random.Range(-maxY, maxY);

        return respawnPoint + new Vector3(randomX, randomY);
    }


    private bool CanKeepPatrolling()
    {
        return !enemyAttackZone.PlayerInRange && !enemyChaseZone.PlayerInSight;
    }

    /*private Vector2 GetRandomPointInsideCircunference(CircleCollider2D circle, Vector2 centre)
    {
        float randomX = UnityEngine.Random.Range(-circle.radius, circle.radius);

        float maxY = Mathf.Sqrt(Mathf.Pow(circle.radius, 2) - Mathf.Pow(randomX, 2));

        float randomY = UnityEngine.Random.Range(-maxY, maxY);

        return centre + new Vector2(randomX, randomY);
    }
    private Vector2 GetRandomPointInCircunference(CircleCollider2D circle)
    {

        Vector2 localCentre = circle.offset;
        float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

        float x = localCentre.x + circle.radius * Mathf.Cos(angle);
        float y = localCentre.y + circle.radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }*/
}
