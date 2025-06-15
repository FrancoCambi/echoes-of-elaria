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
        UpdateText();
    }
    private void UpdateText()
    {
        text.text = PlayerManager.Instance.Level.ToString();
    }

    private void OnDisable()
    {
        PlayerManager.OnLevelUp -= UpdateText;
    }
}

