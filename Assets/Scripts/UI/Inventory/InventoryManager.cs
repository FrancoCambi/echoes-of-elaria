using System.Collections.Generic;
using System.Data;
using UnityEngine;

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
            AddItems(1, 3);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            AddItems(3, 1);
            AddItems(4, 1);
        }
    }


    // TODO: FIX RETURN SO IT RETURNS TRUE IF IT CAN ADD ITEMS
    // AND FALSE OTHERWISE.
    public bool AddItems(int itemID, int amount)
    {
        if (ItemInInventory(itemID) && GetFirstNonFullSlot(itemID) != null)
        {
            StackItems(itemID, amount);
        }
        else
        {
            AddItemsInEmpty(itemID, amount);
        }

        SaveItemsToDatabase();
        return true;
    }

    private void AddItemsInEmpty(int itemID, int amount)
    {
        if (amount > 0)
        {
            if (GetFirstEmptySlot() is InventorySlot slot and not null)
            {
                slot.AddItemsInEmpty(itemID, amount);
            }
        }
    }

    private void StackItems(int itemID, int amount)
    {
        if (amount > 0)
        {
            if (GetFirstNonFullSlot(itemID) is InventorySlot slot and not null)
            {
                slot.AddItemsInNonEmpty(itemID, amount);
            }
        }

    }

    public void AddItemsInSlot(int itemID, int amount, int slotIndex)
    {
        InventorySlot slot = slots[slotIndex];

        if (!slot.IsEmpty) return;

        slot.AddItemsInEmpty(itemID, amount);
    }

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

}
