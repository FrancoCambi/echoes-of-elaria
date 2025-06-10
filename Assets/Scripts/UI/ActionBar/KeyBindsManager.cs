using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public struct KeyBinding
{
    public Key key;
    public EventModifiers modifiers;

    public bool IsEmpty
    {
        get
        {
            return key == Key.None;
        }
    }
    public bool Matches(Keyboard kb)
    {
        if (!kb[key].wasPressedThisFrame) return false;

        bool shift = (modifiers & EventModifiers.Shift) != 0;
        bool ctrl = (modifiers & EventModifiers.Control) != 0;
        bool alt = (modifiers & EventModifiers.Alt) != 0;

        return
            kb[Key.LeftShift].isPressed == shift &&
            kb[Key.LeftCtrl].isPressed == ctrl &&
            kb[Key.LeftAlt].isPressed == alt;
    }

    public override string ToString()
    {
        string mod = "";
        if ((modifiers & EventModifiers.Control) != 0) mod += "C";
        if ((modifiers & EventModifiers.Shift) != 0) mod += "S";
        if ((modifiers & EventModifiers.Alt) != 0) mod += "A";

        string stringKey = NormalizeKey(key);
        return $"{mod}{stringKey}";
    }

    private string NormalizeKey(Key key)
    {
        switch (key)
        {
            // Special characters
            case Key.Semicolon: return ";";
            case Key.Quote: return "'";
            case Key.Comma: return ",";
            case Key.Period: return ".";
            case Key.Slash: return "/";
            case Key.Backslash: return "\\";
            case Key.LeftBracket: return "[";
            case Key.RightBracket: return "]";
            case Key.Minus: return "-";
            case Key.Equals: return "=";
            case Key.Backquote: return "`";
            case Key.NumpadDivide: return "/";
            case Key.NumpadMultiply: return "*";
            case Key.NumLock: return "NLock";
            case Key.Insert: return "Ins";
            case Key.PageUp: return "PagUP";
            case Key.PageDown: return "PagDn";
            case Key.End: return "End";
            case Key.ScrollLock: return "Bloq";
            case Key.PrintScreen: return "Impr";
            case Key.None: return "";

            default:
                return key.ToString().Replace("Digit", "").Replace("Numpad", "");
        }

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

    private bool listening;

    private ActionSlot hoveredSlot;

    public bool Listening
    {
        get
        {
            return listening;
        }
    }

    public ActionSlot HoveredSlot
    {
        get
        {
            return hoveredSlot;
        }
        set
        {
            hoveredSlot = value;
        }
    }

    private void Update()
    {
        if (listening && hoveredSlot != null)
        {
            foreach (KeyControl key in Keyboard.current.allKeys)
            {
                if (key != null && key.wasPressedThisFrame)
                {
                    if (IsModifier(key.keyCode)) return;
                    if (key.keyCode == Key.Escape)
                    {
                        hoveredSlot.RemoveKeybind();
                        return;
                    }

                    KeyBinding binding = new KeyBinding
                    {
                        key = key.keyCode,
                        modifiers = GetModifiers()
                    };

                    hoveredSlot.SetKeybind(binding);
                    SaveBinding(hoveredSlot.GetSlotIndex(), binding);

                }
            }
        }
        else if (!listening)
        {
            foreach (ActionSlot slot in ActionBarManager.Instance.Slots)
            {
                KeyBinding binding = slot.KeyBind;

                if (!binding.IsEmpty && Keyboard.current[binding.key].wasPressedThisFrame && binding.Matches(Keyboard.current))
                {
                    slot.Use();
                }
            }
        }
    }

    private bool IsModifier(Key key)
    {
        return key == Key.LeftCtrl || key == Key.RightCtrl ||
               key == Key.LeftShift || key == Key.RightShift ||
               key == Key.LeftAlt || key == Key.RightAlt;
    }

    private EventModifiers GetModifiers()
    {
        var kb = Keyboard.current;
        EventModifiers mods = 0;
        if (kb.leftShiftKey.isPressed || kb.rightShiftKey.isPressed) mods |= EventModifiers.Shift;
        if (kb.leftCtrlKey.isPressed || kb.rightCtrlKey.isPressed) mods |= EventModifiers.Control;
        if (kb.leftAltKey.isPressed || kb.rightAltKey.isPressed) mods |= EventModifiers.Alt;
        return mods;
    }

    private void SaveBinding(int index, KeyBinding binding)
    {
        string json = JsonUtility.ToJson(binding);
        PlayerPrefs.SetString($"keybind_{index}", json);
        PlayerPrefs.Save();
    }

    public KeyBinding LoadBinding(int index)
    {
        string json = PlayerPrefs.GetString($"keybind_{index}", "");
        if (string.IsNullOrEmpty(json)) return default;
        return JsonUtility.FromJson<KeyBinding>(json);
    }

    public void StartListening()
    {
        listening = true;
    }

    public void StopListening()
    {
        listening = false;
    }

}
