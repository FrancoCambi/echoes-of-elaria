using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumePercentageText : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private TextMeshProUGUI percentageText;

    private void Awake()
    {
        percentageText = GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        percentageText.text = $"{Math.Round(slider.value * 100)}%";
    }
}
