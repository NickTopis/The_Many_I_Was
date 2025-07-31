using UnityEngine;

public class PlayerComponentManager : MonoBehaviour
{
    private GameObject playerObject;

    private FirstPersonController firstPersonController;
    private DeathHandler deathHandler;
    private PlayerHealth playerHealth;
    private Ammo ammo;
    private PlayerRaycaster playerRaycaster;
    private SurfaceDetection surfaceDetection;
    private DisplayDamage displayDamage;
    private Animator playerAnimator;

    void Start()
    {
        // Find the player in the scene by tag
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            Debug.LogError("Player not found in the scene! Make sure the Player object is tagged 'Player'.");
            return;
        }

        // Automatically find all components on the scene instance
        firstPersonController = playerObject.GetComponent<FirstPersonController>();
        deathHandler = playerObject.GetComponent<DeathHandler>();
        playerHealth = playerObject.GetComponent<PlayerHealth>();
        ammo = playerObject.GetComponent<Ammo>();
        playerRaycaster = playerObject.GetComponent<PlayerRaycaster>();
        surfaceDetection = playerObject.GetComponent<SurfaceDetection>();
        displayDamage = playerObject.GetComponent<DisplayDamage>();
        playerAnimator = playerObject.GetComponent<Animator>();

        // Disable player controls and enable Animator
        DisablePlayerComponents();
        EnableAnimator();
    }

    public void DisablePlayerComponents()
    {
        if (firstPersonController != null) firstPersonController.enabled = false;
        if (deathHandler != null) deathHandler.enabled = false;
        if (playerHealth != null) playerHealth.enabled = false;
        if (ammo != null) ammo.enabled = false;
        if (playerRaycaster != null) playerRaycaster.enabled = false;
        if (surfaceDetection != null) surfaceDetection.enabled = false;
        if (displayDamage != null) displayDamage.enabled = false;

        Debug.Log("Player components disabled.");
    }

    public void EnableAnimator()
    {
        if (playerAnimator != null)
        {
            playerAnimator.enabled = true;
            Debug.Log("Player animator enabled.");
        }
    }

    public void EnablePlayerComponents()
    {
        if (firstPersonController != null) firstPersonController.enabled = true;
        if (deathHandler != null) deathHandler.enabled = true;
        if (playerHealth != null) playerHealth.enabled = true;
        if (ammo != null) ammo.enabled = true;
        if (playerRaycaster != null) playerRaycaster.enabled = true;
        if (surfaceDetection != null) surfaceDetection.enabled = true;
        if (displayDamage != null) displayDamage.enabled = true;

        Debug.Log("Player components re-enabled.");
    }
}
