using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2dController : MonoBehaviour
{
    float speedForce = 10f;
    float brakeForce = -5f;
    float torqueForce = 400f;
    float driftFactorSticky = 0.9f;
    float driftFactorSlippy = 1f;
    float maxStickyVelocity = 2.5f;
    float minSlippyVelocity = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        //52:36 - tutorial stopped
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float driftFactor = driftFactorSticky;

        if( RightVelocity().magnitude> maxStickyVelocity )
        {
            driftFactor = driftFactorSlippy;
        } 

        rb.velocity = ForwardVelocity() + RightVelocity()* driftFactorSlippy;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.up * speedForce);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(transform.up * brakeForce);
        }
        rb.angularVelocity = Input.GetAxis("Horizontal") * torqueForce; 

    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot( GetComponent<Rigidbody2D>().velocity, transform.up );
    }

    Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right );
    }
}
