using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform target;
    public GameObject map;
    /*public float speedForce = 10f;
    public float brakeForce = -50f;
    public float driftFactorSticky = 0.9f;
    public float driftFactorSlippy = 0.6f;
    public float maxStickyVelocity = 100f;
    public float minSlippyVelocity = 100f;
    public float torqueForce = 400f;
    public float roadStickness = 1.0f;*/
    private Cars currentCar;
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
        GenerateName();
        RandomizeCar();
        this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Cars/" + currentCar.SpriteName);
        target = GameObject.Find("controlPoint").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        map.GetComponent<MapRules>().racerPositions.Add(this.gameObject);
        this.GetComponent<RacerStat>().SetName(this.name);
        amountOfControls = GameObject.Find("path").transform.childCount;
    }

    void GenerateName()
    {
        Transform racersObject = GameObject.Find("Racers").transform;
        int childCounter = racersObject.childCount;
        int currentObjectIndex = 0;

        for (int i = 0; i < childCounter; i++)
        {
            if (racersObject.GetChild(i).name == this.name)
            {
                currentObjectIndex = i;
            }
        }
        RandomizeName();
        for (int i = 0; i < childCounter; i++)
        {
            if (currentObjectIndex == i)
                continue;

            if (racersObject.GetChild(i).name == this.name)
            {
                i = 0;
                RandomizeName();
            }
        }
    }

    void RandomizeName()
    {
        int number = Random.Range(0, GlobalVars.racerNames.Length - 1);
        this.name = GlobalVars.racerNames[number];
    }

    void RandomizeCar()
    {
        int number = Random.Range(0, SelectionMenu.carList.carList.Count - 1);
        currentCar = SelectionMenu.carList.carList[number];
    }

    // Update is called once per frame
    void Update()
    {
        int number = Random.Range(0, GlobalVars.racerNames.Length - 1);
    }

    void FixedUpdate()
    {
        Vector3 heading = target.position - transform.position;
        dirNum = AngleDir(transform.forward, heading, transform.up);

        if (bIfDriving)
        {
            float driftFactor = Difficulty.driftFactorSticky;

            if ( RightVelocity().magnitude > (currentCar.maxStickyVelocity+Difficulty.maxStickyVelocity) )
            {
                driftFactor = Difficulty.driftFactorSlippy;
            }

            if (DetectObstacles())
            {
                rb.velocity = ForwardVelocity() + RightVelocity() * Difficulty.driftFactorSlippy;

                if (direction == 0)
                {
                    rb.angularVelocity = 200f;
                    rb.AddForce(transform.up * (Difficulty.speed + currentCar.torqueForce / 500));
                }
                else if (direction == 1)
                {
                    rb.angularVelocity = -200f;
                    rb.AddForce(transform.up * (Difficulty.speed + currentCar.torqueForce / 500));
                }
                else if (direction == -1)
                {
                    rb.angularVelocity = 100f;
                    rb.AddForce(transform.up * (Difficulty.speed + currentCar.torqueForce / 500));

                }
                else if (direction == -2)
                {
                    rb.angularVelocity = -100f;
                    rb.AddForce(transform.up * (Difficulty.speed + currentCar.torqueForce / 500));
                }

            }
            else
            {
                rb.velocity = ForwardVelocity() + RightVelocity() * Difficulty.driftFactorSlippy;

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

            rb.AddForce(transform.up * ( Difficulty.speed+currentCar.torqueForce / 500) );


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

            if (lap > map.GetComponent<MapRules>().laps)
            {
                bIfDriving = false;
            }

            if (bIfDriving)
            {
                this.GetComponent<RacerStat>().SetControlPoint(allControlPoints);
            }
        }
    }
}