using UnityEngine;
using UnityEngine.Localization.Settings;

public enum ItemType
{
    Consumable, Equipment, Material, Quest, None
}

public enum ItemRarity
{
    Common, Uncommon, Rare, Epic, Legendary, None
}

public class Item : SlotContent
{

    public Item(int id, string name, int reqLevel, int itemLevel, ItemRarity rarity, int maxStack, ItemType type) : base(Resources.Load<Sprite>($"Icons/Items/{name}"), maxStack)
    {
        this.Id = id;
        this.Name = name;
        this.RequiredLevel = reqLevel;
        this.ItemLevel = itemLevel;
        this.Rarity = rarity;
        this.Type = type;
    }

    #region properties


    public int Id { get; private set; }
    public string Name { get; private set; }
    public int RequiredLevel { get; private set; }
    public int ItemLevel { get; private set; }
    public ItemRarity Rarity { get; private set; }
    public ItemType Type { get; private set; }

    #endregion
    public void Use()
    {
        foreach (ItemEffect effect in ItemsManager.Instance.GetEffectsByID(Id))
        {
            effect.Apply();
        }
    }

    public override bool CanStackWith(SlotContent other)
    {
        if (other is Item otherItem)
        {
            return Id == otherItem.Id;
        }
        return false;
    }


    public static string GetRarityColor(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Common => "#FFFFFF",
            ItemRarity.Uncommon => "#00FF00",
            ItemRarity.Rare => "#0000FF",
            ItemRarity.Epic => "#FF00FF",
            ItemRarity.Legendary => "#FFA500",
            _ => "#FFFFFF"
        };
    }


    public override string GetDescription()
    {
        string rarityColor = GetRarityColor(Rarity);
        string localizedDescription = LocalizationSettings.StringDatabase.GetLocalizedString("Tooltip", "itemTooltipDescription");
        string localizedName = LocalizationSettings.StringDatabase.GetLocalizedString("ItemNames", $"{Id}");

        return string.Format(localizedDescription, rarityColor, localizedName, ItemLevel, RequiredLevel);
    }
}
