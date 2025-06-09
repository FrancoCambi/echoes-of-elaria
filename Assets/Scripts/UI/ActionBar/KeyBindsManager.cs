using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public struct Binding
{
    public EventModifiers modifiers;
    public KeyCode key;

    public Binding(EventModifiers modifiers, KeyCode key)
    {
        this.modifiers = modifiers;
        this.key = key;
    }
}

public class KeyBindsManager : MonoBehaviour
{
    private static KeyBindsManager instance;

    public static KeyBindsManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<KeyBindsManager>();
            return instance;
        }
    }

    private Dictionary<Binding, ActionSlot> bindings = new();

    private bool hearing;

    private ActionSlot heard;

    public bool IsHearing
    {
        get
        {
            return hearing;
        }
    }

    public ActionSlot Heard
    {
        get
        {
            return heard;
        }
        set
        {
            heard = value;
        }
    }

    private void Start()
    {
        //bindings.Add(new Binding(EventModifiers.None, KeyCode.Alpha1), );
    }

    public void BindKey(Binding binding, ActionSlot slot)
    {
        if (bindings.ContainsKey(binding))
        {
            bindings[binding] = slot;
        }
        else
        {
            bindings.Add(binding, slot);
        }

        Debug.Log(binding.key);
    }

    public void UnbindKey(Binding binding)
    {
        if (bindings.ContainsKey(binding))
        {
            bindings.Remove(binding);
        }
    }

    public void StartHearing()
    {
        hearing = true;
    }

    public void StopHearing()
    {
        hearing = false;
    }

    private bool CheckValidKey(KeyCode code)
    {
        return code != KeyCode.None &&
            code != KeyCode.LeftShift &&
            code != KeyCode.RightShift &&
            code != KeyCode.LeftControl &&
            code != KeyCode.RightControl &&
            code != KeyCode.AltGr &&
            code != KeyCode.LeftAlt &&
            code != KeyCode.RightAlt &&
            code != KeyCode.LeftCommand && 
            code != KeyCode.RightCommand &&
            code != KeyCode.Numlock &&
            code != KeyCode.CapsLock;
    }

    private void OnGUI()
    {
        if (!Event.current.isKey) return;


        if (!hearing)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None)
            {
                Binding binding = new(Event.current.modifiers, Event.current.keyCode);
                if (bindings.ContainsKey(binding))
                {
                    bindings[binding].Use();

                }

            }

        }
        else
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None && CheckValidKey(Event.current.keyCode) && heard != null)
            {
                Binding binding = new(Event.current.modifiers, Event.current.keyCode);

                UnbindKey(binding);

                BindKey(binding, heard);


            }
        }
    }

}
