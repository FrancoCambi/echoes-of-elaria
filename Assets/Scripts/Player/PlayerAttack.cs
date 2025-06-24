using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AttackDirection
{
    Left, Right, Up, Down
}

public class PlayerAttack : MonoBehaviour
{
    private Camera mainCamera;

    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;

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
    void Start()
    {

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();

        mainCamera = Camera.main;

        comboIndex = 0;
        lastAttackTime = 0;
        comboResetTime = 1f;
    }


    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && !attacking && !playerMovement.Dashing)
        {
            playerMovement.CanMove = false;
            playerMovement.StopMovement();
            SetUpAttack();
        }

        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboIndex = 0;
        }
    }


    private void SetUpAttack()
    {
        attacking = true;
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
        playerAnimation.PlayAttackAnimation();

    }

    private void Attack()
    {

        (Vector3 range, Vector3 offset, int angle) = CalculateHitBoxAndAngle();
        Vector3 size = range;

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(transform.position + offset, size, angle, LayerMask.GetMask("Enemies"));

        foreach (Collider2D col in hitEnemies)
        {
            int damage = CalculateDamage();
            int damageDealt = col.GetComponent<EnemyHealth>().OnHit(damage, Vector2.zero);
            PlayerManager.Instance.GainRage(CalculateRagePerAA(damageDealt));
        }

    }

    private int CalculateRagePerAA(int damageDealt)
    {
        // This probably needs to change
        return (30 * damageDealt) / (4 * PlayerManager.Instance.Level + 20);
    }

    private int CalculateDamage()
    {
        int minDamage = PlayerManager.Instance.MinDamage;
        int maxDamage = PlayerManager.Instance.MaxDamage;
        int rage = PlayerManager.Instance.CurrentRage;

        int randomDamage = UnityEngine.Random.Range(minDamage, maxDamage + 1);
        int rageAmplifiedDamage = (int)Mathf.Round(randomDamage * (1 + (float)rage / 500));

        return rageAmplifiedDamage;

    }

    private Tuple<Vector3, Vector3, int> CalculateHitBoxAndAngle()
    {

        float xRange;
        float yRange;
        float xOffset;
        float yOffset;
        int angle;

        if (attackDir == AttackDirection.Up)
        {
            xRange = 1f;
            yRange = 1.75f;
            xOffset = 0;
            yOffset = 0.5f;
            angle = 90;
        }
        else if (attackDir == AttackDirection.Down)
        {
            xRange = 1f;
            yRange = 1.75f;
            xOffset = 0;
            yOffset = -0.5f;
            angle = 90;

        }
        else if (attackDir == AttackDirection.Left)
        {
            xRange = 1.75f;
            yRange = 1f;
            xOffset = -0.5f;
            yOffset = 0f;
            angle = 0;

        }
        else
        {
            xRange = 1.75f;
            yRange = 1f;
            xOffset = 0.5f;
            yOffset = 0f;
            angle = 0;

        }

        return new Tuple<Vector3, Vector3, int>(new Vector3(xRange, yRange), new Vector3(xOffset, yOffset), angle);
    }

    /*private void OnDrawGizmos()
    {
        Vector3 range = new(1.5f, 1, 0);
        Vector3 size = range;
        Vector3 offset = new(0.5f, 0f, 0f);

        var oldMatrix = Gizmos.matrix;

        // create a matrix which translates an object by "position", rotates it by "rotation" and scales it by "halfExtends * 2"
        Gizmos.matrix = Matrix4x4.TRS(transform.position + offset, Quaternion.AngleAxis(90, new Vector3(0,0,1)), range);
        // Then use it one a default cube which is not translated nor scaled
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1));

        Gizmos.matrix = oldMatrix;
    }*/

    public void StopAttack()
    {
        attacking = false;
        playerMovement.CanMove = true;
        playerAnimation.PlayIdleAfterAttack();
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
