using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class StartingSequence : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject tvOff;
    [SerializeField] GameObject tvOn;

    [SerializeField] GameObject ui;
    [SerializeField] public bool startingSequence = true;
    [SerializeField] Pausemenu pausemenu;
    [SerializeField] SaveAndLoadFunctions saveAndLoadFunctions;

    private GameObject playerObject;
    private FirstPersonController firstPersonController;
    private DeathHandler deathHandler;
    private PlayerHealth playerHealth;
    private Ammo ammo;
    private PlayerRaycaster playerRaycaster;
    private SurfaceDetection surfaceDetection;
    private DisplayDamage displayDamage;
    private Animator animatorOnPlayer;
    [SerializeField] GameObject uI;
    void Start()
    {
        if (startingSequence)
        {
            player.transform.position = new Vector3(-24.845f, 51.309f, 74.962f);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        tvOff.SetActive(true);
        tvOn.SetActive(false);

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
        animatorOnPlayer = playerObject.GetComponent<Animator>();
    }

    public void StartSequence()
    {
        if (startingSequence)
        {

            StartCoroutine(InitializeFirst());
            player.transform.position = new Vector3(-24.845f, 51.309f, 74.962f);
        }
        else
        {         
            EnablePlayerComponents();
            DisableAnimator();
            //player.transform.position = new Vector3(-27.524f, 51.1954f, 74.962f);
        }
    }

    IEnumerator InitializeFirst()
    {
        DisablePlayerComponents();
        EnableAnimator();


        yield return new WaitForSeconds(5f);

        tvOff.SetActive(false);
        tvOn.SetActive(true);

        animatorOnPlayer.SetTrigger("Start");

        AnimatorStateInfo stateInfo = animatorOnPlayer.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("StartingSequence"))
        {
            yield return null;
            stateInfo = animatorOnPlayer.GetCurrentAnimatorStateInfo(0);
        }

        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        ui.SetActive(true);
        pausemenu.enabled = true;
        animatorOnPlayer.enabled = false;
        startingSequence = false;
        saveAndLoadFunctions.SaveGame();
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
        if(ui != null) ui.SetActive(false);

        Debug.Log("Player components disabled.");
    }

    public void EnableAnimator()
    {
        if (animatorOnPlayer != null)
        {
            animatorOnPlayer.enabled = true;
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

    public void DisableAnimator()
    {
        if (animatorOnPlayer != null)
        {
            animatorOnPlayer.enabled = false;
            Debug.Log("Player animator enabled.");
        }
    }
}
