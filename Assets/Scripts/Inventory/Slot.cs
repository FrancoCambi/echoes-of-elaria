using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    private TextMeshProUGUI amountText;

    private Item item;

    private int amount;

    #region properties

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

    private void Start()
    {
        amountText = GetComponentInChildren<TextMeshProUGUI>();
        amount = 0;
    }

    public void UpdateText()
    {
        amountText.text = amount.ToString();
    }

    #region methods

    public void AddItemInEmpty(int id)
    {
        // Assuming this is an empty slot, add an item.
        item = ItemsManager.Instance.GetItemByID(id);
        icon.enabled = true;
        icon.sprite = item.Icon;

        Amount++;
    }

    private void AddItemsInEmpty(int id, int amount)
    {
        // Assuming this is an empty slot, add items.
        item = ItemsManager.Instance.GetItemByID(id);
        icon.enabled = true;
        icon.sprite = item.Icon;

        for (int i = 0; i < amount; i++)
        {
            Amount++;
        }
    }


    private void CombineStacks(Slot slot1, Slot slot2)
    {
        // If I cant combine them all, just do what I can.
        if (slot2.Amount + slot1.Amount > slot2.Item.MaxStack)
        {
            int tempAmount = slot2.Amount;
            slot2.Amount += (slot2.Item.MaxStack - tempAmount);
            slot1.Amount -= (slot2.Item.MaxStack - tempAmount);
        }
        // Else, just put all 1 in 2.
        else
        {
            slot2.Amount += slot1.amount;
            slot1.Amount = 0;
        }
    }

    private void SwapSlots(Slot slot1, Slot slot2)
    {
        // Get the needed variables before clear
        int fromId = slot1.Item.Id;
        int fromAmount = slot1.Amount;
        int toId = slot2.Item.Id;
        int toAmount = slot2.Amount;

        // Place s2 in s1
        slot1.Clear();
        slot1.AddItemsInEmpty(toId, toAmount);

        // Place s1 in s2
        slot2.Clear();
        slot2.AddItemsInEmpty(fromId, fromAmount);
    }

    private void Clear()
    {
        icon.enabled = false;
        amountText.text = "";
        amount = 0;
        item = null;
    }

    #endregion

    #region interfaces


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmpty)
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

        // If dropping item on the floor, delete it.
        // NOTE: A confirmation before destroy will be needed.
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Clear();
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        // If I'm dragging an item and not putting it in the same slot..
        if (ItemDragManager.Instance.IsDragging && ItemDragManager.Instance.DraggedSlot != this)
        {
            // If drop slot is empty, just put it there.
            if (IsEmpty)
            {
                AddItemsInEmpty(ItemDragManager.Instance.DraggedItem.Id, ItemDragManager.Instance.DraggedAmount);
                ItemDragManager.Instance.DraggedSlot.Clear();
            }
            // If drop slot contains the same item, combine the stacks.
            else if (Item.Id == ItemDragManager.Instance.DraggedSlot.Item.Id)
            {
                CombineStacks(ItemDragManager.Instance.DraggedSlot, this);
            }
            // If drop slot contains a different item, swap places.
            else if (Item.Id != ItemDragManager.Instance.DraggedSlot.Item.Id)
            {
                SwapSlots(ItemDragManager.Instance.DraggedSlot, this);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEmpty && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item.Type == ItemType.Consumable)
            {
                foreach (ItemEffect effect in ItemsManager.Instance.GetEffectsByID(Item.Id))
                {
                    effect.Apply();
                }
            }
        }
    }

    #endregion



}
