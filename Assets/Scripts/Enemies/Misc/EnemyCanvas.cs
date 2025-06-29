using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour
{
    [SerializeField] private GameObject LevelBarVLO;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Canvas canvas;

    private SpriteRenderer spriteRenderer;

    private EnemyHealth enemyHealth;
    private EnemyData enemyData;

    private bool hideOnExit;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        enemyHealth = GetComponent<EnemyHealth>();
        enemyData = EnemyDatabase.GetEnemyData(EnemyDatabase.GetIdByName(gameObject.name));

        hideOnExit = true;

    }

    public void ShowCanvas()
    {
        UpdateFramePosition();
        UpdateValues();
        canvas.gameObject.SetActive(true);
    }

    public void HideCanvas()
    {
        canvas.gameObject.SetActive(false);
        hideOnExit = true;
    }

    public void ShowCanvasSeconds(float seconds)
    {
        StartCoroutine(ShowCanvasSecondsIE(seconds));
    }

    private IEnumerator ShowCanvasSecondsIE(float seconds)
    {
        hideOnExit = false;
        ShowCanvas();
        yield return new WaitForSeconds(seconds);
        hideOnExit = true;
        HideCanvas();
    }

    private void UpdateFramePosition()
    {
        float colYExtents = spriteRenderer.bounds.extents.y;
        float Yoffset = 0.2f;

        LevelBarVLO.transform.localPosition = new Vector3(0, colYExtents + Yoffset, 0);
        nameText.transform.localPosition = new Vector3(0, -colYExtents - Yoffset, 0);
    }

    private void UpdateValues()
    {
        healthBar.fillAmount = enemyHealth.Health / (float)enemyData.MaxHealth;
        levelText.text = enemyData.Level.ToString();
        nameText.text = enemyData.Name.ToString();
    }

    private void OnMouseEnter()
    {
        ShowCanvas();
    }

    private void OnMouseExit()
    {
        if (hideOnExit) HideCanvas();
    }
}
