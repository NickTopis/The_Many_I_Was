using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotation : MonoBehaviour
{
    private Transform camTransform;
    private Vector3 offset = new Vector3(0, 180, 0);
    void Start()
    {
        camTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camTransform);
        transform.Rotate(offset);
    }
}
