using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    protected CanvasGroup group;

    public virtual void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    public virtual void Close()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
    }
    public virtual void OpenClose()
    {
        group.alpha = group.alpha != 0 ? 0 : 1;
        group.blocksRaycasts = !group.blocksRaycasts;
    }
}
