using UnityEngine;

public enum AttackDirection
{
    Left, Right, Up, Down
}

public class PlayerAttack : MonoBehaviour
{

    private Camera mainCamera;
    private Animator animator;

    private PlayerAnimation playerAnimation;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private SwordHitbox hitbox;

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
        playerHealth = GetComponent<PlayerHealth>();
        hitbox = GetComponentInChildren<SwordHitbox>();

        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking && !playerMovement.Dashing)
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
        Invoke(nameof(StopAttack), playerAnimation.FindAnimationByName("attack_" + TypeToStringAnimation(attackDir)).length);
    }

    void StopAttack()
    {
        attacking = false;
        playerMovement.CanMove = true;
    }

    private void ActivateHitbox()
    {
        hitbox.ActivateHitbox();
    }

    private void DeactivateHitbox()
    {
        hitbox.DeactivateHitbox();
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
