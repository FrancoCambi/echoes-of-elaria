using TMPro;
using UnityEngine;

public class PlayerLevelText : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        PlayerManager.OnLevelUp += UpdateText;
    }
    private void Start()
    {
        UpdateText(PlayerManager.Instance.Level);
    }
    private void UpdateText(int newLevel)
    {
        text.text = newLevel.ToString();
    }

    private void OnDisable()
    {
        PlayerManager.OnLevelUp -= UpdateText;
    }
}

