using System.Collections.Generic;
using NUnit.Framework;
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

    public void EquipGear(Gear gear)
    {
        EquipmentSlot matchingSlot = GetSlotByType(gear.EquipType);

        matchingSlot.Clear();
        matchingSlot.EquipGear(gear);
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

    public override void Awake()
    {
        base.Awake();
        instance = instance != null ? instance : this;
    }
}
