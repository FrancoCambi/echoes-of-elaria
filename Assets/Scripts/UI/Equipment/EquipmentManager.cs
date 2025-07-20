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

    #region properties

    public List<EquipmentSlot> Slots
    {
        get
        {
            return slots;
        }
    }

    #endregion

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

    #region equipmentManagement

    public void EquipGear(Gear gear, bool updateStats = true)
    {
        EquipmentSlot matchingSlot = GetSlotByType(gear.EquipType);

        if (matchingSlot.IsEmpty)
        {
            AddGearToEmpty(gear, matchingSlot, updateStats);
        }
        else
        {
            ReplaceGear(gear, matchingSlot, updateStats);
        }

        SaveEquipment();
    }

    private void AddGearToEmpty(Gear gear, EquipmentSlot slot, bool updateStats = true)
    {
        slot.EquipGear(gear, updateStats);
        OnEquipmentChanged?.Invoke();
    }

    private void ReplaceGear(Gear gear, EquipmentSlot slot, bool updateStats = true)
    {
        slot.ClearWithoutSaving();
        AddGearToEmpty(gear, slot, updateStats);
    }

    public void UnEquipGear(bool saveEquipment = true)
    {
        OnEquipmentChanged?.Invoke();
        if (saveEquipment) SaveEquipment();
    }

    #endregion

    #region utils

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

    #endregion

    #region persistance

    private void SaveEquipment()
    {

#if UNITY_EDITOR
        print("Equipment has been saved.");
#endif
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
