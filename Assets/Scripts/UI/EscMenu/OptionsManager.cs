using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class OptionsManager : Panel
{
    private static OptionsManager instance;

    public static OptionsManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<OptionsManager>();
            return instance;
        }
    }

    public void LanguageButton()
    {
        if (GameManager.CurrentLanguage == Language.English)
        {
            GameManager.SwitchLanguage(Language.Spanish);
        }
        else
        {
            GameManager.SwitchLanguage(Language.English);
        }
    }

    public void DifficultyButton()
    {

    }
    
   

}
