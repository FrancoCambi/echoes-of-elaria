using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EquipmentManager : Panel
{
    private static EquipmentManager instance;

    public static EquipmentManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private List<EquipmentSlot> slots = new();

    public List<EquipmentSlot> Slots
    {
        get
        {
            return slots;
        }
    }

    public static event Action OnEquipmentChanged;

    public override void Awake()
    {
        base.Awake();
        instance = instance != null ? instance : this;
    }

    private void Start()
    {
        LoadEquipment();
    }

    public void EquipGear(Gear gear, bool updateStats = true)
    {
        EquipmentSlot matchingSlot = GetSlotByType(gear.EquipType);

        matchingSlot.Clear();
        matchingSlot.EquipGear(gear, updateStats);
        OnEquipmentChanged?.Invoke();
        SaveEquipment();
    }

    public void UnEquipGear()
    {
        OnEquipmentChanged?.Invoke();
        SaveEquipment();
    }

    public EquipmentSlot GetSlotByType(EquipmentType type)
    {
        foreach (EquipmentSlot slot in slots)
        {
            if (slot.EquipType == type)
            {
                return slot;
            }
        }

        return null;
    }

    #region database

    private void SaveEquipment()
    {
        List<(int, string, int)> valuesList = new();

        foreach (EquipmentSlot slot in slots)
        {
            if (!slot.IsEmpty)
            {
                Gear contentGear = slot.Content as Gear;

                valuesList.Add((GameManager.SelCharID, $"'{contentGear.EquipType.ToString().ToLower()}'", contentGear.Id));
            }
        }

        string values = string.Join(",", valuesList);

        string query = values != "" ?
            $"BEGIN TRANSACTION;" +
            $"DELETE FROM character_equipment WHERE character_id = {GameManager.SelCharID};" +
            $"INSERT INTO character_equipment (character_id, type, item_id)" +
            $"VALUES" +
            $"{values};" +
            $"COMMIT;"
            :
            $"DELETE FROM character_equipment WHERE character_id = {GameManager.SelCharID}";
        DBManager.Instance.ExecuteQuery(query);
    }

    private void LoadEquipment()
    {
        string query = $"SELECT * FROM character_equipment WHERE character_id = {GameManager.SelCharID}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        foreach (DataRow row in table.Rows)
        {
            int itemID = int.Parse(row["item_id"].ToString());

            EquipGear(ItemsManager.Instance.GetItemByID(itemID) as Gear, false);
        }
    }

    #endregion
}
