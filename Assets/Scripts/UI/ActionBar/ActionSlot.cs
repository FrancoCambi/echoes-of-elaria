using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ActionSlot : BaseSlot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Sprite emptyBackground;
    [SerializeField]
    private Sprite fullBackground;
    private Image background;
    [SerializeField]
    private TextMeshProUGUI bindingText;


    private InventorySlot itemSlot;
    private KeyBinding keybind;

    public KeyBinding KeyBind
    {
        get
        {
            return keybind;
        }
    }

    public override void Awake()
    {
        base.Awake();
        background = GetComponent<Image>();

    }

    private void Start()
    {
        keybind = KeyBindsManager.Instance.LoadBinding(GetSlotIndex());
        UpdateBindingText();
    }

    private void OnEnable()
    {
        ActionBarManager.Instance.Slots.Add(this);
    }

    private void OnDisable()
    {
        ActionBarManager.Instance.Slots.Remove(this);

    }

    #region methods

    public override void SetContent(SlotContent newContent)
    {
        base.SetContent(newContent);

        background.sprite = fullBackground;
        UpdateText();
    }

    public override void UpdateText()
    {
        if (amountText == null || itemSlot == null) return;

        amountText.text = itemSlot.Amount > 1 ? itemSlot.Amount.ToString() : "";
    }


    public void SetItemSlot(InventorySlot slot)
    {
        if (slot == null) return;
        if (itemSlot != null)
        {
            itemSlot.OnInventorySlotAmountChanged -= ItemUsedFromInventory;
        }

        itemSlot = slot;
        itemSlot.OnInventorySlotAmountChanged += ItemUsedFromInventory;
    }

    private void ItemUsedFromInventory()
    {
        UpdateText();
        if (itemSlot.Amount <= 0)
        {
            Clear();
        }
    }

    public void Use()
    {
        if (!IsEmpty && itemSlot != null)
        {
            (itemSlot.Content as Item).Use();
            itemSlot.AddAmount(-1);
        }
    }

    public void SetKeybind(KeyBinding binding)
    {
        keybind = binding;
        UpdateBindingText();
    }

    public void RemoveKeybind()
    {
        keybind.key = Key.None;
        keybind.modifiers = EventModifiers.None;
        UpdateBindingText();
    }

    private void UpdateBindingText()
    {
        if (bindingText)
        {
            bindingText.text = keybind.ToString();
        }
    }

    public override void Clear()
    {
        base.Clear();

        background.sprite = emptyBackground;

    }
    public override int GetSlotIndex()
    {
        return ActionBarManager.Instance.Slots.IndexOf(this);
    }

    #endregion

    #region interfaces

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsEmpty && eventData.button == PointerEventData.InputButton.Left)
        {
            DragManager.Instance.StartDrag(content, icon.sprite, this);
            icon.color = Color.gray;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        DragManager.Instance.MoveImage(Input.mousePosition);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        icon.color = Color.white;
        DragManager.Instance.EndDrag();

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Clear();
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        BaseSlot fromSlot = DragManager.Instance.FromSlot;

        if (fromSlot == this || fromSlot == null || fromSlot.Content is not Item) return;

        if (fromSlot is InventorySlot)
        {
            SetItemSlot(fromSlot as InventorySlot);
            SetContent(fromSlot.Content);
        }
        else if (fromSlot is ActionSlot && IsEmpty)
        {
            Clear();
            SetItemSlot((fromSlot as ActionSlot).itemSlot);
            SetContent(fromSlot.Content);
            fromSlot.Clear();
        }
        else if (fromSlot is ActionSlot && !IsEmpty)
        {
            SlotContent tmpContent = Content;
            InventorySlot tmpSlot = itemSlot;
            Clear();
            SetItemSlot((fromSlot as ActionSlot).itemSlot);
            SetContent(fromSlot.Content);

            fromSlot.Clear();
            (fromSlot as ActionSlot).SetItemSlot(tmpSlot);
            fromSlot.SetContent(tmpContent);


        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsEmpty && eventData.button == PointerEventData.InputButton.Right)
        {
            // Consumables are the ones with effects.
            if (content is Item contentItem && contentItem.Type == ItemType.Consumable)
            {
                Use();
            }
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {

        if (KeyBindsManager.Instance.Listening)
        {
            KeyBindsManager.Instance.HoveredSlot = this;
        }
        else
        {
           base.OnPointerEnter(eventData);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (KeyBindsManager.Instance.Listening)
        {
            KeyBindsManager.Instance.HoveredSlot = null;
        }
        else
        {
            base.OnPointerExit(eventData);
        }
    }

    #endregion
}
