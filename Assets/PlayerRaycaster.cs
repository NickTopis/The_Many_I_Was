using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float pickupRange = 3f;

    private IPickup currentTarget;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            IPickup pickup = hit.collider.GetComponent<IPickup>();

            if (pickup != null)
            {
                if (currentTarget != pickup)
                {
                    if (currentTarget != null) currentTarget.ShowPrompt(false);
                    currentTarget = pickup;
                }

                pickup.ShowPrompt(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickup.PickUp();
                    currentTarget = null;
                }

                return;
            }
        }

        if (currentTarget != null)
        {
            currentTarget.ShowPrompt(false);
            currentTarget = null;
        }
    }
}