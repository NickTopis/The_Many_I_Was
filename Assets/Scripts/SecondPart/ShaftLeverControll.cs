using UnityEngine;
using System.Collections;

public class ShaftLeverControll : MonoBehaviour,IPickup
{

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;
    [SerializeField] Animator animator;
    [SerializeField] Collider maincollider;

    [SerializeField] AudioSource audioFX;

    private bool down;
    public bool isMoving;


    private Material outlineMaterial;
    private Coroutine fadeCoroutine;
    void Start()
    {
        audioFX.Stop();
        isMoving = false;
        down = false;
        outlineMaterial = meshRenderer.materials[2];
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
        if (!isMoving)
        {
            audioFX.Stop();
            isMoving = true;
            if (!down)
            {
                maincollider.enabled = false;
                down = true;
                animator.SetTrigger("GoDown");
                audioFX.Play();
                StartCoroutine(FadeOpacity(0f));
                return;
            }

            if (down)
            {
                maincollider.enabled = false;
                down = false;
                animator.SetTrigger("GoUp");
                audioFX.Play();
                StartCoroutine(FadeOpacity(0f));
                return;
            }
        }
    }

    
}
