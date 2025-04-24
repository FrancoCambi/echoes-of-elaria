using UnityEngine;

public enum AttackDirection
{
    Left, Right, Up, Down
}

public class PlayerAttack : MonoBehaviour
{

    private PlayerAnimation playerAnimation;
    private PlayerMovement playerMovement;

    private AttackDirection attackDir;
    private bool attacking;

    private Camera mainCamera;
    private Animator animator;

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
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            playerMovement.CanMove = false;
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
        Invoke(nameof(StopAttack), playerAnimation.FindAnimationByName(animator, "attack_" + TypeToStringAnimation(attackDir)).length);
    }

    void StopAttack()
    {
        attacking = false;
        playerMovement.CanMove = true;
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
