using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSlot : BaseSlot
{
    [SerializeField]
    private Sprite emptyBackground;
    [SerializeField]
    private Sprite fullBackground;
    private Image background;

    private InventorySlot itemSlot;

    public override void Awake()
    {
        base.Awake();
        background = GetComponent<Image>();
    }

    #region methods

    public override void SetContent(SlotContent newContent)
    {
        base.SetContent(newContent);

        background.sprite = fullBackground;
        UpdateText();
    }

    public override void UpdateText()
    {
        if (amountText == null || itemSlot == null) return;

        amountText.text = itemSlot.Amount > 1 ? itemSlot.Amount.ToString() : "";
    }

    public override void Clear()
    {
        base.Clear();

        background.sprite = emptyBackground;

    }

    public override int GetSlotIndex()
    {
        throw new System.NotImplementedException();
    }

    public void SetItemSlot(InventorySlot slot)
    {
        if (slot == null) return;
        if (itemSlot != null)
        {
            itemSlot.OnInventorySlotAmountChanged -= ItemUsedFromInventory;
        }

        itemSlot = slot;
        itemSlot.OnInventorySlotAmountChanged += ItemUsedFromInventory;
    }

    private void ItemUsedFromInventory()
    {
        UpdateText();
        if (itemSlot.Amount <= 0)
        {
            Clear();
        }
    }

    public void Use()
    {
        if (itemSlot != null)
        {
            (itemSlot.Content as Item).Use();
        }
    }

    #endregion

    #region interfaces

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmpty && eventData.button == PointerEventData.InputButton.Left)
        {
            DragManager.Instance.StartDrag(content, icon.sprite, this);
            icon.color = Color.gray;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        DragManager.Instance.MoveImage(Input.mousePosition);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        icon.color = Color.white;
        DragManager.Instance.EndDrag();

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Clear();
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        BaseSlot fromSlot = DragManager.Instance.FromSlot;

        if (fromSlot == this || fromSlot == null || fromSlot.Content is not Item) return;

        if (fromSlot is InventorySlot)
        {
            SetItemSlot(fromSlot as InventorySlot);
            SetContent(fromSlot.Content);
        }
        else if (fromSlot is ActionSlot && IsEmpty)
        {
            Clear();
            SetItemSlot((fromSlot as ActionSlot).itemSlot);
            SetContent(fromSlot.Content);
            fromSlot.Clear();
        }
        else if (fromSlot is ActionSlot && !IsEmpty)
        {
            SlotContent tmpContent = Content;
            InventorySlot tmpSlot = itemSlot;
            Clear();
            SetItemSlot((fromSlot as ActionSlot).itemSlot);
            SetContent(fromSlot.Content);

            fromSlot.Clear();
            (fromSlot as ActionSlot).SetItemSlot(tmpSlot);
            fromSlot.SetContent(tmpContent);


        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEmpty && eventData.button == PointerEventData.InputButton.Right)
        {
            // Consumables are the ones with effects.
            if (content is Item contentItem && contentItem.Type == ItemType.Consumable)
            {
                foreach (ItemEffect effect in ItemsManager.Instance.GetEffectsByID(contentItem.Id))
                {
                    effect.Apply();
                }

                itemSlot.AddAmount(-1);
            }
        }
    }


    #endregion
}
