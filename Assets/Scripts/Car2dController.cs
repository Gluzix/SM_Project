using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2dController : MonoBehaviour
{
    //zmienna speedForce określa jaka siła zostanie dodana do wektora up
    float speedForce = 10f;
    float brakeForce = -50f;
    float torqueForce = 400f;
    float driftFactorSticky = 0.9f;
    float driftFactorSlippy = 1f;
    float maxStickyVelocity = 2.5f;
    float minSlippyVelocity = 1.5f;
    float speedTuning = 3.0f;
    public float roadStickness = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float driftFactor = driftFactorSticky;

       /* if ( gameObject.transform.position.x > ( roadPos.transform.position.x) - ( roadPos.GetComponent<MeshRenderer>().bounds.size.x / 2 ) )
        {
            roadStickness = 1.0f;
        }
        else
        {
            roadStickness = 0.5f;
        } */     

        if (RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }

        rb.velocity = ForwardVelocity() + RightVelocity() * driftFactorSlippy;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.up * speedForce * speedTuning * roadStickness);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(transform.up * brakeForce);
        }

        float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 4 );

        rb.angularVelocity = Input.GetAxis("Horizontal") * tf;

        //Debug.Log(rb.velocity.magnitude*5);
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
