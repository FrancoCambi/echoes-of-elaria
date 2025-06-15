using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{
    private Image bar;
    private TextMeshProUGUI healthText;

    private void Awake()
    {
        bar = GetComponent<Image>();
        healthText = GetComponentInChildren<TextMeshProUGUI>();

    }

    private void Start()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        int maxXp = XpManager.Instance.CalculateMaxXp(PlayerManager.Instance.Level);
        int currentXp = PlayerManager.Instance.CurrentXp;

        string localizedTitle = LocalizationSettings.StringDatabase.GetLocalizedString("Ui", "XpBar");

        bar.fillAmount = currentXp / (float)maxXp;
        healthText.text = $"{localizedTitle}: {currentXp}/{maxXp} ({Math.Round(currentXp/(float)maxXp*100, 2)}%)";
    }

    private void OnLocaleChanged(Locale newLocale)
    {
        UpdateBar();
    }

    private void OnEnable()
    {
        PlayerManager.OnLevelUp += UpdateBar;
        PlayerManager.OnXpGained += UpdateBar;
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDestroy()
    {
        PlayerManager.OnLevelUp -= UpdateBar;
        PlayerManager.OnXpGained -= UpdateBar;
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;

    }
}
