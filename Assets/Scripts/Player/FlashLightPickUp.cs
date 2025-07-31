using UnityEngine;
using System.Collections;
using System.Xml.Linq;

public class FlashLightPickUp : MonoBehaviour, IPickup
{
    [SerializeField] FlashLightController flashLightController;
    [SerializeField] private MeshRenderer childMeshRenderer; // The child with the materials
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;

    [SerializeField] GameObject objectOnPlayer;
    private AudioSource audioSFX;
    [SerializeField] GameObject textObj;

    private Material outlineMaterial;
    private Coroutine fadeCoroutine;
    private GameObject sfxObject;

    private void Start()
    {
        flashLightController.enabled = false;
        sfxObject = GameObject.FindGameObjectWithTag("BasicPickUpSFX");
        audioSFX = sfxObject.GetComponent<AudioSource>();
        outlineMaterial = childMeshRenderer.materials[1];

        textObj.SetActive(false);
        //objectOnPlayer.SetActive(false);
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
        textObj.SetActive(true);
        audioSFX.Stop();
        audioSFX.Play();
        objectOnPlayer.SetActive(true);
        flashLightController.enabled = true;

        FlashlightStateManager manager = FindObjectOfType<FlashlightStateManager>();
        if (manager != null)
        {
            manager.SetPickedUp(true);
            manager.ApplyFlashlightState();
        }

        Destroy(gameObject);
    }
}
