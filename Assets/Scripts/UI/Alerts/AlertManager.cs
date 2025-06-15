using System.Collections;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    private static AlertManager instance;

    public static AlertManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<AlertManager>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject background;
    [SerializeField]
    private TextMeshProUGUI alertText;
    private bool showing;

    private void Start()
    {
        showing = false;
    }

    public void ThrowAlert(string msg, float duration = 2f)
    {
        if (!showing) StartCoroutine(ThrowAlertRoutine(msg, duration));
    }

    private IEnumerator ThrowAlertRoutine(string msg, float duration)
    {
        background.SetActive(true);
        alertText.text = msg;
        showing = true;
        yield return new WaitForSeconds(duration);
        alertText.text = "";
        background.SetActive(false);
        showing = false;
    }
}
