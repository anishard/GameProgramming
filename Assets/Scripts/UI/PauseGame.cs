using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject mainUI;
    private Button[] UIButtons;
    // Start is called before the first frame update
    void Start()
    {
        UIButtons = mainUI.GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // void ClickPauseGameButton() {
    //     // if (gameIsPaused) {
    //     //     Resume();
    //     // }
    //     // else {
    //         Pause();
    //     // }
    // }

    // void ClickResumeGameButton()

    public void Resume() { // when resume button is clicked
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

        // make all UI buttons interactable
        foreach (Button btn in UIButtons) {
            btn.interactable = true;
        }
    }

    public void Pause() { // when pause button is clicked
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        // make all UI buttons not interactable
        foreach (Button btn in UIButtons) {
            btn.interactable = false;
        }
    }

    public void Quit() { // when quit button is clicked, go back to starting menu
        Debug.Log("quitting game...");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        //TODO: switch scene to opening menu here
        //SceneManager.LoadScene("main menu scene here");
    }

}
