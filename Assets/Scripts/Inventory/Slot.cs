using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private Image icon;
    private TextMeshProUGUI amountText;

    private Item item;

    private int amount;
    public Item Item
    {
        get
        {
            return item;
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

    private void Start()
    {
        amountText = GetComponentInChildren<TextMeshProUGUI>();
        amount = 0;
    }

    public void UpdateText()
    {
        amountText.text = amount.ToString();
    }

    public void AddItemInEmpty(int id)
    {
        item = ItemsManager.Instance.GetItemByID(id);
        icon.enabled = true;
        icon.sprite = item.Icon;
        
        Amount++;
    }

    public void StackItem()
    {
        Amount++;
    }

    private void RemoveItem()
    {
        icon.enabled = false;
        amountText.text = "";
        amount = 0;
        item = null;
    }

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

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ItemDragManager.Instance.IsDragging && ItemDragManager.Instance.DraggedSlot != this)
        {
            item = ItemDragManager.Instance.DraggedItem;
            Amount = ItemDragManager.Instance.DraggedStack;
            icon.enabled = true;
            icon.sprite = item.Icon;
            ItemDragManager.Instance.DraggedSlot.RemoveItem();
        }
    }

    #endregion



}
