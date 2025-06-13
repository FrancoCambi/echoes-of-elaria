using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager instance;

    public static TooltipManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<TooltipManager>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject background;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Canvas canvas;
    private RectTransform tooltipRect;

    private BaseSlot hoveredSlot;

    public BaseSlot HoveredSlot
    {
        get
        {
            return hoveredSlot;
        }
    }

    private void Start()
    {
        tooltipRect = GetComponent<RectTransform>();
    }

    public void ShowTooltip(BaseSlot slot)
    {
        hoveredSlot = slot;
        background.SetActive(true);
        text.text = slot.Content.GetDescription();

        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);

        Vector2 anchoredPos = GetAnchoredPositionInCanvas(slot.GetComponent<RectTransform>());
        AdjustPivotAndPosition(anchoredPos, slot);
    }

    public void HideTooltip()
    {
        hoveredSlot = null;
        background.SetActive(false);

    }

    private Vector2 GetAnchoredPositionInCanvas(RectTransform target)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            RectTransformUtility.WorldToScreenPoint(null, target.position), // null porque es Overlay
            null,
            out localPoint
        );
        return localPoint;
    }
    private void AdjustPivotAndPosition(Vector2 anchoredPos, BaseSlot slot)
    {
        float tooltipWidth = tooltipRect.rect.width;
        float tooltipHeight = tooltipRect.rect.height;

        RectTransform canvasRect = canvas.transform as RectTransform;
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        Vector2 newPivot = new Vector2(0f, 0f);

        if (anchoredPos.x + tooltipWidth >= canvasWidth / 2f)
        {
            newPivot.x = 1f; 
            anchoredPos.x -= (slot.transform as RectTransform).rect.width;
        }

        tooltipRect.pivot = newPivot;
        tooltipRect.anchoredPosition = anchoredPos;
    }

}
