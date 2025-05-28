using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceBar : MonoBehaviour
{
    private Image bar;
    private TextMeshProUGUI resourceText;

    private void Awake()
    {
        bar = GetComponent<Image>();

        resourceText = GetComponentInChildren<TextMeshProUGUI>();

        PlayerManager.OnCurrentRageChanged += UpdateBar;
    }

    private void Start()
    {
        UpdateBar(PlayerManager.Instance.CurrentRage);
    }

    private void UpdateBar(int currentRage)
    {
        bar.fillAmount = currentRage / (float)PlayerManager.Instance.MaxRage;
        resourceText.text = $"{currentRage}/{PlayerManager.Instance.MaxRage}";
    }

    private void OnDisable()
    {
        PlayerManager.OnCurrentRageChanged -= UpdateBar;
    }
}
    