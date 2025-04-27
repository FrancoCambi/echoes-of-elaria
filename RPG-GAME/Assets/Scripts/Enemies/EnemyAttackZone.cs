using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackZone : MonoBehaviour
{
    private List<Collider2D> detectedObjs;

    public Collider2D PlayerCollider
    {
        get
        {
            foreach(Collider2D obj in detectedObjs)
            {
                if (obj.gameObject.CompareTag("Player"))
                {
                    return obj;
                }
            }
            return null;
        }
    }

    private void Start()
    {
        detectedObjs = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedObjs.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedObjs.Remove(collision);
    }
}
