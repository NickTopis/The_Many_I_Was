using System;
using UnityEngine;
using System.Collections;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AmmoPickUp : MonoBehaviour, IPickup
{
    [SerializeField] private MeshRenderer childMeshRenderer; // The child with the materials
    [SerializeField] private string opacityProperty = "_Opacity";
    [SerializeField] private float outlineShowDuration = 0.1f;

    [SerializeField] int ammoAmount = 10;
    [SerializeField] AmmoType ammoType;
    private AudioSource audioSFX;
    private GameObject sfxObject;

    [Header("Auto ID")]
    [SerializeField] private string uniqueID;

    private Material outlineMaterial;
    private Coroutine fadeCoroutine;

    private GameObject messageObj;
    private TextMeshProUGUI messageText;
    private string message;
    private Coroutine messageCoroutine;

    void Start()
    {
        messageObj = GameObject.FindGameObjectWithTag("AmmoCheck");
        messageText = messageObj.GetComponent<TextMeshProUGUI>();
        if (messageText != null)
            messageText.text = "";
        if (ammoType == AmmoType.Bullets)
        {
            sfxObject = GameObject.FindGameObjectWithTag("BasicPickUpSFX");
            audioSFX = sfxObject.GetComponent<AudioSource>();
        }
        if (ammoType == AmmoType.Molotov)
        {
            sfxObject = GameObject.FindGameObjectWithTag("MolotovPickUpSFX");
            audioSFX = sfxObject.GetComponent<AudioSource>();
        }
        outlineMaterial = childMeshRenderer.materials[1];

        // If already picked up, remove from scene
        if (PickUpManager.Instance != null && PickUpManager.Instance.IsPickedUp(uniqueID))
        {
            this.gameObject.SetActive(false);
        }
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
        var ammo = FindObjectOfType<Ammo>();
        if (ammo == null) return;

        if (ammo.IsAmmoFull(ammoType))
        {
            if (ammoType == AmmoType.Bullets)
            {
                message = "Clipper is full!";
            }
            if (ammoType == AmmoType.Molotov)
            {
                message = "cant hold more!";
            }
            ShowMessage(message);
            return;
        }

        audioSFX.Stop();
        audioSFX.Play();

        ammo.IncreaseCurrentAmmo(ammoType, ammoAmount);

        if (PickUpManager.Instance != null)
        {
            PickUpManager.Instance.MarkPickedUp(uniqueID);
        }

        this.gameObject.SetActive(false);
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueID) || !IsUnique(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
    }

    private bool IsUnique(string id)
    {
        AmmoPickUp[] all = FindObjectsOfType<AmmoPickUp>(true);
        foreach (var pickup in all)
        {
            if (pickup != this && pickup.uniqueID == id)
                return false;
        }
        return true;
    }
#endif

    public string GetID()
    {
        return uniqueID;
    }
}
