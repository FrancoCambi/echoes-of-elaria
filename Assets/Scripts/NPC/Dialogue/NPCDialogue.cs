using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class NPCDialogue : MonoBehaviour
{
    private new Collider2D collider;

    private NPCData npcData;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        npcData = NPCDatabase.GetNPCData(NPCDatabase.GetIdByName(gameObject.name));
    }

    private List<string> GetDialogueLines()
    {
        List<string> keys = NPCDatabase.GetDialogueKeysByID(npcData.ID);

        List<string> lines = new();

        foreach (string key in keys)
        {
            lines.Add(LocalizationSettings.StringDatabase.GetLocalizedString("Dialogues", key));
        }

        return lines;
    }

    private bool PlayerInInteractRange()
    {
        Collider2D playerCol = PlayerManager.Instance.GetComponent<Collider2D>();

        return collider.Distance(playerCol).distance <= 1f;

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && PlayerInInteractRange() && !DialogueBoxManager.Instance.IsOpen)
        {
            _ = DialogueBoxManager.Instance.ShowDialogueBox(GetDialogueLines(), npcData.Name);
        }
    }
}
