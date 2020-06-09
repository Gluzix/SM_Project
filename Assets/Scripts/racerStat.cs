using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerStat : MonoBehaviour
{
    private int controlPoint;

    public void SetName(string nm)
    {
        name = nm;
    }

    public void SetControlPoint(int point)
    {
        controlPoint = point;
    }

    public int GetControlPointNumber()
    {
        return controlPoint;
    }
}
