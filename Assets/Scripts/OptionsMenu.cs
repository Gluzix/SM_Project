using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static float InGameVolume = 1f;
    public static string DifficultyLevel = "Medium";

    void Start()
    {
        Slider volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        volumeSlider.value = InGameVolume;
        volumeSlider.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ((int)(InGameVolume * 100)).ToString() + "%";
        GameObject.Find("DifficultyButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = DifficultyLevel;
    }

    public void ChangeDifficultyLevel()
    {
        if( DifficultyLevel == "Easy" )
        {
            DifficultyLevel = "Medium";
        }
        else if (DifficultyLevel == "Medium")
        {
            DifficultyLevel = "Hard";
        }
        else if( DifficultyLevel == "Hard" )
        {
            DifficultyLevel = "Unbeatable";
        }
        else
        {
            DifficultyLevel = "Easy";
        }

        Difficulty.SetDifficulty(DifficultyLevel);
        GameObject.Find("DifficultyButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = DifficultyLevel;
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ChangeVolumeLevel()
    {
        Slider volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        InGameVolume = volumeSlider.value;
        volumeSlider.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ((int)(InGameVolume * 100)).ToString() + "%";
        AudioSource gameAudio = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        gameAudio.volume = InGameVolume;
    }
}
