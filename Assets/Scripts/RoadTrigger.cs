﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTrigger : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "car1")
        {
            other.gameObject.GetComponent<Car2dController>().roadStickness = 0.5f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "car1")
        {
            other.gameObject.GetComponent<Car2dController>().roadStickness = 1.0f;
        }
    }
}
