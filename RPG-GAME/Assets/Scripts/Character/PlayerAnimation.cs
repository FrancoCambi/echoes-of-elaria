using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    private string currentState;

    private const string IDLE_FRONT = "idle_front";
    private const string IDLE_SIDE = "idle_side";
    private const string IDLE_BACK = "idle_back";
    private const string WALK_FRONT = "walk_front";
    private const string WALK_SIDE = "walk_side";
    private const string WALK_BACK = "walk_back";
    private const string ATTACK_FRONT = "attack_front";
    private const string ATTACK_SIDE = "attack_side";
    private const string ATTACK_BACK = "attack_back";


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
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();

        currentState = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerAttack.Attacking)
        {
            ChangeAnimationState("attack_" + playerAttack.TypeToStringAnimation(playerAttack.AttackDir));
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
                if (!playerAttack.Attacking)
                {
                    ChangeAnimationState(currentState.Replace("attack_", "idle_"));
                }
            }
        }


    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    public AnimationClip FindAnimationByName(Animator animator, string name)
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
