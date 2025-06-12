using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class SlotContent : ITooltipable
{
    private Sprite icon;

    private int maxStack;

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public int MaxStack
    {
        get
        {
            return maxStack;
        }
    }

    public SlotContent(Sprite icon, int maxStack)
    {
        this.icon = icon;
        this.maxStack = maxStack;
    }

    public abstract bool CanStackWith(SlotContent other);

    public abstract string GetDescription();
}
