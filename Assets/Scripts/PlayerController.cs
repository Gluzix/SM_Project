using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //zmienna speedForce określa jaka siła zostanie dodana do wektora up
    Transform target;
    public GameObject map;
    GameObject raceMenu;
    public static bool gameIsPaused;
    int allControlPoints = 1;
    int currentControlPoints = 1;
    int lap = 1;
    int amountOfControls = 0;
    bool bIfDriving = true;
    private Cars currentCar;
    float pitch = 1f;
    bool throttle = false;

    // Start is called before the first frame update
    void Start()
    {
        currentCar = SelectionMenu.currentCar;
        this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>( "Cars/"+currentCar.SpriteName );
        target = GameObject.Find("controlPoint").GetComponent<Transform>();
        map.GetComponent<MapRules>().racerPositions.Add(this.gameObject);
        this.GetComponent<RacerStat>().SetName(this.name);
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

            float driftFactor = currentCar.driftFactorSticky;

            if (RightVelocity().magnitude > currentCar.maxStickyVelocity)
            {
                driftFactor = currentCar.driftFactorSlippy;
            }

            rb.velocity = ForwardVelocity() + RightVelocity() * currentCar.driftFactorSlippy;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForce(transform.up * currentCar.speedForce * currentCar.speedTuning * currentCar.roadStickness);
                throttle = true;
                if (pitch < 3f)
                {
                    pitch += 0.05f;
                }
            }
            else
            {
                throttle = false;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddForce(transform.up * currentCar.brakeForce);
                if (pitch > 1f)
                {
                    pitch -= 0.10f;
                }
            }


            float tf = Mathf.Lerp(0, currentCar.torqueForce, rb.velocity.magnitude / 4);

            rb.angularVelocity = Input.GetAxis("Horizontal") * tf;

        }

        if(pitch>1f && !throttle)
        {
            pitch -= 0.05f;
        }

        EngineSound();

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

            if ( lap > map.GetComponent<MapRules>().laps && bIfDriving )
            {
                bIfDriving = false;
                raceMenu.SetActive(true);
                bool bIfUnlocked = map.GetComponent<MapRules>().PlayerPlace();

                string nextTrackName = "track_" + (SelectionMenu.currentTrackIndex + 2).ToString();
                if ( SelectionMenu.currentTrackIndex < SelectionMenu.trackAmount-1 )
                {
                    if ( !PlayerData.unlockedLaps.Contains( nextTrackName ) && bIfUnlocked )
                    {
                        PlayerData.unlockedLaps.Add( nextTrackName );
                    }
                }

                PlayerData.SaveGame();
            }

        }

        if (bIfDriving)
        {
            this.GetComponent<RacerStat>().SetControlPoint(allControlPoints);
        }
    }

    public int GetLaps()
    {
        return lap;
    }

    public Cars GetCurrentCar()
    {
        return currentCar;
    }

    void EngineSound()
    {
        AudioSource audio = this.GetComponent<AudioSource>();
        audio.pitch = pitch;
    }
}