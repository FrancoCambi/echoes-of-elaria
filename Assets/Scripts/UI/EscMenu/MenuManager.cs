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

    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
