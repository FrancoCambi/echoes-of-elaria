using UnityEngine;
using UnityEngine.Localization.Settings;

public enum EquipmentType
{
    Helm, Chest, Legs, Hands, Feet, Ring, Trinket, None
}

public class Gear : Item
{
    public EquipmentType EquipType { get; private set; }
    public int Armor { get; private set; }
    public int Stamina { get; private set; } 
    public int Intellect { get; private set; }
    public int ArcanePower {  get; private set; }

    public Gear(int id, string name, int reqLevel, int itemLevel, ItemRarity rarity, int maxStack, 
        ItemType type, EquipmentType equipType, int armor, int stamina, int intellect, int arcanePower) 
        : base(id, name, reqLevel, itemLevel, rarity, maxStack, type)
    {
        EquipType = equipType;   
        Armor = armor;
        Stamina = stamina;
        Intellect = intellect;
        ArcanePower = arcanePower;
    }

    public void Equip()
    {
        GiveStats();
    }

    public void UnEquip()
    {
        TakeStatsAway();

    }

    public bool IsSameType(Gear gear)
    {
        return EquipType == gear.EquipType;
    }

    private void GiveStats()
    {
        PlayerManager.Instance.GainArmor(Armor);
        PlayerManager.Instance.GainStamina(Stamina);
        PlayerManager.Instance.GainIntellect(Intellect);
        PlayerManager.Instance.GainArcanePower(ArcanePower);
    }

    private void TakeStatsAway()
    {
        PlayerManager.Instance.LoseArmor(Armor);
        PlayerManager.Instance.LoseStamina(Stamina);
        PlayerManager.Instance.LoseIntellect(Intellect);
        PlayerManager.Instance.LoseArcanePower(ArcanePower);
    }

    public override string GetDescription()
    {
        string localizedArmor = Armor > 0 ? "\n" + LocalizationSettings.StringDatabase.GetLocalizedString("Tooltip", "ArmorStat") : "";
        string localizedStamina = Stamina > 0 ? "\n" + LocalizationSettings.StringDatabase.GetLocalizedString("Tooltip", "StaminaStat") : "";
        string localizedIntellect = Intellect > 0 ? "\n" + LocalizationSettings.StringDatabase.GetLocalizedString("Tooltip", "IntellectStat") : "";
        string localizedPower = ArcanePower > 0 ? "\n" + LocalizationSettings.StringDatabase.GetLocalizedString("Tooltip", "ArcanePowerStat") : "";

        string composedStats = string.Format(localizedArmor, Armor) + string.Format(localizedStamina, Stamina) + 
            string.Format(localizedIntellect, Intellect) + string.Format(localizedPower, ArcanePower);

        return base.GetDescription() + composedStats;
    }
}
