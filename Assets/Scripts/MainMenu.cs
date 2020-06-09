using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public void Start()
    {
        if (!GlobalVars.ifCarsAreLoaded)
        {
            PlayerData.LoadGame();
            GlobalVars.ifCarsAreLoaded = true;
        }
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
