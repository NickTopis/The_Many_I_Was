using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathHandler : MonoBehaviour
{
    [SerializeField] Animator closingAnimator;
    [SerializeField] Pausemenu pausemenu;

    [SerializeField] GameObject weapons;
    [SerializeField] GameObject ammoPointer;
    [SerializeField] GameObject crossHair;
    [SerializeField] GameObject flaslight;

    public void HandleDeath()
    {
        closingAnimator.SetTrigger("closescene");
        pausemenu.enabled = false;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        GetComponent<FirstPersonController>().enabled = false;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        weapons.SetActive(false);
        ammoPointer.SetActive(false);
        crossHair.SetActive(false);
        flaslight.SetActive(false);

        StartCoroutine(WaitForAnimationEnd());
    }

    private IEnumerator WaitForAnimationEnd()
    {
        while (!closingAnimator.GetCurrentAnimatorStateInfo(0).IsName("closing")) // replace with your state name
        {
            yield return null;
        }

        while (closingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        OnSceneClosed();
    }

    private void OnSceneClosed()
    {
        Debug.Log("Scene close animation finished.");
        SceneManager.LoadScene(2);
        Debug.Log("Reload");
    }
}
