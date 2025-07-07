using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : BaseSlot
{
    [SerializeField] private EquipmentType equipType;

    private Gear equippedGear;

    public EquipmentType EquipType
    {
        get
        {
            return equipType;
        }
    }

    public Gear EquippedGear
    {
        get
        {
            return equippedGear;
        }
    }

    public void EquipGear(Gear gear)
    {
        SetContent(gear);
        AddAmount(1);
        equippedGear = gear;
        gear.Equip();
    }

    public void UnEquipGear()
    {
        Clear();

        if (equippedGear != null)
        {
            InventoryManager.Instance.AddItems(equippedGear.Id, 1);
            equippedGear.UnEquip();
            equippedGear = null;
        }

    }

    public override int GetSlotIndex()
    {
        return EquipmentManager.Instance.Slots.IndexOf(this);
    }

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
            UnEquipGear();
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {

        BaseSlot fromSlot = DragManager.Instance.FromSlot;

        if (fromSlot == this || fromSlot == null || fromSlot.Content is not Gear ||
            fromSlot.Content is Gear && (fromSlot.Content as Gear).EquipType != equipType) return;

        EquipmentManager.Instance.EquipGear(fromSlot.Content as Gear);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        return;
    }
}
