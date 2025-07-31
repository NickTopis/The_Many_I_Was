using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDetection : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1.2f;

    [SerializeField] AudioClip grassWalk;
    [SerializeField] AudioClip grassRun;

    [SerializeField] AudioClip stoneWalk;
    [SerializeField] AudioClip stoneRun;

    [SerializeField] FootStepControl footStepControll;

    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            string hitTag = hit.collider.tag;

            if (hitTag == "Grass")
            {
                footStepControll.walkClip = grassWalk;
                footStepControll.runClip = grassRun;
            }
            else if (hitTag == "Stone")
            {
                footStepControll.walkClip = stoneWalk;
                footStepControll.runClip = stoneRun;
            }
            else
            {
                Debug.Log("Hit unknown tagged surface: " + hitTag);
            }
        }
        else
        {
            Debug.Log("No surface hit.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistance);
    }
}
