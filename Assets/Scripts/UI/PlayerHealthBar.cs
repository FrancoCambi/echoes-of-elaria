using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Image bar;
    private TextMeshProUGUI healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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
}
