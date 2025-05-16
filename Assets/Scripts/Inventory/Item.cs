using UnityEngine;

public class Item
{

    public Item(int id, string name)
    {
        this.Id = id;
        this.Name = name;
        this.Icon = Resources.Load<Sprite>($"Icons/Items/{name}");
    }
    public int Id { get; private set; }
    public string Name { get; private set; }

    public Sprite Icon { get; private set; }
}
