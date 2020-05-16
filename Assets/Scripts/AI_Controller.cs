using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    Transform target;
    public GameObject map;
    public float speedForce = 10f;
    public float brakeForce = -50f;
    public float driftFactorSticky = 0.9f;
    public float driftFactorSlippy = 0.6f;
    public float maxStickyVelocity = 100f;
    public float minSlippyVelocity = 100f;
    public float speed = 13f;
    public float torqueForce = 400f;
    public float roadStickness = 1.0f;
    int allControlPoints = 1;
    int currentControlPoints = 1;
    Rigidbody2D rb;
    float dirNum;
    int direction = 0;
    bool bIfDriving = true;
    int lap = 1;
    int amountOfControls = 0;

    void Start()
    {
        target = GameObject.Find("controlPoint").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        map.GetComponent<mapRules>().racerPositions.Add(this.gameObject);
        this.GetComponent<racerStat>().SetName(this.name);
        amountOfControls = GameObject.Find("path").transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 heading = target.position - transform.position;
        dirNum = AngleDir(transform.forward, heading, transform.up);

        if (bIfDriving)
        {
            float driftFactor = driftFactorSticky;

            if (RightVelocity().magnitude > maxStickyVelocity)
            {
                driftFactor = driftFactorSlippy;
            }

            if (DetectObstacles())
            {
                rb.velocity = ForwardVelocity() + RightVelocity() * driftFactorSlippy;

                if (direction == 0)
                {
                    rb.angularVelocity = 200f;
                    rb.AddForce(transform.up * speed * 2f);
                }
                else if (direction == 1)
                {
                    rb.angularVelocity = -200f;
                    rb.AddForce(transform.up * speed * 2f);
                }
                else if (direction == -1)
                {
                    rb.angularVelocity = 100f;
                    rb.AddForce(transform.up * speed * 2f);

                }
                else if (direction == -2)
                {
                    rb.angularVelocity = -100f;
                    rb.AddForce(transform.up * speed * 2f);
                }

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


            /*if (Vector2.Distance(transform.position, target.position) < 2f)
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
            } */
        }
    }

    private bool DetectObstacles()
    {
        bool ifCollided = false;

        Debug.DrawLine( transform.position + transform.up * 2.5f, transform.position + transform.up * 1.1f );
        Debug.DrawLine( transform.position + transform.up * 2.5f + transform.right * 0.2f, transform.position + transform.up * 1.1f + transform.right * 0.2f);
        Debug.DrawLine( transform.position + transform.up * 2.5f + transform.right * (-0.2f), transform.position + transform.up * 1.1f + transform.right * (-0.2f) );

        RaycastHit2D raycastHit = Physics2D.Raycast( transform.position + transform.up * 2.5f, transform.position + transform.up * 1.1f, 2.5f, 1 << LayerMask.NameToLayer("Racers") );
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
        }
        raycastHit = Physics2D.Raycast(transform.position + transform.up * 2.5f + transform.right * 0.2f, transform.position + transform.up * 1.1f + transform.right * 0.2f, 2.5f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
        }
        raycastHit = Physics2D.Raycast(transform.position + transform.up * 2.5f + transform.right * (-0.2f), transform.position + transform.up * 1.1f + transform.right * (-0.2f), 2.5f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
        }

        Debug.DrawLine(transform.position + transform.up * 2.5f + transform.right * 0.5f, transform.position + transform.up * 1.1f + transform.right * 0.2f);
        Debug.DrawLine(transform.position + transform.up * 2.5f + transform.right * (-0.5f), transform.position + transform.up * 1.1f + transform.right * (-0.2f) );

        raycastHit = Physics2D.Raycast(transform.position + transform.up * 2.5f + transform.right * 0.5f, transform.position + transform.up * 1.1f + transform.right * 0.2f, 2.5f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
            direction = 0;
        }
        raycastHit = Physics2D.Raycast(transform.position + transform.up * 2.5f + transform.right * (-0.5f), transform.position + transform.up * 1.1f + transform.right * (-0.2f), 2.5f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
            direction = 1;
        }

        Debug.DrawLine(transform.position + transform.up * 2f + transform.right * 1f, transform.position + transform.up * 1.1f + transform.right * 0.2f);
        Debug.DrawLine(transform.position + transform.up * 2f + transform.right * (-1f), transform.position + transform.up * 1.1f + transform.right * (-0.2f));

        raycastHit = Physics2D.Raycast(transform.position + transform.up * 2f + transform.right * 1f, transform.position + transform.up * 1.1f + transform.right * 0.2f, 2f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
            direction = 0;
        }
        raycastHit = Physics2D.Raycast(transform.position + transform.up * 2f + transform.right * (-1f), transform.position + transform.up * 1.1f + transform.right * (-0.2f), 2f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
            direction = 1;
        }

        Debug.DrawLine(transform.position + transform.up * 1.5f + transform.right * 1f, transform.position + transform.up * 1.1f + transform.right * 0.2f);
        Debug.DrawLine(transform.position + transform.up * 1.5f + transform.right * (-1f), transform.position + transform.up * 1.1f + transform.right * (-0.2f));

        raycastHit = Physics2D.Raycast(transform.position + transform.up * 1.5f + transform.right * 1f, transform.position + transform.up * 1.1f + transform.right * 0.2f, 2f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
            direction = 0;
        }
        raycastHit = Physics2D.Raycast(transform.position + transform.up * 1.5f + transform.right * (-1f), transform.position + transform.up * 1.1f + transform.right * (-0.2f), 2f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
            direction = 1;
        }


        Debug.DrawLine(transform.position + transform.right * 0.45f, transform.position + transform.right * 0.2f);
        Debug.DrawLine(transform.position + transform.right * (-0.45f), transform.position + transform.right * (-0.2f));

        raycastHit = Physics2D.Raycast(transform.position + transform.right * 0.45f, transform.position + transform.right * 0.2f, 2f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
            direction = -1;
        }
        raycastHit = Physics2D.Raycast(transform.position + transform.right * (-0.45f), transform.position + transform.right * (-0.2f), 2f, 1 << LayerMask.NameToLayer("Racers"));
        if (raycastHit.collider != null && raycastHit.collider.name != this.name)
        {
            ifCollided = true;
            direction = -2;
        }

        if (ifCollided)
        {
            return true;
        }
        else
        {
            return false;
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

    /*void OnTriggerExit2D(Collider2D other)
    {
        if ( other.gameObject.transform.parent.name == "path" )
        {

        }
    }*/

    void OnTriggerEnter2D(Collider2D other)
    {

        if ( other.gameObject.name == target.name )
        {
            string brakes;
            if (currentControlPoints == 0)
            {
                brakes = "controlPoint";
            }
            else
            {
                brakes = "controlPoint (" + currentControlPoints.ToString() + ")";
            }

            target = GameObject.Find(brakes).GetComponent<Transform>();

            if ( currentControlPoints == amountOfControls-1 )
            {
                currentControlPoints = 0;
                lap++;
            }
            else
            {
                currentControlPoints++;
                allControlPoints++;
            }

            if (lap > map.GetComponent<mapRules>().laps)
            {
                bIfDriving = false;
            }

            if (bIfDriving)
            {
                this.GetComponent<racerStat>().SetControlPoint(allControlPoints);
            }
        }
    }
}