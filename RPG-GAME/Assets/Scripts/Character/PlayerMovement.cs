using UnityEditor.Toolbars;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    private DBManager db;
    private PlayerStatsLoader statsLoader;
    private PlayerAttack playerAttack;

    private float movementSpeed;
    private float horizontal = 0f;
    private float vertical = 0f;
    private Vector2 movementDirection;
    private bool canMove;

    public float Horizontal
    {
        get
        {
            return horizontal;
        }
    }

    public float Vertical
    {
        get
        {
            return vertical;
        }
    }

    public bool CanMove
    {
        get
        {
            return canMove;
        }
        set
        {
            canMove = value;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        db = new("template.db");
        statsLoader = new PlayerStatsLoader(db);
        playerAttack = GetComponent<PlayerAttack>();

        movementSpeed = statsLoader.GetMovementSpeed();

        canMove = true;

        db.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            movementDirection = new Vector2(horizontal, vertical).normalized;
        }

    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            StopMovement();
            return;
        }

        if (horizontal > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }


        if (movementDirection != Vector2.zero)
        {

            rb.linearVelocity = movementDirection * movementSpeed;
        }
        else
        {
            StopMovement();
        }


    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
