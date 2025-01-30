using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionMenu;

    public Button buttonSelected;

    public static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                ResumeGameFromOM();
            }
            else
            {
                PauseGame();
                SelectButton();
            }
        }
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    //public void ResumeGameFromLS()
    //{
    //    Cursor.visible = true;
    //    pauseMenu.SetActive(false);
    //    Time.timeScale = 1f;
    //    isPaused = false;
    //}
    public void ShowOMMenu()
    {
        Cursor.visible = true;
        optionMenu.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGameFromOM()
    {
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    //public void GoLS()
    //{
    //    Time.timeScale = 1f;
    //    SceneManager.LoadScene("LevelSelector");
    //}

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SelectButton()
    {
        buttonSelected.Select();
    }
}
