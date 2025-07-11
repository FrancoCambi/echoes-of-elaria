using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Image bar;
    private TextMeshProUGUI healthText;

    private void Awake()
    {
        bar = GetComponent<Image>();
        healthText = GetComponentInChildren<TextMeshProUGUI>();

        PlayerManager.OnCurrentHealthChanged += UpdateBar;
        PlayerManager.OnMaxHealthChanged += UpdateBar;
    }

    private void Start()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        bar.fillAmount = PlayerManager.Instance.CurrentHealth / (float)PlayerManager.Instance.MaxHealth;
        healthText.text = $"{PlayerManager.Instance.CurrentHealth}/{PlayerManager.Instance.MaxHealth}";
    }

    private void OnDisable()
    {
        PlayerManager.OnCurrentHealthChanged -= UpdateBar;
        PlayerManager.OnMaxHealthChanged -= UpdateBar;
    }
}
