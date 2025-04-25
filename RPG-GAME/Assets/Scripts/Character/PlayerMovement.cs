using System.Collections;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    private DBManager db;
    private PlayerStatsLoader statsLoader;
    private PlayerAttack playerAttack;
    private PlayerAnimation playerAnimation;

    private float movementSpeed;
    private float horizontal = 0f;
    private float vertical = 0f;
    private Vector2 movementDirection;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerAttack = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();

        // NOTE: ID IS HARDCODED HERE, WILL NEED TO BE DYNAMIC.
        movementSpeed = PlayerStatsLoader.Instance.GetMovementSpeedById(1);
        dashForce = PlayerStatsLoader.Instance.GetDashForceById(1);
        dashCD = PlayerStatsLoader.Instance.GetDashCDById(1);

        canMove = true;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
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

        if (canMove)
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
            else if (movementDirection == Vector2.zero && !dashing)
            {
                StopMovement();
            }
        }
        


    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
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
