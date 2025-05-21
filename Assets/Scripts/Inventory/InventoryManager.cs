using System;
using System.Collections.Generic;
using System.Data;
using Unity.Burst.Intrinsics;
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
    private CanvasGroup group;

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
            Slot slot = go.GetComponent<Slot>();
            slots.Add(slot);
        }

        group = GetComponent<CanvasGroup>();

        LoadItemsFromDatabase();
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

        SaveItemsToDatabase();
    }

    public void AddItemsInSlot(int itemID, int amount, int slotIndex)
    {
        Slot slot = slots[slotIndex];

        if (!slot.IsEmpty) return;

        slot.AddItemsInEmpty(itemID, amount);
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
    
    public void LoadItemsFromDatabase()
    {
        string query = $"SELECT * FROM characters_inventory WHERE character_id = {GameManager.Instance.SelCharID}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            int itemID = int.Parse(row["item_id"].ToString());
            int amount = int.Parse(row["amount"].ToString());
            int slotIndex = int.Parse(row["slot_index"].ToString());

            AddItemsInSlot(itemID, amount, slotIndex);
        }
    }
    public void SaveItemsToDatabase()
    {
        List<(int, int, int, int)> valuesList = new();

        foreach (Slot slot in slots)
        {
            if (!slot.IsEmpty)
            {
                valuesList.Add((GameManager.Instance.SelCharID, slot.Item.Id, slot.Amount, slot.SlotIndex));
            }
        }

        string values = string.Join(",", valuesList);

        string query = values != "" ? 
            $"BEGIN TRANSACTION;" +
            $"DELETE FROM characters_inventory WHERE character_id = {GameManager.Instance.SelCharID};" +
            $"INSERT INTO characters_inventory(character_id, item_id, amount, slot_index)" +
            $"VALUES" +
            $"{values};" +
            $"COMMIT;" 
            : 
            $"DELETE FROM characters_inventory WHERE character_id = {GameManager.Instance.SelCharID}";

        DBManager.Instance.ExecuteQuery(query);
    }


    public void OpenCloseUI()
    {
        group.alpha = group.alpha != 0 ? 0 : 1;
        group.blocksRaycasts = !group.blocksRaycasts;
    }
}
