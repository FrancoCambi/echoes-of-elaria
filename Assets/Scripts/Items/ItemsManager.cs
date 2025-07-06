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

       
        int _id = int.Parse(table.Rows[0]["id"].ToString());
        string _name = table.Rows[0]["name"].ToString();
        int _requiredLevel = int.Parse(table.Rows[0]["required_level"].ToString());
        int _itemLevel = int.Parse(table.Rows[0]["item_level"].ToString());
        int _rarity = int.Parse(table.Rows[0]["rarity"].ToString());
        int _maxStack = int.Parse(table.Rows[0]["max_stack"].ToString());
        int _typeId = int.Parse(table.Rows[0]["type_id"].ToString());
        int armor = 0;
        int stamina = 0;
        int intellect = 0;
        int power = 0;

        if (_typeId == 2)
        {
            armor = int.Parse(table.Rows[0]["armor"].ToString());
            stamina = int.Parse(table.Rows[0]["stamina"].ToString());
            intellect = int.Parse(table.Rows[0]["intellect"].ToString());
            power = int.Parse(table.Rows[0]["arcane_power"].ToString());
            Gear gear = new(_id, _name, _requiredLevel, _itemLevel, _rarity, _maxStack, _typeId, armor, stamina, intellect, power);
            return gear;

        }
        else
        {
            Item item = new(_id, _name, _requiredLevel, _itemLevel, _rarity, _maxStack, _typeId);
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
