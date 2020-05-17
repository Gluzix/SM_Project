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
    GameObject carName;
    GameObject carBitmap;
    GameObject trackName;
    GameObject trackBitmap;
    public static Sprite currentStaticSprite;

    void Start()
    {
        carBitmap = GameObject.Find("CarBitmap");
        carName = GameObject.Find("CarName");
        trackBitmap = GameObject.Find("TrackBitmap");
        trackName = GameObject.Find("TrackName");
        trackSprites = Resources.LoadAll<Sprite>("RaceTracks");
        carSprites = Resources.LoadAll<Sprite>("Cars");

        currentStaticSprite = carSprites[0];

        trackName.GetComponent<TMPro.TextMeshProUGUI>().text = trackSprites[currentTrackIndex].name;
        carName.GetComponent<TMPro.TextMeshProUGUI>().text = carSprites[currentCarIndex].name;
        carBitmap.GetComponent<Image>().sprite = carSprites[currentCarIndex];
        trackBitmap.GetComponent<Image>().sprite = trackSprites[currentTrackIndex];
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
        trackName.GetComponent<TMPro.TextMeshProUGUI>().text = trackSprites[currentTrackIndex].name;
        trackBitmap.GetComponent<Image>().sprite = trackSprites[currentTrackIndex];
    }

    private void ChangeCar()
    {
        carName.GetComponent<TMPro.TextMeshProUGUI>().text = carSprites[currentCarIndex].name;
        carBitmap.GetComponent<Image>().sprite = carSprites[currentCarIndex];
        currentStaticSprite = carSprites[currentCarIndex];
    }
}
