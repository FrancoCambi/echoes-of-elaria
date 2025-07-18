using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class InventoryManager : Panel
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
    private List<InventorySlot> slots = new();

    public List<InventorySlot> Slots
    {
        get
        {
            return slots;
        }
    }

    public bool SlotsLeft
    {
        get
        {
            return GetFirstEmptySlot() != null;
        }
    }

    public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < PlayerManager.Instance.InventorySpace; i++)
        {
            GameObject go = Instantiate(slotPrefab, transform);
            InventorySlot slot = go.GetComponent<InventorySlot>();
            slots.Add(slot);
        }
        LoadItemsFromDatabase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddItems(1, 2);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            AddItems(3, 1);
            AddItems(4, 1);
            AddItems(5, 1);
            AddItems(6, 1);
        }
    }

    #region itemManagement

    public bool AddItems(int itemID, int amount)
    {
        bool result;
        if (ItemInInventory(itemID) && GetFirstNonFullSlot(itemID) != null)
        {
            result = StackItems(itemID, amount);
        }
        else
        {
            result = AddItemsInEmpty(itemID, amount);
        }

        SaveItemsToDatabase();
        return result;
    }

    public void SlotCleared(int index)
    {
        SaveItemsToDatabase();
    }

    private bool AddItemsInEmpty(int itemID, int amount)
    {
        if (amount > 0)
        {
            if (GetFirstEmptySlot() is InventorySlot slot and not null)
            {
                slot.AddItemsInEmpty(itemID, amount);
                return true;
            }
        }

        string fullInvMsg = LocalizationSettings.StringDatabase.GetLocalizedString("Ui", "AlertFullInventory");
        AlertManager.Instance.ThrowAlert(fullInvMsg);
        return false;
    }


    private bool StackItems(int itemID, int amount)
    {
        InventorySlot slot = GetFirstNonFullSlot(itemID);
        
        if (slot.Amount + amount <= slot.Content.MaxStack || SlotsLeft)
        {
             slot.AddItemsInNonEmpty(itemID, amount);
             return true;
        }
        
        // Else
        string fullInvMsg = LocalizationSettings.StringDatabase.GetLocalizedString("Ui", "AlertFullInventory");
        AlertManager.Instance.ThrowAlert(fullInvMsg);
        return false;
    }

    public void AddItemsInSlot(int itemID, int amount, int slotIndex)
    {
        InventorySlot slot = slots[slotIndex];

        if (!slot.IsEmpty) return;

        slot.AddItemsInEmpty(itemID, amount);

        SaveItemsToDatabase();
    }

    public void SortInventory()
    {
        List<Tuple<int, int>> itemsData = new();

        foreach (InventorySlot slot in Slots)
        {
            if (slot.IsEmpty) continue;

            itemsData.Add(new Tuple<int, int>((slot.Content as Item).Id, slot.Amount));
        }

        ClearInventory();

        itemsData = itemsData.OrderBy(x => x.Item1).ToList();

        for (int i = 0; i < itemsData.Count; i++)
        {
            AddItemsInSlot(itemsData[i].Item1, itemsData[i].Item2, i);
        }
        
        SaveItemsToDatabase();
    }

    #endregion

    #region utility

    public InventorySlot GetFirstEmptySlot()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty)
            {
                return slot;
            }
        }
        return null;
    }

    private InventorySlot GetFirstNonFullSlot(int itemID)
    {
        foreach (InventorySlot slot in slots)
        {
            Item contentItem = slot.Content as Item;

            if (!slot.IsEmpty && contentItem.Id == itemID && !slot.IsFull)
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
            InventorySlot slot = slots[i];
            Item contentItem = slot.Content as Item;

            if (!slot.IsEmpty && contentItem.Id == itemID)
            {
                return true;
            }
        }
        return false;
    }

    private void ClearInventory()
    {
        foreach (InventorySlot slot in Slots)
        {
            slot.Clear();
        }

        SaveItemsToDatabase();
    }

    #endregion

    #region persistence
    public void SaveItemsToDatabase()
    {
        List<(int, int, int, int)> valuesList = new();

        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty)
            {
                Item contentItem = slot.Content as Item;

                valuesList.Add((GameManager.SelCharID, contentItem.Id, slot.Amount, slot.GetSlotIndex()));
            }
        }

        string values = string.Join(",", valuesList);

        string query = values != "" ? 
            $"BEGIN TRANSACTION;" +
            $"DELETE FROM characters_inventory WHERE character_id = {GameManager.SelCharID};" +
            $"INSERT INTO characters_inventory(character_id, item_id, amount, slot_index)" +
            $"VALUES" +
            $"{values};" +
            $"COMMIT;" 
            : 
            $"DELETE FROM characters_inventory WHERE character_id = {GameManager.SelCharID}";

        DBManager.Instance.ExecuteQuery(query);
    }
    
    public void LoadItemsFromDatabase()
    {
        string query = $"SELECT * FROM characters_inventory WHERE character_id = {GameManager.SelCharID}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            int itemID = int.Parse(row["item_id"].ToString());
            int amount = int.Parse(row["amount"].ToString());
            int slotIndex = int.Parse(row["slot_index"].ToString());

            AddItemsInSlot(itemID, amount, slotIndex);
        }
    }

    #endregion

}
