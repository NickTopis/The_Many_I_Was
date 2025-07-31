using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] FlashlightStateManager stateManager;


    [SerializeField] AudioSource audioPlayer;
    [SerializeField] GameObject flashlight;
    [SerializeField] Animator animator;

    [SerializeField] float lightDecay = .05f;
    [SerializeField] float angleDecay = .25f;
    [SerializeField] float minimunAngle = 40f;
    [SerializeField] private float maxVolumeOpacity = 1f; // max battery level (100%)
    [SerializeField] private float maxLightAngle = 90f;    // or your max allowed spotAngle
    [SerializeField] private float maxLightIntensity = 5f; // max intensity you want to allow

    [SerializeField] Light myLight;

    [SerializeField] Material volume;
    [SerializeField] float volumeOpacity = 0.5f;
    [SerializeField] float volumeDecay = .0025f;

    [SerializeField] TextMeshProUGUI batteryText;

    bool lightOn;

    void Start()
    {
        lightOn = flashlight.activeSelf;

        if (stateManager != null)
        {
            volumeOpacity = stateManager.GetVolumeOpacity();
            volume.SetFloat("_Opacity", volumeOpacity);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            audioPlayer.Play();
            
            flashlight.SetActive(!flashlight.activeSelf);
            lightOn = !lightOn;
            myLight.enabled = lightOn;

            if (stateManager != null)
            {
                stateManager.SetLightOn(lightOn);
            }
        }               

        if(lightOn == true)
        {
            DecreaceLightIntensity();
            DecreaceLightAngle();
            DecreaceVolumeIntensity();
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                animator.SetBool("Running", true);
            }
        }
        else
        {
            animator.SetBool("Running", false);
        }

        DisplayBatteryPercent();
    }

    private void DisplayBatteryPercent()
    {
        int currentBattery = (int)(volumeOpacity * 100);
        if (currentBattery < 100)
        {
            batteryText.text = "Battery : " + currentBattery.ToString() + "%";
        }
        else
        {
            batteryText.text = "Battery : 100%";
        }
    }

    public void RestoreLightAngle(float restoreAngle)
    {
        myLight.spotAngle += restoreAngle;
        myLight.spotAngle = Mathf.Min(myLight.spotAngle, maxLightAngle);
    }

    public void RestoreLightIntensity(float intesityAmount)
    {
        myLight.intensity += intesityAmount;
        myLight.intensity = Mathf.Min(myLight.intensity, maxLightIntensity);
    }

    public void RestoreVolumeIntensity(float volumeAmount)
    {
        volumeOpacity += volumeAmount;
        volumeOpacity = Mathf.Min(volumeOpacity, maxVolumeOpacity);
        volume.SetFloat("_Opacity", volumeOpacity);
    }

    private void DecreaceVolumeIntensity()
    {
        if (volumeOpacity <= 0)
        {
            return;
        }

        volumeOpacity -= volumeDecay * Time.deltaTime;
        volume.SetFloat("_Opacity", volumeOpacity);

        if (stateManager != null)
        {
            stateManager.SetVolumeOpacity(volumeOpacity);
        }

    }

    private void DecreaceLightIntensity()
    {

        myLight.intensity -= lightDecay * Time.deltaTime;

    }

    private void DecreaceLightAngle()
    {
        if(myLight.spotAngle <= minimunAngle)
        { 
            return;
        }
        else
        {
            myLight.spotAngle -= angleDecay * Time.deltaTime;
        }        
    }

    public bool IsVolumeFull()
    {
        return volumeOpacity >= maxVolumeOpacity;
    }

    public bool IsAngleFull()
    {
        return myLight.spotAngle >= maxLightAngle;
    }

    public bool IsIntensityFull()
    {
        return myLight.intensity >= maxLightIntensity;
    }

}
