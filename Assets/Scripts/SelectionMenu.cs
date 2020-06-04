using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SelectionMenu : MonoBehaviour
{
    static public int currentTrackIndex = 0;
    static public int trackAmount = 0;
    private int currentCarIndex = 0;
    private float bestSpeed = 0f;
    private float bestHandling = 0f;
    private float bestBraking = 0f;
    Sprite[] trackSprites;
    GameObject carName;
    GameObject carBitmap;
    GameObject carPrice;
    GameObject trackName;
    GameObject trackBitmap;
    GameObject playButton;
    GameObject buyButton;
    GameObject cashText;
    GameObject LockedText;
    public static CarListObject carList;
    public static Cars currentCar;
    bool bIfMapIsOk;
    bool bIfCarIsOk;

    void Start()
    {
        string json = File.ReadAllText( Application.dataPath + "/cars.json" );
        carList = JsonUtility.FromJson<CarListObject>(json);
        FindBestCarPerformance();

        if ( !GlobalVars.ifCarsAreLoaded )
        {
            PlayerData.LoadGame();
            GlobalVars.ifCarsAreLoaded = true;
        }

        carBitmap = GameObject.Find("CarBitmap");
        carName = GameObject.Find("CarName");
        carPrice = GameObject.Find("CarPrice");
        trackBitmap = GameObject.Find("TrackBitmap");
        trackName = GameObject.Find("TrackName");
        cashText = GameObject.Find("PlayerCashText");
        trackSprites = Resources.LoadAll<Sprite>("RaceTracks");
        playButton = GameObject.Find("PlayButton");
        buyButton = GameObject.Find("BuyButton");
        LockedText = GameObject.Find("Locked");
        trackName.GetComponent<TMPro.TextMeshProUGUI>().text = trackSprites[currentTrackIndex].name;
        carBitmap.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cars/" + carList.carList[currentCarIndex].SpriteName);
        carName.GetComponent<TMPro.TextMeshProUGUI>().text = carList.carList[currentCarIndex].Name;
        carPrice.GetComponent<TMPro.TextMeshProUGUI>().text = "Price: " + carList.carList[currentCarIndex].points.ToString();
        trackBitmap.GetComponent<Image>().sprite = trackSprites[currentTrackIndex];
        cashText.GetComponent<TMPro.TextMeshProUGUI>().text = "Cash: " + PlayerData.cash;
        currentCar = carList.carList[currentCarIndex];
        Difficulty.SetDifficulty( OptionsMenu.DifficultyLevel );
        IfPlayerHasCurrentCar();
        SetCarStatistics();
        trackAmount = trackSprites.Length;

        if (!PlayerData.unlockedLaps.Contains(trackSprites[currentTrackIndex].name))
        {
            Color color = new Color(255, 255, 255, 0);
            trackBitmap.GetComponent<Image>().color = color;
            LockedText.SetActive(true);
            bIfMapIsOk = false;
        }
        else
        {
            Color color = new Color(255, 255, 255, 1);
            trackBitmap.GetComponent<Image>().color = color;
            LockedText.SetActive(false);
            bIfMapIsOk = true;
        }
    }

    private void FindBestCarPerformance()
    {
        bestSpeed = carList.carList[0].speedForce;
        bestHandling = carList.carList[0].torqueForce;
        bestBraking = carList.carList[0].brakeForce;
        for (int i=0; i< carList.carList.Count; i++ )
        {
            if(carList.carList[i].speedForce > bestSpeed)
            {
                bestSpeed = carList.carList[i].speedForce;
            }

            if(carList.carList[i].torqueForce > bestHandling)
            {
                bestHandling = carList.carList[i].torqueForce;
            }

            if (Mathf.Abs(carList.carList[i].brakeForce) > Mathf.Abs(bestBraking) )
            {
                bestBraking = carList.carList[i].brakeForce;
            }
        }
    }

    private void SetCarStatistics()
    {
        GameObject speedStatChild = GameObject.Find("SpeedStatChild");
        GameObject handlingStatChild = GameObject.Find("HandlingStatChild");
        GameObject brakingStatChild = GameObject.Find("BrakingStatChild");

        float percentage = carList.carList[currentCarIndex].speedForce / bestSpeed;
        Vector3 localScale = speedStatChild.GetComponent<RectTransform>().localScale;
        speedStatChild.GetComponent<RectTransform>().localScale = new Vector3( percentage, localScale.y, localScale.z );

        percentage = carList.carList[currentCarIndex].torqueForce / bestHandling;
        localScale = handlingStatChild.GetComponent<RectTransform>().localScale;
        handlingStatChild.GetComponent<RectTransform>().localScale = new Vector3(percentage, localScale.y, localScale.z);

        percentage = Mathf.Abs(carList.carList[currentCarIndex].brakeForce) / Mathf.Abs(bestBraking);
        localScale = brakingStatChild.GetComponent<RectTransform>().localScale;
        brakingStatChild.GetComponent<RectTransform>().localScale = new Vector3(percentage, localScale.y, localScale.z);

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Track_" + (currentTrackIndex + 1).ToString());
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void BuyCar()
    {
        if ( carList.carList[currentCarIndex].points <= PlayerData.cash )
        {
            PlayerData.cash -= carList.carList[currentCarIndex].points;
            PlayerData.playerCars.Add(carList.carList[currentCarIndex]);
            cashText.GetComponent<TMPro.TextMeshProUGUI>().text = "Cash: " + PlayerData.cash;
            IfPlayerHasCurrentCar();
            PlayerData.SaveGame();
        }
    }

    private void IfPlayerHasCurrentCar()
    {
        bool bIfCarFound = false;
        for( int i=0; i<PlayerData.playerCars.Count; i++ )
        {
            if (PlayerData.playerCars[i].SpriteName == carList.carList[currentCarIndex].SpriteName)
            {
                bIfCarFound = true;
                break;
            }
        }

        if ( bIfCarFound )
        {
            bIfCarIsOk = true;
            buyButton.SetActive(false);
        }
        else
        {
            bIfCarIsOk = false;
            buyButton.SetActive(true);
        }

        UnlockPlayButton();
    }

    private void UnlockPlayButton()
    {
        if (bIfCarIsOk && bIfMapIsOk)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
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
        if( !PlayerData.unlockedLaps.Contains(trackSprites[currentTrackIndex].name) )
        {
            Color color = new Color(255, 255, 255, 0);
            trackBitmap.GetComponent<Image>().color = color;
            LockedText.SetActive(true);
            bIfMapIsOk = false;
        }
        else
        {
            Color color = new Color(255, 255, 255, 1);
            trackBitmap.GetComponent<Image>().color = color;
            LockedText.SetActive(false);
            bIfMapIsOk = true;
        }
        UnlockPlayButton();
    }

    private void ChangeCar()
    {
        carBitmap.GetComponent<Image>().sprite = Resources.Load<Sprite>( "Cars/"+carList.carList[currentCarIndex].SpriteName );
        carName.GetComponent<TMPro.TextMeshProUGUI>().text = carList.carList[currentCarIndex].Name;
        carPrice.GetComponent<TMPro.TextMeshProUGUI>().text = "Price: " + carList.carList[currentCarIndex].points.ToString();
        currentCar = carList.carList[currentCarIndex];
        SetCarStatistics();
        IfPlayerHasCurrentCar();
    }
}
