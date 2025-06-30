using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public enum Language
{
    English, 
    Spanish,
}

public enum GameState
{
    Dialogue,
    Playing,
}
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

    public static event Action OnGameStateChanged;

    public static int SelCharID { get; set; }

    public static Language CurrentLanguage { get; set; }

    public static GameState CurrentState { get; private set; }

    private void Awake()
    {
        SelCharID = 1;
        CurrentState = GameState.Playing;

        StartCoroutine(nameof(LoadLanguage));
    }

    /*TESTING*/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            MobFactory.SpawnMob(1, new Vector3(3, 3, 0));
        }
    }

    /*TESTING*/

    #region language

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
    #endregion

    #region game state

    public void SetGameState(GameState gameState)
    {
        CurrentState = gameState;
        OnGameStateChanged?.Invoke();
    }

    public bool IsInputBlocked()
    {
        return CurrentState == GameState.Dialogue;
    }

    #endregion
}
