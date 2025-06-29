using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Material originalMaterial;
    private Material flashMaterial;
    private Animator animator;
    private RuntimeAnimatorController controller;
    private SpriteRenderer spriteRenderer;

    private EnemyJumpAttack enemyAttack;
    private EnemyHealth enemyHealth;
    private EnemyData enemyData;

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
    private const string HIT = "hit";

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = Resources.Load<Material>("SRMaterials/Default");
        flashMaterial = Resources.Load<Material>("SRMaterials/Flash");

        enemyAttack = GetComponent<EnemyJumpAttack>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyData = EnemyDatabase.GetEnemyData(EnemyDatabase.GetIdByName(gameObject.name));

        controller = Resources.Load<RuntimeAnimatorController>($"AnimationControllers/Mobs/{enemyData.Name}");
        animator.runtimeAnimatorController = controller;
        currentState = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    void Update()
    {
        if (!enemyHealth.IsAlive) return;

        if (enemyAttack.Attacking)
        {
            ChangeAnimationState(ATTACK_FRONT);
        }
        else
        {
            ChangeAnimationState(IDLE_FRONT);
        }
    }

    public void DeathAnimation()
    {
        ChangeAnimationState("death");
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    private IEnumerator HitAnimation()
    {

        float flashTime = 0.2f;

        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.material = originalMaterial;
    }
    public void StartFlash()
    {
        StartCoroutine(nameof(HitAnimation));
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
