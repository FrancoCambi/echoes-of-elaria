using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Material flashMaterial;

    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

    private string currentState;
    private bool alreadyInvincible;
    private bool playingHit;

    private const string IDLE_FRONT = "idle_front";
    private const string IDLE_SIDE = "idle_side";
    private const string IDLE_BACK = "idle_back";
    private const string WALK_FRONT = "walk_front";
    private const string WALK_SIDE = "walk_side";
    private const string WALK_BACK = "walk_back";
    private const string ATTACK_FRONT = "attack_front";
    private const string ATTACK_SIDE = "attack_side";
    private const string ATTACK_BACK = "attack_back";
    private const string HIT = "hit";


    public string CurrentState
    {
        get
        {
            return currentState;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = Resources.Load<Material>("SRMaterials/Default");
        flashMaterial = Resources.Load<Material>("SRMaterials/Flash");

        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();


        currentState = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        alreadyInvincible = false;
        playingHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playingHit || playerAttack.Attacking || playerMovement.Dashing)
        {
            return;
        }


        if (playerHealth.Invincible && !alreadyInvincible)
        {
            StartCoroutine(nameof(HitAnimation));
 
        }
        else
        {

            if (playerMovement.Horizontal > 0)
            {
                ChangeAnimationState(WALK_SIDE);
            }
            else if (playerMovement.Horizontal < 0)
            {
                ChangeAnimationState(WALK_SIDE);
            }
            else if (playerMovement.Vertical > 0)
            {
                ChangeAnimationState(WALK_BACK);
            }
            else if (playerMovement.Vertical < 0)
            {
                ChangeAnimationState(WALK_FRONT);
            }
            else
            {
                ChangeAnimationState(currentState.Replace("walk_", "idle_"));
            }
        }

    }

    private IEnumerator HitAnimation()
    {
        alreadyInvincible = true;
        playingHit = true;
        float flashTime = 0.2f;

        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.material = originalMaterial;

        playingHit = false;

        Color originalColor = spriteRenderer.color;
        float elapsed = 0f;
        float flashInterval = 0.3f;
        float minAlpha = 0.5f;

        while (elapsed < playerHealth.InvincibilityTime - flashTime)
        {
            // Fade out
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, minAlpha);
            yield return new WaitForSeconds(flashInterval / 2f);

            // Full alpha
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            yield return new WaitForSeconds(flashInterval / 2f);

            elapsed += flashInterval;
        }

        spriteRenderer.color = originalColor;
        alreadyInvincible = false;
    }

    public void PlayAttackAnimation()
    {
        ChangeAnimationState("attack_" + TypeToStringAnimation(playerAttack.AttackDir) + "_" + playerAttack.ComboIndex);
    }

    public void PlayIdleAfterAttack()
    {
        // THIS NEEDS TO CHANGE
        string anim = TypeToStringAnimation(playerAttack.AttackDir);

        if (anim == "back_side")
        {
            ChangeAnimationState("idle_back");
        }
        else if (anim == "front_side")
        {
            ChangeAnimationState("idle_front");
        }
        else
        {
            ChangeAnimationState("idle_" + TypeToStringAnimation(playerAttack.AttackDir));
        }

    }
    
    public void PlayDashAnimation()
    {
        ChangeAnimationState(currentState.Replace("idle_", "dash_").Replace("walk_", "dash_"));
    }

    public void PlayIdleAfterDash()
    {
        ChangeAnimationState(currentState.Replace("dash_", "idle_"));
    }


    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
    private string TypeToStringAnimation(AttackDirection attackDirection)
    {
        return attackDirection switch
        {
            AttackDirection.Left => "side",
            AttackDirection.Right => "side",
            AttackDirection.Up => "back",
            AttackDirection.Down => "front",
            AttackDirection.UpRight => "back_side",
            AttackDirection.UpLeft => "back_side",
            AttackDirection.DownRight => "front_side",
            AttackDirection.DownLeft => "front_side",
            _ => "",
        };
    }
    public AnimationClip FindAnimationByName(string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }

}
