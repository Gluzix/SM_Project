using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SelectionMenu : MonoBehaviour
{
    private int currentTrackIndex = 0;
    private int currentCarIndex = 0;
    //Sprite[] carSprites;
    Sprite[] trackSprites;
    GameObject carName;
    GameObject carBitmap;
    GameObject trackName;
    GameObject trackBitmap;
    public static CarListObject carList;
    public static Cars currentCar;
    //public static List<GameObject> carList;

    void Start()
    {
        string json = File.ReadAllText( Application.dataPath + "/cars.json" );
        carList = JsonUtility.FromJson<CarListObject>(json);

        carBitmap = GameObject.Find("CarBitmap");
        carName = GameObject.Find("CarName");
        trackBitmap = GameObject.Find("TrackBitmap");
        trackName = GameObject.Find("TrackName");
        trackSprites = Resources.LoadAll<Sprite>("RaceTracks");
        //carSprites = Resources.LoadAll<Sprite>("Cars");

        //currentStaticSprite = carSprites[0];

        trackName.GetComponent<TMPro.TextMeshProUGUI>().text = trackSprites[currentTrackIndex].name;
        //carName.GetComponent<TMPro.TextMeshProUGUI>().text = carSprites[currentCarIndex].name;
        carBitmap.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cars/" + carList.carList[currentCarIndex].SpriteName);
        carName.GetComponent<TMPro.TextMeshProUGUI>().text = carList.carList[currentCarIndex].Name;
        trackBitmap.GetComponent<Image>().sprite = trackSprites[currentTrackIndex];
        currentCar = carList.carList[currentCarIndex];
        Debug.Log(carList.carList[currentCarIndex].SpriteName);
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
        if (currentCarIndex > carList.carList.Count - 1)
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
            currentCarIndex = carList.carList.Count - 1;
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
        carBitmap.GetComponent<Image>().sprite = Resources.Load<Sprite>( "Cars/"+carList.carList[currentCarIndex].SpriteName );
        carName.GetComponent<TMPro.TextMeshProUGUI>().text = carList.carList[currentCarIndex].Name;
        currentCar = carList.carList[currentCarIndex];
    }
}
