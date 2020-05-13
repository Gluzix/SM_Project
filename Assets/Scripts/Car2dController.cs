using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2dController : MonoBehaviour
{
    //zmienna speedForce określa jaka siła zostanie dodana do wektora up
    Transform target;
    public GameObject map;
    public float speedForce = 10f;
    public float brakeForce = -50f;
    public float torqueForce = 400f;
    public float driftFactorSticky = 0.9f;
    public float driftFactorSlippy = 1f;
    public float maxStickyVelocity = 2.5f;
    public float minSlippyVelocity = 1.5f;
    public float speedTuning = 3.0f;
    public float roadStickness = 1.0f;
    int position = -1;
    int index = 0;
    int lap = 1;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("controlPoint").GetComponent<Transform>();

        map.GetComponent<mapRules>().racerPositions.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

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

        float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 4 );

        rb.angularVelocity = Input.GetAxis("Horizontal") * tf;

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
            if (index == 0)
            {
                brakes = "controlPoint";
            }
            else
            {
                brakes = "controlPoint (" + index.ToString() + ")";
            }

            target = GameObject.Find(brakes).GetComponent<Transform>();

            if (index == 15)
            {
                index = 0;
                lap++;
            }
            else
            {
                index++;
            }
        }
    }

}
