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

    private new PolygonCollider2D collider;

    private EnemyHealth enemyHealth;
    private EnemyData enemyData;

    private void Start()
    {
        collider = GetComponent<PolygonCollider2D>();

        enemyHealth = GetComponent<EnemyHealth>();
        enemyData = EnemyDatabase.GetEnemyData(EnemyDatabase.GetIdByName(gameObject.name));

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
    }

    public void UpdateFramePosition()
    {
        float colYExtents = collider.bounds.extents.y;
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
        HideCanvas();
    }
}
