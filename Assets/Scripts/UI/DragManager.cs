using UnityEngine;
using UnityEngine.UI;

public class DragManager : MonoBehaviour
{
    private static DragManager instance;

    public static DragManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<DragManager>();
            return instance;
        }
    }

    private Image draggedImage;

    public SlotContent DraggedContent { get; private set; }
    public BaseSlot FromSlot { get; private set; }

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

    public void StartDrag(SlotContent content, Sprite icon, BaseSlot fromSlot)
    {
        DraggedContent = content;
        FromSlot = fromSlot;
        draggedImage.sprite = icon;
        draggedImage.enabled = true;

    }

    public void EndDrag()
    {
        DraggedContent = null;
        draggedImage.enabled = false;
    }

    public void Drop()
    {
        FromSlot = null;
    }

    public void MoveImage(Vector3 newPos)
    {
        draggedImage.transform.position = newPos;
    }
}
