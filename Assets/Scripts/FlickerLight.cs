using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]

public class FlickerLight : MonoBehaviour
{
    private Light lightToFlicker;
    [SerializeField, Range(0f, 3f)] private float minIntensity = 0.05f;
    [SerializeField, Range(0f, 20f)] private float maxIntensity = 12f;
    [SerializeField, Min(0f)] private float timeBetweenIntensity = 0.1f;

    private float currentTimer;

    private void Awake()
    {
        if(lightToFlicker == null)
        {
            lightToFlicker = GetComponent<Light>();
        }

        ValidateIntensityBounds();
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
        if(!(currentTimer >= timeBetweenIntensity)) { return; }
        
        lightToFlicker.intensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
        currentTimer = 0;
    }

    private void ValidateIntensityBounds()
    {
        if(!(minIntensity > maxIntensity))
        {
            return;
        }

        Debug.LogWarning("Min intesity is greater than max intensity, Swapping Values!");
        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }
}
