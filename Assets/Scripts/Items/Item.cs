using UnityEngine;

public enum ItemType
{
    Consumable, Equipment, Material, Quest
}
public class Item
{

    public Item(int id, string name, int maxStack, int typeId)
    {
        this.Id = id;
        this.Name = name;
        this.MaxStack = maxStack;
        this.Type = GetItemTypeById(typeId);
        this.Icon = Resources.Load<Sprite>($"Icons/Items/{name}");
    }
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int MaxStack {  get; private set; }
    public ItemType Type { get; private set; }
    public Sprite Icon { get; private set; }

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
}
