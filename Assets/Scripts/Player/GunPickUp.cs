using UnityEngine;
using System.Collections;

public class GunPickUp : MonoBehaviour, IPickup
{
    [SerializeField] private MeshRenderer childMeshRenderer; 
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;

    [SerializeField] GameObject objectOnPlayer;
    private AudioSource audioSFX;
    [SerializeField] GameObject textObj;
    [SerializeField] MeshRenderer weaponMeshRenderer;
    [SerializeField] Weapon weapon;

    private Material outlineMaterial;
    private Coroutine fadeCoroutine;
    private GameObject sfxObject;

    void Start()
    {
        sfxObject = GameObject.FindGameObjectWithTag("BasicPickUpSFX");
        audioSFX = sfxObject.GetComponent<AudioSource>();
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
        weapon.enabled = true;
        weaponMeshRenderer.enabled = true;
        textObj.SetActive(true);
        audioSFX.Stop();
        audioSFX.Play();
        objectOnPlayer.SetActive(true);

        GunPickUpStateManager manager = FindObjectOfType<GunPickUpStateManager>();
        if (manager != null)
        {
            manager.SetGunPickedUp(true);
            manager.ApplyGunState();
        }

        Destroy(gameObject);
    }
}
