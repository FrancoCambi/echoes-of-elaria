using UnityEngine;
using UnityEngine.EventSystems;

public class LootSlot : BaseSlot
{
    public Loot Loot { get; set; }

    public override int GetSlotIndex()
    {
        // This is not implemented
        return 0;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        return;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        return;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        return;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Loot.Obtain();
        }
    }

    private void OnDestroy()
    {
        if (TooltipManager.Instance.HoveredSlot == this)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}
