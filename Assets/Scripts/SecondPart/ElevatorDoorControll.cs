using UnityEngine;
using System.Collections;

public class ElevatorDoorControll : MonoBehaviour, IPickup
{

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;
    [SerializeField] Animator animator;
    [SerializeField] ShaftLeverControll leverControll;
    [SerializeField] Collider leverCollider;
    [SerializeField] AudioSource openAudioFX;
    [SerializeField] AudioSource closeAudioFX;

    private bool opened;


    private Material outlineMaterial;
    private Coroutine fadeCoroutine;
    void Start()
    {
        openAudioFX.Stop();
        closeAudioFX.Stop();
        opened = false;
        outlineMaterial = meshRenderer.materials[1];
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
        if (leverControll.isMoving == false)
        {
            openAudioFX.Stop();
            closeAudioFX.Stop();
            if (!opened)
            {
                opened = true;
                animator.SetTrigger("OpenDoor");
                openAudioFX.Play();
                leverCollider.enabled = false;
                return;
            }

            if (opened)
            {
                opened = false;
                animator.SetTrigger("CloseDoor");
                closeAudioFX.Play();
                leverCollider.enabled = true;
                return;
            }
        }
    }
}
