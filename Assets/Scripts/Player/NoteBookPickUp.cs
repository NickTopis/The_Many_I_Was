using UnityEngine;
using System.Collections;

public class NoteBookPickUp : MonoBehaviour, IPickup
{
    [SerializeField] private MeshRenderer childMeshRenderer;
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;
    [SerializeField] GameObject bookIndicator;
    [SerializeField] AudioSource audioFX;

    [SerializeField] BookController bookController;
    [SerializeField] GameObject[] pieces;

    private Material outlineMaterial;
    private Coroutine fadeCoroutine;

    void Start()
    {
        audioFX.Stop();
        outlineMaterial = childMeshRenderer.materials[3];
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
        bookIndicator.SetActive(true);
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].GetComponent<TornPagePiece>().enabled = true;
        }
        audioFX.Stop();
        audioFX.Play();
        bookController.gotbook = true;

        BookPickUpManager manager = FindObjectOfType<BookPickUpManager>();
        if (manager != null)
        {
            manager.SetBookPickedUp(true);
            manager.ApplyBookState();
        }

        Destroy(gameObject);
    }
}
