using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    private TextMeshProUGUI amountText;

    private int itemID;
    private int amount;
    public int ItemID { get; set; }
    public int Amount
    {
        get
        {
            return amount;
        }
        set
        {
            amount = value;
            UpdateText();
        }
    }

    private void Start()
    {
        amountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public bool IsEmpty
    {
        get
        {
            return amount == 0;
        }
    }
    public void UpdateText()
    {
        amountText.text = amount.ToString();
    }

    public void AddItem(int id, int quantity)
    {
        Item item = ItemsManager.Instance.GetItemByID(id);
        icon.sprite = item.Icon;
        icon.enabled = true;
        
        amount = quantity;
        UpdateText();
    }

}
