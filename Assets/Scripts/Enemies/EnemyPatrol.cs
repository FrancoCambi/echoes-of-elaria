using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private CircleCollider2D patrolZone;
    private Rigidbody2D rb;
    private GameObject parent;

    private EnemyChaseZone enemyChaseZone;
    private EnemyAttackZone enemyAttackZone;
    private EnemyHealth enemyHealth;

    private Vector2 initialCentre;

    private int id;
    private int maxPatrolCD;
    private float patrolSpeed;
    private bool canPatrol = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        patrolZone = GetComponent<CircleCollider2D>();
        rb = GetComponentInParent<Rigidbody2D>();
        parent = transform.parent.gameObject;

        enemyAttackZone = transform.parent.Find("AttackZone").GetComponent<EnemyAttackZone>();
        enemyChaseZone = transform.parent.Find("ChaseZone").GetComponent<EnemyChaseZone>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        initialCentre = parent.transform.position;

        id = EnemyDataLoader.Instance.GetIdByName(parent.name);
        maxPatrolCD = EnemyDataLoader.Instance.GetMaxPatrolCD(id);
        patrolSpeed = EnemyDataLoader.Instance.GetPatrolSpeed(id);
        
    }

    // Update is called once per frame
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
        Vector3 wanderPoint = GetRandomPointInsideCircunference(patrolZone, initialCentre);
        Vector3 direction = (wanderPoint - parent.transform.position).normalized;

        while (Vector3.Distance(wanderPoint, parent.transform.position) > 0.05f && CanKeepPatrolling())
        {
            rb.linearVelocity = (patrolSpeed * direction);
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        float wanderCd = UnityEngine.Random.Range(0, maxPatrolCD + 1);
        yield return new WaitForSeconds(wanderCd);
        canPatrol = true;
    }

    private Vector2 GetRandomPointInsideCircunference(CircleCollider2D circle, Vector2 centre)
    {
        float randomX = UnityEngine.Random.Range(-circle.radius, circle.radius);

        float maxY = Mathf.Sqrt(Mathf.Pow(circle.radius, 2) - Mathf.Pow(randomX, 2));

        float randomY = UnityEngine.Random.Range(-maxY, maxY);

        return centre + new Vector2(randomX, randomY);
    }

    private bool CanKeepPatrolling()
    {
        return !enemyAttackZone.PlayerCollider && !enemyChaseZone.PlayerCollider;
    }

    private Vector2 GetRandomPointInCircunference(CircleCollider2D circle)
    {

        Vector2 localCentre = circle.offset;
        float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

        float x = localCentre.x + circle.radius * Mathf.Cos(angle);
        float y = localCentre.y + circle.radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}
