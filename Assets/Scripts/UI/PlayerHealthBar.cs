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
    }

    private void Start()
    {
        UpdateBar(PlayerManager.Instance.CurrentHealth);
    }

    private void UpdateBar(int currentHealth)
    {
        bar.fillAmount = currentHealth / (float)PlayerManager.Instance.MaxHealth;
        healthText.text = $"{currentHealth}/{PlayerManager.Instance.MaxHealth}";
    }

    private void OnDisable()
    {
        PlayerManager.OnCurrentHealthChanged -= UpdateBar;
    }
}
