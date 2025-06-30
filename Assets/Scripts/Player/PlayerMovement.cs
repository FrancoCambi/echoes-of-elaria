using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private PlayerHealth playerHealth;
    private PlayerAnimation playerAnimation;

    private Vector2 movementDirection;

    private int charID;
    private float horizontal = 0f;
    private float vertical = 0f;
    private bool canMove;
    private bool canDash;
    private bool dashing;
    private float dashingTime = 0.3f;

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

    public bool Dashing
    {
        get
        {
            return dashing;
        }
    }

    public bool IsMoving
    {
        get
        {
            return horizontal != 0 || vertical != 0;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerHealth = GetComponent<PlayerHealth>();
        playerAnimation = GetComponent<PlayerAnimation>();

        charID = GameManager.SelCharID;

        canMove = true;
        canDash = true;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += StopMovement;
    }

    // Update is called once per frame
    void Update()
    {
        // If Game State blocks input, just return
        if (GameManager.Instance.IsInputBlocked()) return;

        // Otherwise, read input.
        if (canMove && playerHealth.IsAlive)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            movementDirection = new Vector2(horizontal, vertical).normalized;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash && canMove)
        {
            StartCoroutine(nameof(Dash));
            playerAnimation.PlayDashAnimation();
        }

    }

    private void FixedUpdate()
    {
        // If Game State blocks input, just return
        if (GameManager.Instance.IsInputBlocked()) return;

        // Otherwise, read input
        if (canMove && playerHealth.IsAlive)
        {
            if (horizontal > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontal < 0)
            {
                spriteRenderer.flipX = true;

            }


            if (movementDirection != Vector2.zero && !dashing)
            {

                rb.linearVelocity = movementDirection * PlayerManager.Instance.MovementSpeed;
            }
            else if (movementDirection == Vector2.zero && !dashing)
            {
                StopMovement();
            }
        }
        


    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        horizontal = 0;
        vertical = 0;
    }

    public IEnumerator Dash()
    {
        canDash = false;
        canMove = false;
        dashing = true;
        float dashForce = PlayerManager.Instance.DashForce;

        if (horizontal < 0 && vertical == 0 || (playerAnimation.CurrentState == "idle_side"))
        {
            rb.linearVelocity = new Vector2(-dashForce, rb.linearVelocity.y);
        }
        else if (horizontal > 0 && vertical == 0 || (playerAnimation.CurrentState == "idle_side"))
        {
            rb.linearVelocity = new Vector2(dashForce, rb.linearVelocity.y);

        }
        else if (vertical > 0 && horizontal == 0 || (playerAnimation.CurrentState == "idle_back"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, dashForce);
        }
        else if (vertical < 0 && horizontal == 0 || (playerAnimation.CurrentState == "idle_front"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -dashForce);
        }
        else if (horizontal < 0 && vertical > 0)
        {
            float diagonalDashForce = Mathf.Sqrt(Mathf.Pow(dashForce, 2) / 2);
            rb.linearVelocity = new Vector2(-diagonalDashForce, diagonalDashForce);
        }
        else if (horizontal < 0 && vertical < 0)
        {
            float diagonalDashForce = Mathf.Sqrt(Mathf.Pow(dashForce, 2) / 2);
            rb.linearVelocity = new Vector2(-diagonalDashForce, -diagonalDashForce);
        }
        else if (horizontal > 0 && vertical > 0)
        {
            float diagonalDashForce = Mathf.Sqrt(Mathf.Pow(dashForce, 2) / 2);
            rb.linearVelocity = new Vector2(diagonalDashForce, diagonalDashForce);
        }
        else if (horizontal > 0 && vertical < 0)
        {
            float diagonalDashForce = Mathf.Sqrt(Mathf.Pow(dashForce, 2) / 2);
            rb.linearVelocity = new Vector2(diagonalDashForce, -diagonalDashForce);
        }
        else
        {
            int x = spriteRenderer.flipX ? -1 : 1;
            
            rb.linearVelocity = new Vector2(x, 0) * dashForce;
        }
        yield return new WaitForSeconds(dashingTime);
        playerAnimation.PlayIdleAfterDash();
        dashing = false;
        canMove = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(PlayerManager.Instance.DashCD);
        canDash = true;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= StopMovement;
    }
}
