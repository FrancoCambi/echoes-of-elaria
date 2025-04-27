using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }
            return instance;
        }
    }

    private int selCharID;
    public int SelCharID
    {
        get
        {
            return selCharID;
        }
        set
        {
            selCharID = value;
        }
    }

    private void Start()
    {
        selCharID = 1;
    }
}
