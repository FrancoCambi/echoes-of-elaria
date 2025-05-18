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

    public static event Action OnInventoryChanged;

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
            AddItem(1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem(1, 3);
        }
    }

    public void AddItem(int itemID, int amount)
    {
        if (ItemInInventory(itemID))
        {
            StackItem(itemID, amount);
        }
        else
        {
            AddItemInEmpty(itemID, amount);
        }

        OnInventoryChanged?.Invoke();
    }

    public void StackItem(int itemID, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Slot slot = GetFirstNonFullSlot(itemID);

            if (slot)
            {
                slot.StackItem();
            }
            else
            {
                AddItemInEmpty(itemID, amount - i);
                return;
            }
        }
    }

    public void AddItemInEmpty(int itemID, int amount)
    {
        if (amount > 0)
        {
            Slot slot = GetFirstEmptySlot();
            slot.AddItemInEmpty(itemID);

            StackItem(itemID, amount - 1);
        }
    }

    public Slot GetFirstEmptySlot()
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

    public Slot GetFirstNonFullSlot(int itemID)
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
