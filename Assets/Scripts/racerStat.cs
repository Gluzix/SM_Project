using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class racerStat : MonoBehaviour
{
    private string name;
    private int controlPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
