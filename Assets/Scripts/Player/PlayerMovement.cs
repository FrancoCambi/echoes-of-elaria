using System.Collections;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    private PlayerHealth playerHealth;

    private Vector2 movementDirection;

    private int charID;
    private float movementSpeed;
    private float horizontal = 0f;
    private float vertical = 0f;
    private bool canMove;
    private bool canDash;
    private bool dashing;
    private float dashingTime = 0.3f;
    private float dashForce;
    private float dashCD;

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

        playerHealth = GetComponent<PlayerHealth>();

        charID = GameManager.Instance.SelCharID;
        movementSpeed = PlayerDataLoader.Instance.GetMovementSpeed(charID);
        dashForce = PlayerDataLoader.Instance.GetDashForce(charID);
        dashCD = PlayerDataLoader.Instance.GetDashCD(charID);

        canMove = true;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && playerHealth.IsAlive)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            movementDirection = new Vector2(horizontal, vertical).normalized;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(nameof(Dash));
        }

    }

    private void FixedUpdate()
    {

        if (canMove && playerHealth.IsAlive)
        {
            if (horizontal > 0)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }


            if (movementDirection != Vector2.zero && !dashing)
            {

                rb.linearVelocity = movementDirection * movementSpeed;
            }
            else if (movementDirection == Vector2.zero && !dashing && !playerHealth.Invincible)
            {
                StopMovement();
            }
        }
        


    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void NotAllowMovement()
    {
        canMove = false;

    }

    private void AllowMovement()
    {
        canMove = true;
    }


    public IEnumerator Dash()
    {
        canDash = false;
        canMove = false;
        dashing = true;

        if (horizontal < 0 && vertical == 0)
        {
            rb.linearVelocity = new Vector2(-dashForce, rb.linearVelocity.y);
        }
        else if (horizontal > 0 && vertical == 0)
        {
            rb.linearVelocity = new Vector2(dashForce, rb.linearVelocity.y);

        }
        else if (vertical > 0 && horizontal == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, dashForce);
        }
        else if (vertical < 0 && horizontal == 0)
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
            rb.linearVelocity = new Vector2(transform.localScale.x, 0) * dashForce;
        }
        yield return new WaitForSeconds(dashingTime);
        dashing = false;
        canMove = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }
}
