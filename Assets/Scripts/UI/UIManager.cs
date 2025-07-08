using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<UIManager>();
            return instance;
        }
    }

    private List<Panel> panelList = new();
 
    private void Awake()
    {
        panelList = new()
        {
            InventoryManager.Instance, MenuManager.Instance, OptionsManager.Instance, LootManager.Instance, EquipmentManager.Instance
        };
    }

    void Update()
    {
        // If Input is blocked, just return.
        if (KeyBindsManager.Instance.Listening) return;
        if (GameManager.Instance.IsInputBlocked()) return;

        // Otherwise, read input.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (AnyOpen())
            {
                foreach (Panel panel in panelList)
                {
                    if (panel.IsOpen)
                    {
                        panel.Close();
                    }
                }
            }
            else
            {
                MenuManager.Instance.Open();
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryManager.Instance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            EquipmentManager.Instance.OpenClose();
        }
    }

    private bool AnyOpen()
    {
        foreach (Panel panel in panelList)
        {
            if (panel.IsOpen)
            {
                return true;
            }
        }

        return false;
    }

}
