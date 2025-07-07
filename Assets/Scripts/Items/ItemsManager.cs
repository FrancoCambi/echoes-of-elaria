using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    private static ItemsManager instance;

    public static ItemsManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<ItemsManager>();
            return instance;
        }
    }
    public Item GetItemByID(int id)
    {
        string query = $"SELECT * FROM items WHERE id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);
        if (table.Rows.Count == 0) return null;

        string name = table.Rows[0]["name"].ToString();
        int requiredLevel = int.Parse(table.Rows[0]["required_level"].ToString());
        int itemLevel = int.Parse(table.Rows[0]["item_level"].ToString());
        ItemRarity rarity = GetItemRarityById(int.Parse(table.Rows[0]["rarity"].ToString()));
        int maxStack = int.Parse(table.Rows[0]["max_stack"].ToString());
        ItemType type = GetItemTypeFromString(table.Rows[0]["item_type"].ToString());

        if (type == ItemType.Equipment)
        {
            EquipmentType equipType = GetEquipTypeFromString(table.Rows[0]["equipment_type"].ToString());
            int armor = int.Parse(table.Rows[0]["armor"].ToString());
            int stamina = int.Parse(table.Rows[0]["stamina"].ToString());
            int intellect = int.Parse(table.Rows[0]["intellect"].ToString());
            int power = int.Parse(table.Rows[0]["arcane_power"].ToString());
            Gear gear = new(id, name, requiredLevel, itemLevel, rarity, maxStack, type, equipType, armor, stamina, intellect, power);
            return gear;

        }
        else
        {
            Item item = new(id, name, requiredLevel, itemLevel, rarity, maxStack, type);
            return item;
        }

    }

    public List<ItemEffect> GetEffectsByID(int id)
    {
        string query = $"SELECT * FROM item_effects WHERE item_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);
        List<ItemEffect> effects = new();

        foreach (DataRow row in table.Rows)
        {
            int _id = int.Parse(row["id"].ToString());
            int _itemId = int.Parse(row["item_id"].ToString());
            ItemEffectType _effectType = GetEffectTypeFromString(row["effect_type"].ToString());
            int _value = int.Parse(row["value"].ToString());
            int _duration = int.Parse(row["duration"].ToString());
            TargetType _targetType = GetTargetTypeFromString(row["target_type"].ToString());
            string _extraParam = row["extra_param"].ToString();
            ItemEffect effect = new(_id, _itemId, _effectType, _value, _duration, _targetType, _extraParam);
            effects.Add(effect);

        }

        return effects;
    }

    private EquipmentType GetEquipTypeFromString(string type)
    {
        return type switch
        {
            "helm" => EquipmentType.Helm,
            "chest" => EquipmentType.Chest,
            "legs" => EquipmentType.Legs,
            "hands" => EquipmentType.Hands,
            "feet" => EquipmentType.Feet,
            "ring" => EquipmentType.Ring,
            "trinket" => EquipmentType.Trinket,
            _ => EquipmentType.None,
        };
    }

    private ItemType GetItemTypeFromString(string type)
    {
        return type switch
        {
            "consumable" => ItemType.Consumable,
            "equipment" => ItemType.Equipment,
            "material" => ItemType.Material,
            "quest" => ItemType.Quest,
            _ => ItemType.None,
        };
    }

    private ItemRarity GetItemRarityById(int id)
    {
        return id switch
        {
            0 => ItemRarity.Common,
            1 => ItemRarity.Uncommon,
            2 => ItemRarity.Rare,
            3 => ItemRarity.Epic,
            4 => ItemRarity.Legendary,
            _ => ItemRarity.None,

        };
    }

    private ItemEffectType GetEffectTypeFromString(string type)
    {
        return type switch
        {
            "heal_hp" => ItemEffectType.HealHp,
            "gain_mana" => ItemEffectType.GainMana,
            "gain_rage" => ItemEffectType.GainRage,
            "add_exp" => ItemEffectType.AddExp,
            "add_gold" => ItemEffectType.AddGold,
            "add_item" => ItemEffectType.AddItem,
            "teleport" => ItemEffectType.Teleport,
            _ => ItemEffectType.Custom,
        };
    }

    private TargetType GetTargetTypeFromString (string type)
    {
        return type switch
        {
            "self" => TargetType.Self,
            _ => TargetType.Enemy,
        };
    }
}
