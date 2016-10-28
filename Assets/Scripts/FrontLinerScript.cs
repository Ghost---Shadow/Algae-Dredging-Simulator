using UnityEngine;
using System.Collections;

public class FrontLinerScript : MonoBehaviour
{
    public bool manualControl = false;
    public float acceleration = 10.0f;
    public float maxSpeed = 5.0f;
    public float maxAngularVelocity = 1.0f;
    public float motorHeight = .1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
    }

    void FixedUpdate()
    {
        if (manualControl)
        {
            if (Mathf.Abs(transform.position.y) < motorHeight)
            {
                if (Input.GetKey(KeyCode.I))
                {
                    rb.AddForce(transform.forward * rb.mass * acceleration);
                }
                else if (Input.GetKey(KeyCode.K))
                {
                    rb.AddForce(-transform.forward * rb.mass * acceleration);
                }
                else if (Input.GetKey(KeyCode.J))
                {
                    rb.AddTorque(-Vector3.up * rb.mass * acceleration);
                }
                else if (Input.GetKey(KeyCode.L))
                {
                    rb.AddTorque(Vector3.up * rb.mass * acceleration);
                }
            }
        }
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
