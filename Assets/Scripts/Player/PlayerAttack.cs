using UnityEngine;
using UnityEngine.EventSystems;

public enum AttackDirection
{
    Left, Right, Up, Down
}

public class PlayerAttack : MonoBehaviour
{

    [SerializeField]
    BoxCollider2D sideHitbox;
    [SerializeField]
    BoxCollider2D frontHitbox;
    [SerializeField]
    BoxCollider2D backHitbox;

    private Camera mainCamera;

    private PlayerAnimation playerAnimation;
    private PlayerMovement playerMovement;

    private AttackDirection attackDir;

    private bool attacking;


    public AttackDirection AttackDir
    {
        get
        {
            return attackDir;
        }
    }

    public bool Attacking
    {
        get
        {
            return attacking;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();

        mainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && !attacking && !playerMovement.Dashing)
        {
            playerMovement.CanMove = false;
            playerMovement.StopMovement();
            Attack();
        }
    }


    void Attack()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                transform.localScale = new Vector2(1, 1);
                attackDir = AttackDirection.Right;
            }
            else
            {
                transform.localScale = new Vector2(-1, 1);
                attackDir = AttackDirection.Left;
            }
        }
        else
        {
            if (direction.y > 0)
            {
                attackDir = AttackDirection.Up;
            }
            else
            {
                attackDir = AttackDirection.Down;
            }
        }

        attacking = true;
    }

    public void StopAttack()
    {
        attacking = false;
        playerMovement.CanMove = true;
        sideHitbox.enabled = false;
        backHitbox.enabled = false;
        frontHitbox.enabled = false;
    }

    public void ActivateHitbox()
    {
        if (attackDir == AttackDirection.Right || attackDir == AttackDirection.Left)
        {
            sideHitbox.enabled = true;
        }
        else if (attackDir == AttackDirection.Up)
        {
            backHitbox.enabled = true;
        }
        else
        {
            frontHitbox.enabled = true;
        }
    }

    public string TypeToStringAnimation(AttackDirection attackDirection)
    {
        return attackDirection switch
        {
            AttackDirection.Left => "side",
            AttackDirection.Right => "side",
            AttackDirection.Up => "back",
            AttackDirection.Down => "front",
            _ => "",
        };
    }
}
