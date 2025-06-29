using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    private Animator animator;
    private RuntimeAnimatorController controller;

    private NPCData npcData;

    void Start()
    {
        animator = GetComponent<Animator>();

        npcData = NPCDatabase.GetNPCData(NPCDatabase.GetIdByName(gameObject.name));

        controller = Resources.Load<RuntimeAnimatorController>($"AnimationControllers/NPC/{npcData.Name}");
        animator.runtimeAnimatorController = controller;
    }

}
