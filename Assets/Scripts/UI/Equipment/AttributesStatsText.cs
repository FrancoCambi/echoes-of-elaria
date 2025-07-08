using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AttributesStatsText : MonoBehaviour
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
        int armor = PlayerManager.Instance.Armor;
        int stamina = PlayerManager.Instance.Stamina;
        int intellect = PlayerManager.Instance.Intellect;
        int arcanePower = PlayerManager.Instance.ArcanePower;

        string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString("Ui", "AttributesStatsText");

        statsText.text = string.Format(localizedString, armor, stamina, intellect, arcanePower);
    }

    private void OnDisable()
    {
        PlayerManager.OnStatsChanged -= UpdateText;
    }
}
