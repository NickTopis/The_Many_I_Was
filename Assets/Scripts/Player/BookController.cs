using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class BookController : MonoBehaviour
{
    public bool gotbook = false;
    [SerializeField] Transform player;
    [SerializeField] GameObject noteBook;
    [SerializeField] Animator animator;
    [SerializeField] GameObject stamina;
    [SerializeField] GameObject ammoPointer;
    [SerializeField] GameObject weapons;
    [SerializeField] GameObject flashLight;
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] AudioSource openBook;
    [SerializeField] AudioSource closeBook;
    [SerializeField] Animator indicatorAnimator;
    [SerializeField] Volume volume;
    [SerializeField] float transitionDuration = 1.5f;
    [SerializeField] AudioSource stepSounds;

    private DepthOfField dof;
    bool animatorclosed = false;
    private Rigidbody rb;

    public bool bookOpened = false;
    void Start()
    {
        firstPersonController = player.GetComponent<FirstPersonController>();
        rb = player.GetComponent<Rigidbody>();
        openBook.enabled = false;
        closeBook.enabled = false;

        if (!gotbook)
        {
            noteBook.SetActive(false);
        }
        else
        {
            noteBook.SetActive(true);
        }

        if (!volume.profile.TryGet(out dof))
        {
            Debug.LogWarning("Depth of Field not found in Volume Profile.");
        }
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (gotbook)
                {
                    noteBook.SetActive(true);
                    if (!bookOpened)
                    {
                        stepSounds.enabled = false;
                        firstPersonController.fov = 60f;
                        firstPersonController.enableHeadBob = false;
                        firstPersonController.cameraCanMove = false;
                        firstPersonController.playerCanMove = false;
                        stamina.SetActive(false);
                        ammoPointer.SetActive(false);
                        weapons.SetActive(false);
                        flashLight.SetActive(false);
                        //firstPersonController.enabled = false;
                        openBook.enabled = true;
                        closeBook.enabled = false;
                        bookOpened = true;
                        animator.SetTrigger("ShowBook");
                        rb.constraints = RigidbodyConstraints.FreezePosition;
                        FocusNear();
                        if (animatorclosed == false)
                        {
                            indicatorAnimator.SetTrigger("close");
                        }
                    }
                    else
                    {
                        stepSounds.enabled = true;
                        rb.constraints = RigidbodyConstraints.FreezeRotation;
                        stamina.SetActive(true);
                        ammoPointer.SetActive(true);
                        weapons.SetActive(true);
                        flashLight.SetActive(true);
                        firstPersonController.enableHeadBob = true;
                        firstPersonController.cameraCanMove = true;
                        firstPersonController.playerCanMove = true;
                        openBook.enabled = false;
                        closeBook.enabled = true;
                        bookOpened = false;
                        FocusFar();
                        animator.SetTrigger("HideBook");
                    }

                }
            }
        }
    }

    public void FocusNear()
    {
        if (dof == null) return;

        // Enable DOF and focus distance
        dof.mode.overrideState = true;
        dof.focusDistance.overrideState = true;

        StopAllCoroutines();
        StartCoroutine(AnimateFocusDistance(dof.focusDistance.value, 0.45f, false));
    }

    public void FocusFar()
    {
        if (dof == null) return;

        StopAllCoroutines();
        StartCoroutine(AnimateFocusDistance(dof.focusDistance.value, 10f, true));
    }

    private IEnumerator AnimateFocusDistance(float from, float to, bool disableAfter)
    {
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            dof.focusDistance.value = Mathf.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dof.focusDistance.value = to;

        if (disableAfter)
        {
            dof.focusDistance.overrideState = false;
            dof.mode.overrideState = false;
        }
    }
}
