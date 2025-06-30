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

    private bool isTyping;
    private float lettersPerSecond;

    private void Start()
    {
        lettersPerSecond = 25f;
    }
    
    public async Task ShowDialogueBox(List<string> lines, string npcName)
    {
        GameManager.Instance.SetGameState(GameState.Dialogue);
        npcNameText.text = npcName;
        Open();

        foreach (string line in lines)
        {
            await TypeLine(line);
        }

        GameManager.Instance.SetGameState(GameState.Playing);
        Close();

    }

    private async Task TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            await Task.Delay((int)((1f / lettersPerSecond) * 1000));
        }

        await WaitForKeyDown(KeyCode.E);
    }

    private async Task WaitForKeyDown(KeyCode key)
    {
        // Wait until key is pressed
        while (!Input.GetKeyDown(key))
            await Task.Yield();
    }

}
