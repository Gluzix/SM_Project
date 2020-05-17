using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTrigger : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().roadStickness = 0.5f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().roadStickness = 1.0f;
        }
    }
}
