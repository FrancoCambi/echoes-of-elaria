using UnityEngine;
using UnityEngine.Localization.Settings;

public class Gear : Item
{
    public int Armor { get; private set; }
    public int Stamina { get; private set; } 
    public int Intellect { get; private set; }
    public int ArcanePower {  get; private set; }

    public Gear(int id, string name, int reqLevel, int itemLevel, int rarity, int maxStack, 
        int typeId, int armor, int stamina, int intellect, int arcanePower) 
        : base(id, name, reqLevel, itemLevel, rarity, maxStack, typeId)
    {
        Armor = armor;
        Stamina = stamina;
        Intellect = intellect;
        ArcanePower = arcanePower;
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
