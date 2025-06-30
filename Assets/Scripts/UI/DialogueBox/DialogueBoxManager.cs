using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueBoxManager : Panel
{
    private static DialogueBoxManager instance;

    public static DialogueBoxManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<DialogueBoxManager>();
            return instance;
        }
    }

    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private float lettersPerSecond;

    private void Start()
    {
        lettersPerSecond = 25f;
    }
    
    public async Task ShowDialogueBox(List<string> lines, string npcName)
    {
        // Set state to dialogue so the player can't move, use 
        // spells, etc.
        GameManager.Instance.SetGameState(GameState.Dialogue);
        npcNameText.text = npcName;
        Open();

        // Foreach line, type it, waiting for it to end
        // before typing the next
        foreach (string line in lines)
        {
            await TypeLine(line);
        }

        // Change the state again and close the window.
        GameManager.Instance.SetGameState(GameState.Playing);
        Close();

    }

    private async Task TypeLine(string line)
    {
        dialogueText.text = "";

        // Write char by char
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            await Task.Delay((int)((1f / lettersPerSecond) * 1000));
        }

        // Wait for the player to press E.
        await WaitForKeyDown(KeyCode.E);
    }

    private async Task WaitForKeyDown(KeyCode key)
    {
        // Wait until key is pressed
        while (!Input.GetKeyDown(key))
            await Task.Yield();
    }

}
