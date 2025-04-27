using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyAttackZone attackZone;
    private Rigidbody2D rb;

    private int id;
    private float movementSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackZone = GetComponentInChildren<EnemyAttackZone>();
        rb = GetComponent<Rigidbody2D>();

        id = EnemyDataLoader.Instance.GetIdByName(gameObject.name);
        movementSpeed = EnemyDataLoader.Instance.GetMovementSpeed(id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
