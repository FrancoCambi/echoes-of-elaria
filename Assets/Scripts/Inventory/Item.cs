using UnityEngine;

public class Item
{

    public Item(int id, string name, int maxStack)
    {
        this.Id = id;
        this.Name = name;
        this.MaxStack = maxStack;
        this.Icon = Resources.Load<Sprite>($"Icons/Items/{name}");
    }
    public int Id { get; private set; }
    public string Name { get; private set; }

    public int MaxStack {  get; private set; }
    public Sprite Icon { get; private set; }
}
