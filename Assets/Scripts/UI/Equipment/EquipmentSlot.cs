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

    public void EquipGear(Gear gear, bool updateStats = true)
    {
        SetContent(gear);
        AddAmount(1);
        equippedGear = gear;
        gear.Equip(updateStats);
    }

    public void UnEquipGear(bool updateStats = true)
    {
        if (equippedGear == null) return;
        
        equippedGear.UnEquip(updateStats);
        equippedGear = null;
        EquipmentManager.Instance.UnEquipGear();
        
    }


    public override void Clear()
    {
        base.Clear();

        UnEquipGear();
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

        if (!EventSystem.current.IsPointerOverGameObject()
            && InventoryManager.Instance.AddItems(equippedGear.Id, 1))
        {

            Clear();
        }

    }

    public override void OnDrop(PointerEventData eventData)
    {
        BaseSlot fromSlot = DragManager.Instance.FromSlot;

        if (fromSlot == this || fromSlot == null || fromSlot.Content is not Gear ||
            fromSlot.Content is Gear && (fromSlot.Content as Gear).EquipType != equipType) return;

        // From inventory slot
        if (fromSlot is InventorySlot)
        {
            if (IsEmpty)
            {
                EquipmentManager.Instance.EquipGear(fromSlot.Content as Gear);
                fromSlot.AddAmount(-1);
            }
            else
            {
                int id = equippedGear.Id;
                EquipmentManager.Instance.EquipGear(fromSlot.Content as Gear);
                fromSlot.AddAmount(-1);
                InventoryManager.Instance.AddItemsInSlot(id, 1, fromSlot.GetSlotIndex());


            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        return;
    }
}
