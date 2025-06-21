using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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

    public void Obtain()
    {
        if (InventoryManager.Instance.AddItems(Item.Id, Amount))
        {
            LootManager.Instance.CurrentLoot.Remove(this);
            if (LootManager.Instance.CurrentLoot.Count == 0)
            {
                LootManager.Instance.Close();
            }
            Destroy(gameObject);
        }
    }

    public void SetUpLoot(Item item, int amount)
    {
        Item = item;
        Amount = amount;

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
