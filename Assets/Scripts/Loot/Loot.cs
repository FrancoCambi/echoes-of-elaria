using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;

    private LootSlot lootSlot;

    private DroppedLoot droppedLoot;

    private EnemyLoot fromEnemy;

    public LootSlot Lootslot
    {
        get
        {
            return lootSlot;
        }
    }

    private void Awake()
    {
        lootSlot = GetComponentInChildren<LootSlot>();
    }

    public void Obtain()
    {
        if (InventoryManager.Instance.AddItems((lootSlot.Content as Item).Id, lootSlot.Amount))
        {
            LootManager.Instance.CurrentLoot.Remove(this);
            fromEnemy.RemoveFromDropped(droppedLoot);

            if (LootManager.Instance.CurrentLoot.Count == 0)
            {
                LootManager.Instance.Close();
            }
            Destroy(gameObject);
        }
    }

    public void SetUpLoot(Item item, int amount, DroppedLoot _droppedLoot, EnemyLoot _fromEnemy)
    {
        lootSlot.Loot = this;
        lootSlot.SetContent(item);
        lootSlot.AddAmount(amount);
        droppedLoot = _droppedLoot;
        fromEnemy = _fromEnemy;

        itemName.text = item.Name;
    }

}
