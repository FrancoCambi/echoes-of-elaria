using System.Collections;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material spawnMaterial;
    private Material defaultMaterial;

    private readonly float dissolveTime = 0.75f;

    private readonly int dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = Resources.Load<Material>("Shaders/DefaultShaderMat");
        spawnMaterial = Resources.Load<Material>("Shaders/SpawnShaderMat");
    }

    private void Start()
    {
        StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        spriteRenderer.material = spawnMaterial;

        float elapsedTime = 0f;

        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / dissolveTime));

            spriteRenderer.material.SetFloat(dissolveAmount, lerpedDissolve);

            yield return null;
        }

        spriteRenderer.material = defaultMaterial;
    }
}
