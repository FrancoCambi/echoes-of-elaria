using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Image icon;
    protected TextMeshProUGUI amountText;

    protected SlotContent content;

    protected int amount;

    #region properties

    public SlotContent Content
    {
        get
        {
            return content;
        }
    }

    public int Amount
    {
        get
        {
            return amount;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return content == null;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty) return false;

            if (content is Item contentItem)
            {
                return amount == contentItem.MaxStack;
            }
            else
            {
                return true;
            }
        }
    }

    #endregion

    public virtual void Awake()
    {
        amountText = GetComponentInChildren<TextMeshProUGUI>();
        amount = 0;

    }

    #region methods

    public virtual void SetContent(SlotContent newContent)
    {
        if (newContent == null)
        {
            Clear();
            return;
        }

        content = newContent;
        icon.sprite = newContent.Icon;
        icon.enabled = true;
        amountText.enabled = true;
    }

    public virtual void AddAmount(int quantity)
    {
        amount = Mathf.Clamp(amount + quantity, 0, content.MaxStack);
        UpdateText();

        if (amount <= 0)
        {
            Clear();
        }
    }

    public virtual void UpdateText()
    {
        amountText.text = amount > 1 ? amount.ToString() : "";
    }

    public virtual void Clear()
    {
        content = null;
        icon.enabled = false;
        amount = 0;
        UpdateText();
        amountText.enabled = false;
    }

    public abstract int GetSlotIndex();

    #endregion

    #region interfaces

    public abstract void OnBeginDrag(PointerEventData eventData);

    public abstract void OnDrag(PointerEventData eventData);

    public abstract void OnEndDrag(PointerEventData eventData);

    public abstract void OnDrop(PointerEventData eventData);

    public abstract void OnPointerClick(PointerEventData eventData);
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (content != null)
        {
            TooltipManager.Instance.ShowTooltip(this);
        }
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (content != null)
        { 
            TooltipManager.Instance.HideTooltip();
        }
    }


    #endregion
}
