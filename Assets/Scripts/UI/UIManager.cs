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
            InventoryManager.Instance
        };
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (Panel panel in panelList)
            {
                panel.Close();
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryManager.Instance.OpenClose();
        }
    }
}
