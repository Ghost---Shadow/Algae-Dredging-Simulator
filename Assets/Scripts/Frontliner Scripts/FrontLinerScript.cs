using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrontLinerScript : MonoBehaviour
{
    public bool manualControl = false;
    public float acceleration = 10.0f;
    public float angularAcceleration = 5.0f;
    public float maxSpeed = 5.0f;
    public float maxAngularVelocity = 1.0f;
    public float motorHeight = .1f;
    public float navigationError = .1f;
    public float triggerRadius = .1f;

    public Text waypointText;
    public Text velocityText;
    public Text dotText;

    private Vector3 currentWaypoint;

    private Rigidbody rb;

    private Vector3[] _wps;
    private int _c = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;

        _wps = new Vector3[3];
        _wps[0] = new Vector3(8, 0, 8);
        _wps[1] = new Vector3(7, 0, 8);
        _wps[2] = new Vector3(7, 0, -8);

        currentWaypoint = getCurrentWaypoint();
    }

    void FixedUpdate()
    {
        if (manualControl)
        {
            manualControlLogic();
        }
        else
        {
            navigateToWaypoint();
        }
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        velocityText.text = "" + rb.velocity.magnitude;
    }

    private Vector3 getCurrentWaypoint()
    {
        Vector3 wp = _wps[_c++ % _wps.Length];
        waypointText.text = "" + wp;
        return wp;
    }

    private void navigateToWaypoint()
    {
        Vector3 direction = currentWaypoint - transform.position;
        if (direction.magnitude < triggerRadius)
        {
            currentWaypoint = getCurrentWaypoint();
            return;
        }

        turn();
        if (Vector3.Dot(direction.normalized, transform.forward) > 0)
        {
            if (Mathf.Abs(Vector3.Dot(direction.normalized, transform.right)) < navigationError)
                forward();
        }
        else
        {
            if (Mathf.Abs(Vector3.Dot(direction.normalized, transform.right)) < navigationError)
                back();
        }
    }

    private void manualControlLogic()
    {
        if (Mathf.Abs(transform.position.y) < motorHeight)
        {
            if (Input.GetKey(KeyCode.I))
                forward();
            else if (Input.GetKey(KeyCode.K))
                back();
            else if (Input.GetKey(KeyCode.J))
                left();
            else if (Input.GetKey(KeyCode.L))
                right();
        }
    }

    private void forward()
    {
        Vector3 direction = currentWaypoint - transform.position;
        //float s = Mathf.Sin(Mathf.Acos(Vector3.Dot(direction.normalized,transform.right)));
        rb.AddForce(transform.forward * rb.mass * acceleration * direction.magnitude);
    }

    private void back()
    {
        rb.AddForce(-transform.forward * rb.mass * acceleration);
    }

    private void left()
    {
        rb.AddTorque(-Vector3.up * rb.mass * angularAcceleration);
    }
    private void right()
    {
        rb.AddTorque(Vector3.up * rb.mass * angularAcceleration);
    }

    private void turn()
    {
        Vector3 direction = currentWaypoint - transform.position;
        float dot = Vector3.Dot(direction.normalized, transform.right);
        dotText.text = dot + ":" + direction.magnitude;
        rb.AddTorque(dot * Vector3.up * rb.mass * angularAcceleration);
    }
}
