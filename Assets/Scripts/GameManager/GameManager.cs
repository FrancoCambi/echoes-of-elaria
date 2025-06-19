using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public enum Language { English, Spanish }
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }
            return instance;
        }
    }

    public static int SelCharID { get; set; }

    public static Language CurrentLanguage { get; set; }

    private void Awake()
    {
        SelCharID = 1;

        StartCoroutine(nameof(LoadLanguage));
    }

    public static void SwitchLanguage(Language newLanguage)
    {
        if (newLanguage == Language.English)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        }
        CurrentLanguage = newLanguage;
        SaveLanguage();
    }

    private static void SaveLanguage()
    {
        if (CurrentLanguage == Language.English)
        {
            PlayerPrefs.SetInt("language", 0);
        }
        else
        {
            PlayerPrefs.SetInt("language", 1);
        }
        PlayerPrefs.Save();
    }

    private IEnumerator LoadLanguage()
    {
        yield return LocalizationSettings.InitializationOperation;
        int languageIndex = PlayerPrefs.GetInt("language", -1);
        if (languageIndex == -1) yield return null;

        if (languageIndex == 0)
        {
            SwitchLanguage(Language.English);
        }
        else
        {
            SwitchLanguage(Language.Spanish);
        }

    }
}
