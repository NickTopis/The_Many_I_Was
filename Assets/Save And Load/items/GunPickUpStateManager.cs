using UnityEngine;

public class GunPickUpStateManager : MonoBehaviour
{
    [Header("Pickup Reference")]
    [SerializeField] GameObject gunPickupInScene;

    [Header("Gun on Player")]
    [SerializeField] GameObject objectOnPlayer; // the gun model object
    [SerializeField] MeshRenderer weaponMeshRenderer;
    [SerializeField] Weapon weaponScript;
    [SerializeField] GameObject textObj;
    [SerializeField] GameObject molotovOnPlayer;

    private bool gunPickedUp = false;

    public void SetGunPickedUp(bool pickedUp)
    {
        gunPickedUp = pickedUp;
    }

    public bool IsGunPickedUp()
    {
        return gunPickedUp;
    }

    public void ApplyGunState()
    {
        if (gunPickedUp)
        {
            if (gunPickupInScene != null)
                Destroy(gunPickupInScene);

            if (objectOnPlayer != null)
                objectOnPlayer.SetActive(true);

            if (weaponMeshRenderer != null)
                weaponMeshRenderer.enabled = true;

            if (weaponScript != null)
                weaponScript.enabled = true;

            if (textObj != null)
                textObj.SetActive(true);
            if(molotovOnPlayer != null)
            {
                molotovOnPlayer.SetActive(false);
            }
        }
        else
        {
            if (gunPickupInScene != null)
                gunPickupInScene.SetActive(true);

            if (objectOnPlayer != null)
                objectOnPlayer.SetActive(false);

            if (weaponMeshRenderer != null)
                weaponMeshRenderer.enabled = false;

            if (weaponScript != null)
                weaponScript.enabled = false;

            if (textObj != null)
                textObj.SetActive(false);
        }
    }
}
