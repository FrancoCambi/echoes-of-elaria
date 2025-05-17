using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private Image icon;

    private TextMeshProUGUI amountText;

    private int itemID;
    private int amount;
    public int ItemID
    {
        get
        {
            return itemID;
        }
        set
        {
            itemID = value;
        }
    }
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

    void Update()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Debug.Log("Hovering over: " + result.gameObject.name);
        }
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
        icon.enabled = true;
        icon.sprite = item.Icon;
        
        amount = quantity;
        itemID = id;
        UpdateText();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (icon.enabled)
        {
            ItemDragManager.Instance.StartDrag(ItemsManager.Instance.GetItemByID(itemID), amount, icon.sprite);
            icon.color = Color.gray;

        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        ItemDragManager.Instance.MoveImage(Input.mousePosition);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        icon.color = Color.white;
        ItemDragManager.Instance.EndDrag();

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ItemDragManager.Instance.IsDragging)
        {
            Item item = ItemDragManager.Instance.DraggedItem;
            icon.enabled = true;
            icon.sprite = item.Icon;
            amount = ItemDragManager.Instance.DraggedStack;
            itemID = item.Id;
            UpdateText();
        }
    }



}
