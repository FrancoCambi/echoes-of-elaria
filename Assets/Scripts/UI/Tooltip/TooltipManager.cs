using TMPro;
using UnityEngine;

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
    private RectTransform tooltipRect;

    private void Start()
    {
        tooltipRect = GetComponent<RectTransform>();
    }

    public void ShowTooltip(BaseSlot slot)
    {
        background.SetActive(true);
        transform.position = slot.transform.position;
        text.text = slot.Content.GetDescription();

        AdjustPivotes(slot);
    }

    public void HideTooltip()
    {
        background.SetActive(false);
    }

    private void AdjustPivotes(BaseSlot slot)
    {
        Vector2 tooltipSize = new Vector2(tooltipRect.rect.width, tooltipRect.rect.height);
        Vector2 tooltipPos = tooltipRect.position;
        Debug.Log(tooltipPos);

        float cameraWidth = 422;
        float cameraHeight = 237f;

        tooltipRect.pivot = new Vector2(0,0);
        slot.GetComponent<RectTransform>().pivot = new Vector2(1, 1);


        if (tooltipPos.x + tooltipSize.x >= cameraWidth)
        {
            Debug.Log("A");
            tooltipRect.pivot = new Vector2(1, 0);
            slot.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        }
        else if (tooltipPos.x <= cameraWidth)
        {
            Debug.Log("B");
            tooltipRect.pivot = new Vector2(0, 0);
            slot.GetComponent<RectTransform>().pivot = new Vector2(1, 1);

        }


    }
}
