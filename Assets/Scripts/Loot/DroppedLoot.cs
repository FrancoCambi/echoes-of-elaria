using UnityEngine;
using UnityEngine.Rendering;

public class DroppedLoot
{
    public int Id { get; set; }
    public int Amount { get; set; }

    public DroppedLoot(int id, int amount)
    {
        Id = id;
        Amount = amount;
    }
}
