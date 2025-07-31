using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startplayersComppnent : MonoBehaviour
{
    [SerializeField] StartingSequence startingSequence;
    public bool includeSequence;

    [Header("Player Components")]
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] DeathHandler deathHandler;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerRaycaster playerRaycaster;
    [SerializeField] SurfaceDetection surfaceDetection;
    [SerializeField] DisplayDamage displayDamage;
    [SerializeField] Animator animatorOnPlayer;
    [SerializeField] GameObject uI;
    void Awake()
    {
        includeSequence = startingSequence.startingSequence;

        if (includeSequence)
        {
            firstPersonController.enabled = false;
            deathHandler.enabled = false;
            playerHealth.enabled = false;
            playerRaycaster.enabled = false;
            surfaceDetection.enabled = false;
            displayDamage.enabled = false;
            animatorOnPlayer.enabled = true;
            uI.SetActive(false);
        }
        else
        {
            firstPersonController.enabled = true;
            deathHandler.enabled = true;
            playerHealth.enabled = true;
            playerRaycaster.enabled = true;
            surfaceDetection.enabled = true;
            displayDamage.enabled = true;
            animatorOnPlayer.enabled = false;
            uI.SetActive(true);
        }
    }
}
