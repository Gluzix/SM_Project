using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapRules : MonoBehaviour
{
    public List< GameObject > racerPositions;
    GameObject racer;
    GameObject positionsText;
    GameObject lapsText;
    public int laps = 3;
    // Start is called before the first frame update
    void Start()
    {
        racer = GameObject.Find("car1");
        positionsText = GameObject.Find("Text (TMP)");
        lapsText = GameObject.Find("Text (TMP)_2");
    }

    // Update is called once per frame
    void Update()
    {
        CheckPositions();
    }

    public void CheckPositions()
    {
        racerPositions.Sort( SortByCheckpoints );

        int currentPlayerLap = racer.GetComponent<Car2dController>().GetLaps();
        string lapText = "Laps: " + currentPlayerLap.ToString() + "/" + laps.ToString();
        string posText ="Positions \n";
        for (int i = 0; i < racerPositions.Count; i++)
        {
            posText += (i + 1).ToString() + " " + racerPositions[i].GetComponent<racerStat>().name + " " + "\n";
        }
        positionsText.GetComponent<TMPro.TextMeshProUGUI>().text = posText;
        lapsText.GetComponent<TMPro.TextMeshProUGUI>().text = lapText;
    }

    static int SortByCheckpoints(GameObject p1, GameObject p2)
    {
        return p2.GetComponent<racerStat>().GetControlPointNumber().CompareTo(p1.GetComponent<racerStat>().GetControlPointNumber());
    }

}
