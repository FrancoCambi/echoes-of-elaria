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

    private PlayerMovement playerMovement;

    private AttackDirection attackDir;

    private bool attacking;
    private int comboIndex;
    private float lastAttackTime;
    private float comboResetTime;


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

    public int ComboIndex
    {
        get
        {
            return comboIndex;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        mainCamera = Camera.main;

        comboIndex = 0;
        lastAttackTime = 0;
        comboResetTime = 1f;
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

        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboIndex = 0;
        }
    }


    void Attack()
    {
        lastAttackTime = Time.time;
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

        IncreaseComboIndex();
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

    private void IncreaseComboIndex()
    {
        comboIndex += 1;
        if (comboIndex > 3)
        {
            comboIndex = 1;
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
