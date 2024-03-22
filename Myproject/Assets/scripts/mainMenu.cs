using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public static mainMenu Instance { get; set; }

    [SerializeField] GameObject menuScreen; 
    [SerializeField] CinemachineBrain cinemachineBrain;
    bool onMenu = false;
    float timeScaleBeforeMenu = 1f;
    bool cursorWasVisible;
    CursorLockMode cursorWaslocked;
    public string gameDifficulty;

    [SerializeField] GameObject minimapCanvas;
    void Start()
    {
        //Menu();
        menuScreen.SetActive(false);


    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Menu()
    {
        onMenu = true;

        cursorWasVisible = Cursor.visible;
        cursorWaslocked = Cursor.lockState;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        menuScreen.SetActive(onMenu);

        cinemachineBrain.enabled = !onMenu; //disable camera control with mouse movement

        timeScaleBeforeMenu = Time.timeScale; //store value

        Time.timeScale = 0f;
        minimapCanvas.SetActive(false);
    }

    public void closeMenu()
    {
        onMenu = false;

        Cursor.visible = cursorWasVisible;
        Cursor.lockState = cursorWaslocked;

        menuScreen.SetActive(onMenu);

        cinemachineBrain.enabled = !onMenu; //enable camera control with mouse movement

        Time.timeScale = timeScaleBeforeMenu;
        minimapCanvas.SetActive(true);
    }

    public void PlayEasyGame()
    {
        gameDifficulty = "Easy";
        Debug.Log(gameDifficulty);
        closeMenu();
    }

    public void PlayMediumGame()
    {
        gameDifficulty = "Medium";
        Debug.Log(gameDifficulty);
        closeMenu();
    }

    public void PlayHardGame()
    {
        gameDifficulty = "Hard";
        Debug.Log(gameDifficulty);
        closeMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
        closeMenu();
    }

}
