using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GeneralStatsText : MonoBehaviour
{
    private TextMeshProUGUI statsText;

    private void Awake()
    {
        statsText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText();
    }

    private void OnEnable()
    {
        PlayerManager.OnStatsChanged += UpdateText;
    }

    private void UpdateText()
    {
        int health = PlayerManager.Instance.MaxHealth;
        int itemLevel = PlayerManager.Instance.ItemLevel;
        int speed = (int)PlayerManager.Instance.MovementSpeed; // THIS NEEDS FIXING

        string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString("Ui", "GeneralStatsText");

        statsText.text = string.Format(localizedString, health, itemLevel, speed);
    }

    private void OnDisable()
    {
        PlayerManager.OnStatsChanged -= UpdateText;
    }
}
