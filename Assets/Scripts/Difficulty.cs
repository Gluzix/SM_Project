using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public static float maxStickyVelocity;
    public static float minSlippyVelocity;
    public static float driftFactorSticky;
    public static float driftFactorSlippy;
    public static float speed;

    public static void SetDifficulty( string difficultyLevel )
    {
        if( difficultyLevel == "Easy" )
        {
            maxStickyVelocity = 10f;
            minSlippyVelocity = 10f;
            driftFactorSticky = 0.9f;
            driftFactorSlippy = 0.8f;
            speed = 25f;
        }
        else if ( difficultyLevel == "Medium" )
        {
            maxStickyVelocity = 40f;
            minSlippyVelocity = 40f;
            driftFactorSticky = 0.9f;
            driftFactorSlippy = 0.6f;
            speed = 26f;
        }
        else if (difficultyLevel == "Hard")
        {
            maxStickyVelocity = 90f;
            minSlippyVelocity = 90f;
            driftFactorSticky = 0.9f;
            driftFactorSlippy = 0.4f;
            speed = 28f;
        }
        else if (difficultyLevel == "Unbeatable")
        {
            maxStickyVelocity = 190f;
            minSlippyVelocity = 190f;
            driftFactorSticky = 0.9f;
            driftFactorSlippy = 0.2f;
            speed = 40f;
        }
        else
        {
            maxStickyVelocity = 0f;
            minSlippyVelocity = 0f;
            driftFactorSticky = 0f;
            driftFactorSlippy = 0f;
            speed = 0f;
        }
    }
}
