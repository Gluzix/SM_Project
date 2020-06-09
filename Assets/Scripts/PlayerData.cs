using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerData : MonoBehaviour
{
    public static List<Cars> playerCars;
    public static List<string> unlockedLaps;
    public static int cash = 300000;

    public static void SaveGame()
    {
        string destination = Application.persistentDataPath + "/savegame.dat";
        FileStream file;
        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
        PlayerDataSave data = new PlayerDataSave( playerCars, unlockedLaps, cash);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public static void LoadGame()
    {
        playerCars = new List<Cars>();
        unlockedLaps = new List<string>();
        string destination = Application.persistentDataPath + "/savegame.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            if (!unlockedLaps.Contains("track_1"))
            {
                unlockedLaps.Add("track_1");
            }
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        PlayerDataSave data = (PlayerDataSave)bf.Deserialize(file);
        file.Close();

        playerCars = data.playerCars;
        unlockedLaps = data.unlockedLaps;
        cash = data.cash;
        if (!unlockedLaps.Contains("track_1"))
        {
            unlockedLaps.Add("track_1");
        }
    }
}

[System.Serializable]
public class PlayerDataSave
{
    public PlayerDataSave( List<Cars> cars, List<string> tracks, int Cash )
    {
        playerCars = cars;
        unlockedLaps = tracks;
        cash = Cash;
    }
    public List<Cars> playerCars;
    public List<string> unlockedLaps;
    public int cash;
}