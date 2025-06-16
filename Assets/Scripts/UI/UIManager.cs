using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
            InventoryManager.Instance, MenuManager.Instance, OptionsManager.Instance
        };
    }

    void Update()
    {
        if (KeyBindsManager.Instance.Listening) return;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (AnyOpen())
            {
                foreach (Panel panel in panelList)
                {
                    panel.Close();
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
