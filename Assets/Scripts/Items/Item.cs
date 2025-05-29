using UnityEngine;

public enum ItemType
{
    Consumable, Equipment, Material, Quest
}
public class Item : SlotContent
{

    public Item(int id, string name, int typeId, int maxStack) : base(Resources.Load<Sprite>($"Icons/Items/{name}"), maxStack)
    {
        this.Id = id;
        this.Name = name;
        this.Type = GetItemTypeById(typeId);
    }
    public int Id { get; private set; }
    public string Name { get; private set; }
    public ItemType Type { get; private set; }
    private ItemType GetItemTypeById(int id)
    {
        return id switch
        {
            1 => ItemType.Consumable,
            2 => ItemType.Equipment,
            3 => ItemType.Material,
            _ => ItemType.Quest,
        };
    }

    public override bool CanStackWith(SlotContent other)
    {
        if (other is Item otherItem)
        {
            return Id == otherItem.Id;
        }
        return false;
    }

    public void Use()
    {
        foreach (ItemEffect effect in ItemsManager.Instance.GetEffectsByID(Id))
        {
            effect.Apply();
        }
    }
}
