using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public enum DeltaTimeType
    {
        Smooth,
        Unscaled
    }

    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private DeltaTimeType deltaType = DeltaTimeType.Smooth;

    private Dictionary<int, string> cachedNumberStrings = new();

    private int[] frameRateSamples;
    private int cacheNumbersAmount = 500;
    private int averageFromAmount = 30;
    private int averageCounter;
    private int currentAveraged;

    void Awake()
    {
        // Cache strings and create array
        {
            for (int i = 0; i < cacheNumbersAmount; i++)
            {
                cachedNumberStrings[i] = i.ToString();
            }

            frameRateSamples = new int[averageFromAmount];
        }
    }

    void Update()
    {
        // Sample
        {
            int currentFrame = (int)Math.Round(1f / deltaType switch
            {
                DeltaTimeType.Smooth => Time.smoothDeltaTime,
                DeltaTimeType.Unscaled => Time.unscaledDeltaTime,
                _ => Time.unscaledDeltaTime
            });
            frameRateSamples[averageCounter] = currentFrame;
        }

        // Average
        {
            float average = 0f;

            foreach (var frameRate in frameRateSamples)
            {
                average += frameRate;
            }

            currentAveraged = (int)Math.Round(average / averageFromAmount);
            averageCounter = (averageCounter + 1) % averageFromAmount;
        }

        // Assign to UI
        {
            fpsText.text = currentAveraged switch
            {
                int x when x >= 0 && x < cacheNumbersAmount => cachedNumberStrings[x],
                int x when x >= cacheNumbersAmount => $"> {cacheNumbersAmount}",
                int x when x < 0 => "< 0",
                _ => "?"
            };
        }

       
    }
}