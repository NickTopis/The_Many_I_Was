using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BunkerDoorControll : MonoBehaviour, IPickup
{
    [SerializeField] int ammoOnDoor = 0;
    [SerializeField] Ammo ammoSlot;
    /*[SerializeField]*/ private AmmoType ammoType;
    [SerializeField] GameObject text;
    [SerializeField] TextMeshPro bunkerDoorTextUI;
    private AudioSource audioSFX;
    private GameObject sfxObject;
    [SerializeField] Animator bunkerDoorAnimator;
    [SerializeField] TextMeshProUGUI ammoText;

    void Start()
    {
        ammoType = AmmoType.Cores;
        sfxObject = GameObject.FindGameObjectWithTag("CorePickUpSFX");
        audioSFX = sfxObject.GetComponent<AudioSource>();
        DisplayAmmount();
        text.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        text.SetActive(show);
    }

    public void PickUp()
    {
        /*Ammo ammoInv = ammoSlot;
        if (ammoInv == null) return;*/

        // Check if player has ammo
        if (ammoSlot.GetCurrentAmmo(ammoType) <= 0)
            return;  // Player has no ammo, do nothing

        // Consume ammo and deposit
        audioSFX.Stop();
        audioSFX.Play();
        ammoSlot.ReduceCurrentAmmo(ammoType);
        ammoOnDoor++;
        DisplayAmmount();
        DisplayAmmo();

        // Open the door if requirement met
        if (ammoOnDoor >= 5)
        {
            bunkerDoorAnimator.SetTrigger("OpenDoor");
            Destroy(gameObject);
        }
    }

    private void DisplayAmmount()
    {       
        bunkerDoorTextUI.text = "Cores : " + ammoOnDoor.ToString() + " / 5";
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = ammoType + " : " + currentAmmo.ToString();
    }
}
