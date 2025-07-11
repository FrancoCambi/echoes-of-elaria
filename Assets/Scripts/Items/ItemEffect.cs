using UnityEngine;

public enum ItemEffectType
{
    HealHp, GainMana, GainRage, AddExp, AddGold, AddItem, Teleport, Custom
}

public enum TargetType
{
    Self, Enemy
}

public class ItemEffect 
{
    public ItemEffect(int id, int itemId, ItemEffectType type, int value, int duration, TargetType tType, string extraParam)
    {
        Id = id;
        ItemId = itemId;
        Type = type;
        Value = value;
        Duration = duration;
        TType = tType;
        ExtraParam = extraParam;
    }

    public int Id { get; private set; }
    public int ItemId { get; private set; }
    public ItemEffectType Type { get; private set; }
    public int Value {  get; private set; }
    public int Duration { get; private set; }
    public TargetType TType { get; private set; }
    public string ExtraParam {  get; private set; }

    public void Apply()
    {
        switch (Type)
        {
            case ItemEffectType.HealHp:
                HealHp();
                break;
            case ItemEffectType.GainRage:
                GainRage();
                break;
            default:
                break;

        }
    }

    private void HealHp()
    {
        PlayerManager.Instance.GainCurrentHealth(Value);
    }

    private void GainRage()
    {
        PlayerManager.Instance.GainRage(Value);
    }
}
