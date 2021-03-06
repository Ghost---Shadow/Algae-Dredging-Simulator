﻿using UnityEngine;
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
    public float stopVelocity = 0.05f;

    public Text waypointText;
    public Text velocityText;
    public Text dotText;

    private Vector3 currentWaypoint;

    private Rigidbody rb;

    private Vector3[] waypoints;
    private int c = 0;

    private FrontLinerRegister registry;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
        registry = transform.parent.GetComponent<FrontLinerRegister>();

        //currentWaypoint = getCurrentWaypoint();
    }

    void FixedUpdate()
    {        
        if (manualControl)
            manualControlLogic();
        else
            navigateToWaypoint();

        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)        
            rb.velocity = rb.velocity.normalized * maxSpeed;
        
        velocityText.text = "Velocity: " + rb.velocity.magnitude;
    }

    public void setWaypoints(Vector3[] wps)
    {
        waypoints = wps;
        c = 0;
        currentWaypoint = wps[c++];
        waypointText.text = "Waypoint: " + currentWaypoint;
        //Debug.Log("Got new waypoints");
    }

    public void requestNewWaypoints(){
        registry.requestWaypoints(this);
    }

    private Vector3 getCurrentWaypoint()
    {
        Vector3 wp = transform.position;
        if (waypoints != null)
        {
            if (c < waypoints.Length)
                wp = waypoints[c++];
            else
                registry.requestWaypoints(this);
        }
        else
        {
            registry.requestWaypoints(this);
        }
        waypointText.text = "Waypoint: " + wp;
        return wp;
    }

    private void navigateToWaypoint()
    {
        if(waypoints == null)
            currentWaypoint = getCurrentWaypoint();
        Vector3 direction = currentWaypoint - transform.position;
        if (direction.magnitude < triggerRadius)
        {
            if (rb.velocity.magnitude > stopVelocity)
                killVelocity();
            else
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
                if (direction.magnitude < 2.0f)
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

    private void killVelocity()
    {
        // rb.velocity = Vector3.zero;
        float component = Vector3.Dot(transform.forward, rb.velocity);
        rb.AddForce(-component * transform.forward * rb.mass * acceleration);
    }

    private void forward()
    {
        Vector3 direction = currentWaypoint - transform.position;
        float d = Mathf.Clamp(direction.magnitude,0,2.0f);
        rb.AddForce(transform.forward * rb.mass * acceleration * d);
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
        dotText.text = "Dot: " + dot + " Distance: " + direction.magnitude;
        rb.AddTorque(dot * Vector3.up * rb.mass * angularAcceleration);
    }
}
