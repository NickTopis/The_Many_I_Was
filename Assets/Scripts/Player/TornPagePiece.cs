using UnityEngine;
using System.Collections;

public class TornPagePiece : MonoBehaviour, IPickup
{
    [SerializeField] private MeshRenderer childMeshRenderer;
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;

    [SerializeField] AudioSource audioFX;

    [SerializeField] TornPagesChecker checker;

    private Material outlineMaterial;
    private Coroutine fadeCoroutine;

    void Start()
    {
        audioFX.Stop();
        outlineMaterial = childMeshRenderer.materials[1];
    }

    public void ShowPrompt(bool show)
    {
        if (outlineMaterial != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            float targetOpacity = show ? 1f : 0f;
            fadeCoroutine = StartCoroutine(FadeOpacity(targetOpacity));
        }
    }

    private IEnumerator FadeOpacity(float targetOpacity)
    {
        float currentOpacity = outlineMaterial.GetFloat(opacityProperty);
        float duration = outlineShowDuration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newOpacity = Mathf.Lerp(currentOpacity, targetOpacity, elapsed / duration);
            outlineMaterial.SetFloat(opacityProperty, newOpacity);
            yield return null;
        }

        outlineMaterial.SetFloat(opacityProperty, targetOpacity);
    }

    public void PickUp()
    {
        checker.ShowPage();
        audioFX.Stop();
        audioFX.Play();
        Destroy(gameObject);
    }
}
