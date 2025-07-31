using UnityEngine;
using System.Collections;
using TMPro;

public class BatteryPickUp : MonoBehaviour, IPickup
{
    [SerializeField] private MeshRenderer childMeshRenderer;
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;

    [SerializeField] float restoreAngle = 90f;
    [SerializeField] float addIntensity = 1f;
    [SerializeField] float addVolume = 0.1f;

    [SerializeField] FlashLightController flashLight;
    private TextMeshProUGUI messageText; // Assign this in Inspector!
    private GameObject messageObject;
    private GameObject flashlight;
    private AudioSource audioSFX;
    private GameObject sfxObject;

    private Material outlineMaterial;
    private Coroutine fadeCoroutine;
    private Coroutine messageCoroutine;

    void Start()
    {
        sfxObject = GameObject.FindGameObjectWithTag("BasicPickUpSFX");
        audioSFX = sfxObject.GetComponent<AudioSource>();
        outlineMaterial = childMeshRenderer.materials[1];
        messageObject = GameObject.FindGameObjectWithTag("AmmoCheck");
        messageText = messageObject.GetComponent<TextMeshProUGUI>();
        if (messageText != null)
            messageText.text = ""; // Clear at start
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
        flashlight = GameObject.FindGameObjectWithTag("Flashlight");
        flashLight = flashlight.GetComponent<FlashLightController>();

        if(flashLight.enabled == false)
        {
            ShowMessage("Need FlashLight");
            return;
        }

        // Check if full before restoring
        if (flashLight.IsAngleFull() && flashLight.IsIntensityFull() && flashLight.IsVolumeFull())
        {
            ShowMessage("Flashlight is fully charged!");
            return;
        }

        audioSFX.Stop();
        audioSFX.Play();

        if (!flashLight.IsAngleFull())
        {
            flashLight.RestoreLightAngle(restoreAngle);
        }
        if (!flashLight.IsIntensityFull())
        {
            flashLight.RestoreLightIntensity(addIntensity);
        }
        if (!flashLight.IsVolumeFull())
        {
            flashLight.RestoreVolumeIntensity(addVolume);
        }

        Destroy(gameObject);
    }

    private void ShowMessage(string message)
    {
        if (messageText == null) return;

        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        messageText.text = message;
        messageText.alpha = 1f;  // reset alpha to fully visible

        messageCoroutine = StartCoroutine(FadeOutMessage(2f));
    }

    private IEnumerator FadeOutMessage(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            messageText.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }
        messageText.text = "";
    }
}
