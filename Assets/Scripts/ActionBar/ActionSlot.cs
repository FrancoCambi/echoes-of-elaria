using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image emptyBackground;
    [SerializeField]
    private Image filledBackground;
    private Image background;
    private TextMeshProUGUI amountText;

    private Item item;

    private int amount;



    void Awake()
    {
        background = GetComponent<Image>();
        amountText = GetComponentInChildren<TextMeshProUGUI>();
        amount = 0;
    }

    private void UpdateText()
    {
        amountText.text = amount.ToString();
    }

    #region propierties

    public Image Icon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }
    public Item Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
        }
    }
    public int Amount
    {
        get
        {
            return amount;
        }
        set
        {
            amount = value;
            if (amount > item.MaxStack)
            {
                amount = item.MaxStack;
            }
            UpdateText();

            if (amount <= 0)
            {
                Clear();
            }
        }
    }

    public bool IsEmpty
    {
        get
        {
            return item == null;
        }
    }

    public bool IsFull
    {
        get
        {
            if (!IsEmpty)
            {
                return amount == item.MaxStack;
            }
            return false;
        }
    }

    #endregion

    #region methods

    private void Clear()
    {
        icon.enabled = false;
        amountText.text = "";
        amount = 0;
        item = null;
        background = emptyBackground;
    }

    #endregion

    #region interfaces

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmpty && eventData.button == PointerEventData.InputButton.Left)
        {
            ItemDragManager.Instance.StartDrag(item, amount, icon.sprite, this);
            icon.color = Color.gray;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        ItemDragManager.Instance.MoveImage(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.color = Color.white;
        ItemDragManager.Instance.EndDrag();

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Clear();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
