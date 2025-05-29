using System.Collections.Generic;
using UnityEngine;

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
    }

    public void UnbindKey(Binding binding)
    {
        if (bindings.ContainsKey(binding))
        {
            bindings.Remove(binding);
        }
    }

    private void OnGUI()
    {

        if (Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            Binding binding;
            binding.modifiers = Event.current.modifiers;
            binding.key = Event.current.keyCode;

            if (bindings.ContainsKey(binding))
            {
                bindings[binding].Use();
            }
        }
    }
}
