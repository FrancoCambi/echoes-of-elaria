using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    protected CanvasGroup group;

    public virtual void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    public bool IsOpen
    {
        get
        {
            return group.alpha == 1;
        }
    }

    public virtual void Close()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
    }

    public virtual void Open()
    {
        group.alpha = 1;
        group.blocksRaycasts = true;
    }

    public virtual void OpenClose()
    {
        group.alpha = group.alpha != 0 ? 0 : 1;
        group.blocksRaycasts = !group.blocksRaycasts;
    }
}
