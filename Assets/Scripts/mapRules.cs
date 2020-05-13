using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapRules : MonoBehaviour
{
    public List< GameObject > racerPositions;
    public int laps = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void AddRacerToList( GameObject racer )
    {

    }

    public int GetLaps()
    {
        return laps;
    }
}
