using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float textDuration;
    private float speed = 5f;
    private float timeElapsed = 0f;
    public string Text {  get; set; }
    public Color TextColor { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Offset { get; set; }

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        textDuration = FloatingTextManager.Instance.TextDuration;
    }

    private void OnEnable()
    {
        text.text = Text;
        text.color = TextColor;
        transform.position = Position + Offset;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - (timeElapsed / textDuration));
        transform.position += speed * Time.deltaTime * new Vector3(0, 1, 0);

        if (timeElapsed >= textDuration)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        text.text = "";
        text.color = Color.white;
        transform.position = Vector2.zero;
        timeElapsed = 0f;
    }
}
