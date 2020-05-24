using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cars
{
    public string SpriteName;
    public string Name;
    public float speedForce;
    public float brakeForce;
    public float torqueForce;
    public float driftFactorSticky;
    public float driftFactorSlippy;
    public float maxStickyVelocity;
    public float minSlippyVelocity;
    public float speedTuning;
    public float roadStickness;
    public int points;
    public int tier;
}

[System.Serializable]
public class CarListObject
{
    public List<Cars> carList;
}
