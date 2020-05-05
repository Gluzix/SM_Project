using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    Transform target;
    float speedForce = 10f;
    float brakeForce = -50f;
    float driftFactorSticky = 0.9f;
    float driftFactorSlippy = 0.6f;
    float maxStickyVelocity = 100f;
    float minSlippyVelocity = 100f;
    float speed = 13f;
    float torqueForce = 400f;
    public float roadStickness = 1.0f;
    int index = 0;
    Rigidbody2D rb;
    float dirNum;
    float additionalSize = 0.2f;

    void Start()
    {
        target = GameObject.Find("controlPoint").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 heading = target.position - transform.position;
        dirNum = AngleDir(transform.forward, heading, transform.up);

        float driftFactor = driftFactorSticky;

        if (RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }

        if (DetectObstacles())
        {
            rb.angularVelocity = 50f;
        }
        else
        {
            rb.velocity = ForwardVelocity() + RightVelocity() * driftFactorSlippy;

            if (dirNum == 1)
            {
                rb.angularVelocity = 200f * (-1);
            }
            else if (dirNum == -1)
            {
                rb.angularVelocity = 200f;
            }
            else
            {
                rb.angularVelocity = 0f;
            }
        }

        rb.AddForce(transform.up * speed * 2f);


        if (Vector2.Distance(transform.position, target.position) < 2f)
        {
            string brakes;
            if( index == 0 )
            {
                brakes = "controlPoint";
            }
            else
            {
                brakes = "controlPoint (" + index.ToString() + ")";
            }

            target = GameObject.Find(brakes).GetComponent<Transform>();

            if (index == 10)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }

    private bool DetectObstacles()
    {
        Debug.DrawLine( transform.position + transform.up * 6f, transform.position+transform.up * 1.1f );

        RaycastHit2D hitFront = Physics2D.Raycast( transform.position + transform.up * 6f, transform.position + transform.up * 1.1f, 6f, 1 << LayerMask.NameToLayer("Racers") );

        /*if ( hitFront.collider != null)
        {
            Debug.Log("hit: " + hitFront.collider.name);
            
            return true;
        }
        else
        {
            return false;
        }*/
        return false;
    }

    Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);

        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
}