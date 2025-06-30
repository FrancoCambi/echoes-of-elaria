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
        if (KeyBindsManager.Instance.Listening) return;

        Close();
    }

    public void OptionsButton()
    {
        if (KeyBindsManager.Instance.Listening) return;

        OptionsManager.Instance.Open();
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
        if (KeyBindsManager.Instance.Listening) return;

        Application.Quit();
    }

    public override void Close()
    {
        base.Close();

        KeyBindsManager.Instance.StopListening();
    }
}
