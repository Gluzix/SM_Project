using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Car2dController : MonoBehaviour
{
    //zmienna speedForce określa jaka siła zostanie dodana do wektora up
    Transform target;
    public GameObject map;
    GameObject raceMenu;
    public static bool gameIsPaused;
    public float speedForce = 10f;
    public float brakeForce = -50f;
    public float torqueForce = 400f;
    public float driftFactorSticky = 0.9f;
    public float driftFactorSlippy = 1f;
    public float maxStickyVelocity = 2.5f;
    public float minSlippyVelocity = 1.5f;
    public float speedTuning = 3.0f;
    public float roadStickness = 1.0f;
    int allControlPoints = 1;
    int currentControlPoints = 1;
    int lap = 1;
    int amountOfControls = 0;

    bool bIfDriving = true;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = SelectionMenu.currentStaticSprite;
        target = GameObject.Find("controlPoint").GetComponent<Transform>();
        map.GetComponent<mapRules>().racerPositions.Add(this.gameObject);
        this.GetComponent<racerStat>().SetName(this.name);
        amountOfControls = GameObject.Find("path").transform.childCount;
        raceMenu = GameObject.Find("RaceMenu");
        raceMenu.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            raceMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            raceMenu.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if ( bIfDriving )
        {

            float driftFactor = driftFactorSticky;

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


            float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 4);

            rb.angularVelocity = Input.GetAxis("Horizontal") * tf;

        }
       
    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot( GetComponent<Rigidbody2D>().velocity, transform.up );
    }

    Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == target.name)
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

            if (currentControlPoints == amountOfControls-1)
            {
                currentControlPoints = 0;
                lap++;
            }
            else
            {
                currentControlPoints++;
                allControlPoints++;
            }

            if ( lap > map.GetComponent<mapRules>().laps)
            {
                bIfDriving = false;
                raceMenu.SetActive(true);
            }

        }

        if (bIfDriving)
        {
            this.GetComponent<racerStat>().SetControlPoint(allControlPoints);
        }
    }

    public int GetLaps()
    {
        return lap;
    }
}
