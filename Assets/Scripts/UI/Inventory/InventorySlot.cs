using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : BaseSlot
{

    public event Action OnInventorySlotAmountChanged;

    public override void AddAmount(int quantity)
    {
        base.AddAmount(quantity);

        OnInventorySlotAmountChanged?.Invoke();
    }

    public override void Clear()
    {
        base.Clear();

        OnInventorySlotAmountChanged?.Invoke();
    }

    public void AddItemsInEmpty(int itemID, int quantity)
    {
        if (IsEmpty && quantity > 0)
        {
            SlotContent newContent = ItemsManager.Instance.GetItemByID(itemID);
            SetContent(newContent);

            if (quantity > content.MaxStack)
            {
                AddAmount(content.MaxStack);
                InventoryManager.Instance.GetFirstEmptySlot().AddItemsInEmpty(itemID, quantity - content.MaxStack);
            }
            else
            {
                AddAmount(quantity);
            }
        }
    }

    public void AddItemsInNonEmpty(int itemID, int quantity)
    {
        if (!IsEmpty && content is Item itemContent && itemContent.Id == itemID && quantity > 0)
        {
            if (Amount + quantity > content.MaxStack)
            {
                int rest = content.MaxStack - Amount;
                AddAmount(rest);
                InventoryManager.Instance.GetFirstEmptySlot().AddItemsInEmpty(itemID, quantity - rest);
            }
            else
            {
                AddAmount(quantity);
            }
        }
    }

    public override int GetSlotIndex()
    {
        return InventoryManager.Instance.Slots.IndexOf(this);
    }

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

        // If drag same thing, none or no item, just return
        if (fromSlot == this || fromSlot == null || fromSlot.Content is not Item) return;


        // From Inventory Slot
        if (fromSlot is InventorySlot)
        {
            if (!IsEmpty && content.CanStackWith(fromSlot.Content))
            {
                int transferable = content.MaxStack - amount;
                int transferAmount = Mathf.Min(transferable, fromSlot.Amount);
                AddAmount(transferAmount);
                fromSlot.AddAmount(-transferAmount);
            }
            else if ((!IsEmpty && !content.CanStackWith(fromSlot.Content)) || IsFull)
            {
                int tmpID = (content as Item).Id;
                int tmpAmount = amount;
                Clear();
                AddItemsInEmpty((fromSlot.Content as Item).Id, fromSlot.Amount);
                fromSlot.Clear();
                (fromSlot as InventorySlot).AddItemsInEmpty(tmpID, tmpAmount);
            }
            else // IsEmpty
            {
                AddItemsInEmpty((fromSlot.Content as Item).Id, fromSlot.Amount);
                fromSlot.Clear();
            }
        }
        // From Action Slot
        else if (fromSlot is ActionSlot actionSlot)
        {
            actionSlot.Clear();
        }
        // From Equipment Slot
        else if (fromSlot is EquipmentSlot equipmentSlot)
        {
            if (IsEmpty)
            {
                AddItemsInEmpty((equipmentSlot.Content as Gear).Id, fromSlot.Amount);
                equipmentSlot.Clear();
            }
            // Replace i
            else if (content is Gear gearContent && equipmentSlot.EquippedGear.IsSameType(gearContent))
            {
                int id = (fromSlot.Content as Item).Id;
                EquipmentManager.Instance.EquipGear(content as Gear);
                AddAmount(-1);
                AddItemsInEmpty(id, 1);
                
            }
        }

        InventoryManager.Instance.SaveItemsToDatabase();
        DragManager.Instance.Drop();
        base.OnDrop(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEmpty && eventData.button == PointerEventData.InputButton.Right)
        {
            // Consumables are the ones with effects.
            if (content is Item contentItem && contentItem.Type == ItemType.Consumable)
            {
                contentItem.Use();

                // Item will consume even if it does not have effects but..
                // why would it be a consumable then?
                AddAmount(-1);
            }
            else if (content is Gear contentGear)
            {
                EquipmentSlot matchingSlot = EquipmentManager.Instance.GetSlotByType(contentGear.EquipType);

                if (!matchingSlot.IsEmpty)
                {
                    int id = (matchingSlot.Content as Gear).Id;
                    EquipmentManager.Instance.EquipGear(contentGear);
                    AddAmount(-1);
                    AddItemsInEmpty(id, 1);
                    TooltipManager.Instance.ShowTooltip(this);
                }
                else
                {
                    EquipmentManager.Instance.EquipGear(contentGear);
                    AddAmount(-1);
                }

            }
        }
    }
}
