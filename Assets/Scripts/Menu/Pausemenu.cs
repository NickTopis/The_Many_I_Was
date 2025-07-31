using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Pausemenu : MonoBehaviour
{
    [SerializeField] FirstPersonController controller;
    [SerializeField] GameObject weapons;
    [SerializeField] GameObject ammoPointer;
    [SerializeField] GameObject crossHair;

    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject optionsMenuCanvas;

    [SerializeField] BookController bookController;

    public GameObject pauseMenuUI;
    public static bool GameIsPause = false;

    public AudioListener audioListener;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPause == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (bookController.bookOpened == false)
        {
            weapons.SetActive(true);
            ammoPointer.SetActive(true);
            crossHair.SetActive(true);

            pauseMenuCanvas.SetActive(true);
            optionsMenuCanvas.SetActive(false);

            controller.enabled = true;
        }
        bookController.enabled = true;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    public void Pause()
    {
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        weapons.SetActive(false);
        ammoPointer.SetActive(false);
        crossHair.SetActive(false);

        controller.enabled = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        bookController.enabled = false;
        GameIsPause = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        Debug.Log("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
