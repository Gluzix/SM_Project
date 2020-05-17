using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RaceMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        PlayerController.gameIsPaused = false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("SelectionMenu");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

