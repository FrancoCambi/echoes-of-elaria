using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Loot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI itemName;
    public Item Item { get; set; }
    public int Amount { get; set; }

    private DroppedLoot droppedLoot;

    private EnemyLoot fromEnemy;

    public void Obtain()
    {
        if (InventoryManager.Instance.AddItems(Item.Id, Amount))
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
        Item = item;
        Amount = amount;
        droppedLoot = _droppedLoot;
        fromEnemy = _fromEnemy;

        icon.sprite = Item.Icon;
        amountText.text = amount.ToString();
        itemName.text = item.Name;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Obtain();
        }
    }
}
