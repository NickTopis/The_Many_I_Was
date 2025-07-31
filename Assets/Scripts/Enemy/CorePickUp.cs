using System.Collections;
using UnityEngine;
using TMPro;

public class CorePickUp : MonoBehaviour, IPickup
{
    [SerializeField] private MeshRenderer childMeshRenderer; // The child with the materials
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;

    private AudioSource audioSFX;
    private GameObject sfxObject;
    [SerializeField] TextMeshProUGUI coreTextUI;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;

    private Material outlineMaterial;
    private Coroutine fadeCoroutine;

    private void OnEnable()
    {
        outlineMaterial = childMeshRenderer.materials[1];
        DisplayAmmount();
    }

    public void ShowPrompt(bool show)
    {
        sfxObject = GameObject.FindGameObjectWithTag("CorePickUpSFX");
        audioSFX = sfxObject.GetComponent<AudioSource>();
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
        ammoSlot.IncreaseCurrentAmmo(ammoType, 1);
        DisplayAmmount();

        audioSFX.Stop();
        audioSFX.Play();

        Destroy(this.gameObject);
    }

    private void DisplayAmmount()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        coreTextUI.text = ammoType + " : " + currentAmmo.ToString();
    }
}
