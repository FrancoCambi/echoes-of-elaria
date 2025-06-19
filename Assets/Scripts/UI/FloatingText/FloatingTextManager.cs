using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloatingTextType
{
    Damage, Heal, Crit, None
}

public class FloatingTextManager : MonoBehaviour
{
    private static FloatingTextManager instance;

    public static FloatingTextManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<FloatingTextManager>();
            return instance;
        }
    }

    private GameObject floatingTextPrefab;
    private List<GameObject> pool;

    private readonly float textDuration = 0.5f;

    public float TextDuration
    {
        get
        {
            return textDuration;
        }
    }

    private void Awake()
    {
        floatingTextPrefab = Resources.Load<GameObject>("Prefabs/FloatingText");
        pool = new List<GameObject>();
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(floatingTextPrefab, transform);
            go.SetActive(false);
            pool.Add(go);
        }
    }

    public void ShowFloatingText(FloatingTextType type, string text, Vector2 position, Vector2 offset)
    {
        if (pool.Count >= 1)
        {
            GameObject go = pool[0];
            pool.Remove(go);
            FloatingText floating = go.GetComponent<FloatingText>();
            SetFloatingText(floating, type, text, position, offset);
            go.SetActive(true);
            StartCoroutine(ReturnToPool(go));
        }
        else
        {
            GameObject go = Instantiate(floatingTextPrefab, transform);
            Destroy(go, textDuration);
        }
    }

    private IEnumerator ReturnToPool(GameObject go)
    {
        yield return new WaitForSeconds(textDuration);
        pool.Add(go);
    }

    private void SetFloatingText(FloatingText floating, FloatingTextType type, string text, Vector2 position, Vector2 offset)
    {
        if (type == FloatingTextType.Heal)
        {
            floating.Text = text;
            floating.TextColor = Color.green;
            floating.Position = position;
            floating.Offset = offset;
        }
    }
}
