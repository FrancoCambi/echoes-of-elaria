using UnityEngine;

public enum NPCType
{
    Vendor, QuestGiver, None
}
public class NPCData
{
    public int ID;
    public string Name;
    public NPCType Type;
}
