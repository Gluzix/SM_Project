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
    float driftFactorSlippy = 0.92f;
    float maxStickyVelocity = 100f;
    float minSlippyVelocity = 100f;
    float speed = 10f;
    float torqueForce = 400f;
    public float roadStickness = 1.0f;
    int index = 1;
    Rigidbody2D rb;
    bool turning = false;
    float dirNum;

    void Start()
    {
        target = GameObject.Find("brake_1").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 heading = target.position - transform.position;
        dirNum = AngleDir(transform.forward, heading, transform.up);

        float driftFactor = driftFactorSticky;

        if (RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }

        rb.velocity = ForwardVelocity() + RightVelocity() * driftFactorSlippy;

        if ( dirNum == 1 )
        {
            rb.angularVelocity = 200f * (-1);
        }
        else if( dirNum == -1 )
        {
            rb.angularVelocity = 200f;
        }
        else
        {
            rb.angularVelocity = 0f;
        }


        rb.AddForce(transform.up * speed);


        if( Vector2.Distance(transform.position, target.position) < 2f )
        {
            string brakes = "brake_" + index.ToString();
            target = GameObject.Find(brakes).GetComponent<Transform>();

            if ( index == 4 )
            {
                index = 1;
            }
            else
            {
                index++;
            }
        }
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

void FixedUpdate()
{
/* if( Vector2.Distance(transform.position, target.position) < 2f )
 {
     target = GameObject.Find("brake_1").GetComponent<Transform>();
     turning = true;
 }

 //rb.AddForce( transform.up * speed );

 var heading = target.position - transform.position;
 var direction = heading.normalized;


 Vector3 cross = Vector3.Cross(direction, Vector3.right *(-1));

 rb.velocity = cross * speed;

 rb.angularVelocity = cross.x * speed; ;*/

        //rb.angularVelocity = cross * speed;

        /*if ( direction.x == 0 )
        {
            rb.angularVelocity = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime).magnitude;
        }*/


        //Debug.Log(heading);
        //Debug.Log(direction);



        //rb.angularVelocity = rb.angularVelocity = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime).magnitude ;

        /*
            float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 4);

            Vector2.

            rb.angularVelocity = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime).sqrMagnitude;

            Debug.Log(Vector2.Angle(transform.position, target.position) );
            */

        //Debug.Log(Vector2.Distance(transform.position, target.position));

    }
}
