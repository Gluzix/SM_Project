using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SelectionMenu : MonoBehaviour
{
    private int currentTrackIndex = 0;
    private int currentCarIndex = 0;
    Sprite[] carSprites;
    Sprite[] trackSprites;
    public static Sprite currentStaticSprite;

    void Start()
    {
        trackSprites = Resources.LoadAll<Sprite>("RaceTracks");
        carSprites = Resources.LoadAll<Sprite>("Cars");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Track_" + (currentTrackIndex + 1).ToString());
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void TrackRightClick()
    {
        currentTrackIndex++;
        if (currentTrackIndex > trackSprites.Length - 1)
        {
            currentTrackIndex = 0;
        }
        ChangeTrack();
    }

    public void TrackLeftClick()
    {
        currentTrackIndex--;
        if (currentTrackIndex < 0)
        {
            currentTrackIndex = trackSprites.Length - 1;
        }
        ChangeTrack();
    }

    public void CarRightClick()
    {
        currentCarIndex++;
        if (currentCarIndex > carSprites.Length - 1)
        {
            currentCarIndex = 0;
        }
        ChangeCar();
    }

    public void CarLeftClick()
    {
        currentCarIndex--;
        if (currentCarIndex < 0)
        {
            currentCarIndex = carSprites.Length - 1;
        }
        ChangeCar();
    }

    private void ChangeTrack()
    {
        GameObject track = GameObject.Find("TrackBitmap");
        GameObject name = GameObject.Find("TrackName");
        name.GetComponent<TMPro.TextMeshProUGUI>().text = trackSprites[currentTrackIndex].name;
        track.GetComponent<Image>().sprite = trackSprites[currentTrackIndex];
    }

    private void ChangeCar()
    {
        GameObject car = GameObject.Find("CarBitmap");
        GameObject name = GameObject.Find("CarName");
        name.GetComponent<TMPro.TextMeshProUGUI>().text = carSprites[currentCarIndex].name;
        car.GetComponent<Image>().sprite = carSprites[currentCarIndex];
        currentStaticSprite = carSprites[currentCarIndex];
    }
}
