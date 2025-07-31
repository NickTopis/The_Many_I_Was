using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float transitionTime = 1f;

    private string playerSavePath;
    private string enemySavePath;

    [Header("UI References")]
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI continueButtonText;

    private void Awake()
    {
        playerSavePath = Application.persistentDataPath + "/playerdata.dat";
        enemySavePath = Application.persistentDataPath + "/enemy_save.json";

        bool hasSave = File.Exists(playerSavePath) || File.Exists(enemySavePath);
        continueButton.interactable = hasSave;

        if (!hasSave && continueButtonText != null)
        {
            Color c = continueButtonText.color;
            c.a = 0.3f;
            continueButtonText.color = c;

        }
    }
    public void NewGame()
    {
        if (File.Exists(playerSavePath)) { File.Delete(playerSavePath); }
            
        if (File.Exists(enemySavePath)) { File.Delete(enemySavePath); }
            
        Debug.Log("Save files deleted. Starting new game.");

        StartCoroutine(LoadNextScene());
    }
    public void OnContinue()
    {
        if (File.Exists(playerSavePath) || File.Exists(enemySavePath))
        {
            StartCoroutine(LoadNextScene());
        }
        else
        {
            Debug.LogWarning("No save data found.");
        }
    }
    public void QuitGame()
    {
        Debug.Log("Quitting...");
        StartCoroutine(QuitGameRouty());

    }

    IEnumerator LoadNextScene()
    {
        animator.SetTrigger("changeScene");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator QuitGameRouty()
    {
        animator.SetTrigger("changeScene");

        yield return new WaitForSeconds(transitionTime);

        Application.Quit();
    }
}
