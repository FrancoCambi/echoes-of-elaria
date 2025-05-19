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
    public Slot DraggedSlot { get; private set; }

    public bool IsDragging
    {
        get
        {
            return draggedImage.enabled;
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
        DraggedSlot = slot;
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
