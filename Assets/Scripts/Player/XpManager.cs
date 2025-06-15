using UnityEngine;

public class XpManager : MonoBehaviour
{
    private static XpManager instance;

    public static XpManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<XpManager>();
            return instance;
        }
    }

    public int CalculateMaxXp(int level)
    {
        return Mathf.FloorToInt(((8 * level) + Diff(level)) * MXP(level) * RF(level));
    }

    private int Diff(int level)
    {
        if (level <= 28) return 0;
        else if (level == 29) return 1;
        else if (level == 30) return 3;
        else if (level == 31) return 6;
        else return 5 * (level - 30);
    }
    
    private int MXP(int level)
    {
        return 45 + (5 * level);
    }

    private float RF(int level)
    {
        if (level <= 10) return 1f;
        else if (level >= 11 && level <= 27) return (1f - (level - 10f) / 100f);
        else if (level >= 28 && level <= 59) return 0.82f;
        else return 1f;
    }
}
