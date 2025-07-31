using UnityEngine;
using System.Collections;

public class OpenLSecretRoom : MonoBehaviour,IPickup
{
    [SerializeField] private MeshRenderer childMeshRenderer;
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;
    [SerializeField] private OpenLSecretRoom script;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource openFX;

    private Material outlineMaterial;
    private Coroutine fadeCoroutine;

    void Start()
    {
        openFX.enabled = false;
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
        animator.SetTrigger("Open");
        openFX.enabled = true;
        script.enabled = false;
    }
}
