using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    private CanvasGroup group;
    [SerializeField] private AudioClip openClip;

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

        if (openClip != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(openClip, transform);
        }
    }

    public virtual void OpenClose()
    {
        if (group.alpha == 1)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
