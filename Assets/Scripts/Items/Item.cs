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

    public Item(int id, string name, int reqLevel, int itemLevel, int rarity, int maxStack, int typeId) : base(Resources.Load<Sprite>($"Icons/Items/{name}"), maxStack)
    {
        this.Id = id;
        this.Name = name;
        this.RequiredLevel = reqLevel;
        this.ItemLevel = itemLevel;
        this.Rarity = GetItemRarityById(rarity);
        this.Type = GetItemTypeById(typeId);
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

    private ItemType GetItemTypeById(int id)
    {
        return id switch
        {
            1 => ItemType.Consumable,
            2 => ItemType.Equipment,
            3 => ItemType.Material,
            4 => ItemType.Quest,
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
