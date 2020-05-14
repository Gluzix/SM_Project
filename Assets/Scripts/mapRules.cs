using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapRules : MonoBehaviour
{
    public List< GameObject > racerPositions;
    GameObject racer;
    public int laps = 3;
    // Start is called before the first frame update
    void Start()
    {
        racer = GameObject.Find("car1");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void CheckPositions()
    {
        for ( int i=0; i<racerPositions.Count; i++)
        {
            for ( int j=0; j<racerPositions.Count; j++)
            {
                if( i == j )
                {
                    continue;
                }
                if( racerPositions[i].GetComponent<racerStat>().GetControlPointNumber() > racerPositions[j].GetComponent<racerStat>().GetControlPointNumber() )
                {
                    GameObject temp = racerPositions[i];
                    racerPositions[i] = racerPositions[j];
                    racerPositions[j] = temp;
                }              
            }
        }

        for (int i = 0; i < racerPositions.Count; i++)
        {         
            Debug.Log( (i+1).ToString()+" "+ racerPositions[i].GetComponent<racerStat>().name + " "+ racerPositions[i].GetComponent<racerStat>().GetControlPointNumber().ToString() );
        }
    }
}
