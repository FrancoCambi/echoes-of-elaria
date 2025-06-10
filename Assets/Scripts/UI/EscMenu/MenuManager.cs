using UnityEditor;
using UnityEngine;

public class MenuManager : Panel
{
    private static MenuManager instance;

    public static MenuManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<MenuManager>();
            return instance;
        }
    }

    public void PlayButton()
    {
        Close();
    }

    public void OptionsButton()
    {
        
    }

    public void KeysButton()
    {
        if (KeyBindsManager.Instance.Listening)
        {
            KeyBindsManager.Instance.StopListening();
        }
        else
        {
            KeyBindsManager.Instance.StartListening();
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public override void Close()
    {
        base.Close();

        KeyBindsManager.Instance.StopListening();
    }
}
