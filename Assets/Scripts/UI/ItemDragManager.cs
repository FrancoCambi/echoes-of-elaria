using UnityEngine;
using UnityEngine.UI;

public class ItemDragManager : MonoBehaviour
{
    private static ItemDragManager instance;

    public static ItemDragManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<ItemDragManager>();
            return instance;
        }
    }

    private Image draggedImage;

    public Item DraggedItem { get; private set; }
    public int DraggedAmount { get; private set; }
    public Slot DraggedInventorySlot { get; private set; }
    public ActionSlot DraggedActionSlot { get; private set; }

    public bool IsDragging
    {
        get
        {
            return draggedImage.enabled;
        }
    }

    public bool IsInventoryDrag
    {
        get
        {
            return IsDragging && DraggedInventorySlot != null;
        }
    }

    public bool IsActionDrag
    {
        get
        {
            return IsDragging && DraggedActionSlot != null;
        }
    }

    private void Start()
    {
        draggedImage = GetComponent<Image>();
    }

    public void StartDrag(Item item, int stack, Sprite icon, Slot slot)
    {
        DraggedItem = item;
        DraggedAmount = stack;
        DraggedInventorySlot = slot;
        draggedImage.sprite = icon;
        draggedImage.enabled = true;
    }

    public void StartDrag(Item item, int stack, Sprite icon, ActionSlot slot)
    {
        DraggedItem = item;
        DraggedAmount = stack;
        DraggedActionSlot = slot;
        draggedImage.sprite = icon;
        draggedImage.enabled = true;
    }

    public void EndDrag()
    {
        DraggedItem = null;
        DraggedAmount = 0;
        draggedImage.enabled = false;
    }

    public void MoveImage(Vector3 newPos)
    {
        draggedImage.transform.position = newPos;
    }
}
