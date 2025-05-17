using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;

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

    private Dictionary<int, int> items = new();

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
    }

    public void AddItem(int itemID, int amount)
    {
        if (ItemInInventory(itemID))
        {
            slots[items[itemID]].Amount += amount;
        }
        else
        {
            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];

                if (slot.IsEmpty)
                {
                    slot.AddItem(itemID, amount);
                    items[itemID] = i;
                    return;
                }
            }
        }

        OnInventoryChanged?.Invoke();
    }

    public bool ItemInInventory(int itemID)
    {
        return items.ContainsKey(itemID);
    }
    
    public void RemoveItem(int itemID, int amount)
    {

    } 
}
