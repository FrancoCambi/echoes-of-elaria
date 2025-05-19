using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    private static InventoryManager instance;

    public static InventoryManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<InventoryManager>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject slotPrefab;

    private List<Slot> slots = new();

    public List<Slot> Slots
    {
        get
        {
            return slots;
        }
    }

    private void Start()
    {
        for (int i = 0; i < PlayerManager.Instance.InventorySpace; i++)
        {
            GameObject go = Instantiate(slotPrefab, transform);
            slots.Add(go.GetComponent<Slot>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddItems(1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            AddItems(2, 1);
        }
    }

    public void AddItems(int itemID, int amount)
    {
        if (ItemInInventory(itemID))
        {
            StackItems(itemID, amount);
        }
        else
        {
            AddItemsInEmpty(itemID, amount);
        }
    }

    private void StackItems(int itemID, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Slot slot = GetFirstNonFullSlot(itemID);

            if (slot)
            {
                slot.Amount++;
            }
            else
            {
                AddItemsInEmpty(itemID, amount - i);
                return;
            }
        }
    }

    private void AddItemsInEmpty(int itemID, int amount)
    {
        if (amount > 0)
        {
            if (GetFirstEmptySlot() is Slot slot and not null)
            {
                slot.AddItemInEmpty(itemID);
                StackItems(itemID, amount - 1);
            }
        }
    }

    private Slot GetFirstEmptySlot()
    {
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty)
            {
                return slot;
            }
        }
        return null;
    }

    private Slot GetFirstNonFullSlot(int itemID)
    {
        foreach (Slot slot in slots)
        {
            if (!slot.IsEmpty && slot.Item.Id == itemID && !slot.IsFull)
            {
                return slot;
            }
        }
        return null;
    }
    public bool ItemInInventory(int itemID)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].IsEmpty && slots[i].Item.Id == itemID)
            {
                return true;
            }
        }
        return false;
    }
    
    public void RemoveItem(int itemID, int amount)
    {

    } 
}
