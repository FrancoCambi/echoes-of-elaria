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
        UpdateBar();
    }

    private void UpdateBar()
    {
        bar.fillAmount = PlayerManager.Instance.CurrentRage / (float)PlayerManager.Instance.MaxRage;
        resourceText.text = $"{PlayerManager.Instance.CurrentRage}/{PlayerManager.Instance.MaxRage}";
    }

    private void OnDisable()
    {
        PlayerManager.OnCurrentRageChanged -= UpdateBar;
    }
}
    