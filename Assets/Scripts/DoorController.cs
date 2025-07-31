using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour, IPickup
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject text;
    [SerializeField] AudioSource audioFX;
    [SerializeField] TextMeshPro tmText1;
    [SerializeField] TextMeshPro tmText2;

    bool isOpen = false;

    private void Start()
    {
        text.SetActive(false);
        tmText1.text = "Open";
        tmText2.text = "Open";
    }

    public void ShowPrompt(bool show)
    {
        text.SetActive(show);
    }

    public void PickUp()
    {


        audioFX.Play();

        if (!isOpen)
        {
            animator.SetTrigger("Open");
            tmText1.text = "Close";
            tmText2.text = "Close";
            isOpen = true;
        }
        else
        {
            animator.SetTrigger("Close");
            tmText1.text = "Open";
            tmText2.text = "Open";
            isOpen = false;
        }
    }
}
