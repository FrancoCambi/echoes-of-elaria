using System.Collections.Generic;
using UnityEngine;

public class ActionBarManager : MonoBehaviour
{
    private static ActionBarManager instance;

    public static ActionBarManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<ActionBarManager>();
            return instance;
        }
    }


    [SerializeField]
    private GameObject slotPrefab;

    public List<ActionSlot> Slots = new();


}
