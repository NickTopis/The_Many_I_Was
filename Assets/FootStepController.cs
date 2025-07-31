using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootStepController : MonoBehaviour
{
    [Range(0f, 20f)]
    public float frequency = 10.0f;

    public UnityEvent onFootStep;

    float Sin;

    bool isTriggred = false;

    void Update()
    {
        float inpitMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if(inpitMagnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                frequency = 20.0f;
            }
            else
            {
                frequency = 10.0f;
            }
            StartFootSteps();
        }
        
    }

    private void StartFootSteps()
    {
        Sin = Mathf.Sin(Time.time * frequency);

        if (Sin > 0.97f && isTriggred == false)
        {
            isTriggred = true;
            onFootStep.Invoke();
        }
        else if(isTriggred == true && Sin < 0.97f)
        {
            isTriggred = false;
        }
    }
}
