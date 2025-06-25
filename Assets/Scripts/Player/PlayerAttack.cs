using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AttackDirection
{
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] LayerMask enemyMask;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;
        attackDir = GetAttackDirectionByAngle(angle);
        FlipRendererByDirection();

        IncreaseComboIndex();
        playerAnimation.PlayAttackAnimation();

    }

    private void Attack()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, PlayerManager.Instance.BasicAttackRange, enemyMask);

        foreach (Collider2D col in hitEnemies)
        {
            Vector3 direction = (col.transform.position - transform.position).normalized;

            if (CheckEnemyDirection(attackDir, direction) || comboIndex == 3)
            {
                int damage = CalculateDamage();
                int damageDealt = col.GetComponent<EnemyHealth>().OnHit(damage, Vector2.zero);
                PlayerManager.Instance.GainRage(CalculateRagePerAA(damageDealt));
            }

        }

    }

    private bool CheckEnemyDirection(AttackDirection attackDirection, Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        if (GetAttackDirectionByAngle(angle) == attackDirection)
        {
            return true;
        }

        return false;
    }

    private AttackDirection GetAttackDirectionByAngle(float angle)
    {
        Debug.Log(angle);
        AttackDirection dir;

        if (angle >= 337.5f || angle < 22.5f)
            dir = AttackDirection.Right;
        else if (angle < 67.5f)
            dir = AttackDirection.UpRight;
        else if (angle < 112.5f)
            dir = AttackDirection.Up;
        else if (angle < 157.5f)
            dir = AttackDirection.UpLeft;
        else if (angle < 202.5f)
            dir = AttackDirection.Left;
        else if (angle < 247.5f)
            dir = AttackDirection.DownLeft;
        else if (angle < 292.5f)
            dir = AttackDirection.Down;
        else
            dir = AttackDirection.DownRight;

        return dir;
    }

    private void FlipRendererByDirection()
    {
        if (attackDir == AttackDirection.Left || attackDir == AttackDirection.UpLeft
            || attackDir == AttackDirection.DownLeft)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
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


    /*private void OnDrawGizmos()
    {
        Vector3 range = new(1f, 0.25f, 0);
        Vector3 size = range;
        Vector3 offset = new(0.5f, 0.125f, 0f);

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

}
