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


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
         
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryManager.Instance.OpenCloseUI();
        }
    }
}
