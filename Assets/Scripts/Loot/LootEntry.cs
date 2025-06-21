using UnityEngine;

public class LootEntry
{
    public int ItemID { get; private set; }
    public float DropChance { get; private set; }
    public int Min {  get; private set; }
    public int Max { get; private set; }    
    public LootEntry(int itemID, float dropChance, int min, int max)
    {
        ItemID = itemID;
        DropChance = dropChance / 100;
        Min = min;
        Max = max;
    }

}
