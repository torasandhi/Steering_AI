using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICarController : MonoBehaviour
{
    [Header("Path & Waypoints")]
    public Transform pathGroup; // The parent object holding the waypoints
    public float distFromPath = 20f; // How close the car needs to be to a waypoint to move to the next one

    [Header("Car Physics & Setup")]
    public Transform centerOfMassObject; // An empty GameObject placed at the car's desired center of mass
    private Rigidbody rb;

    [Header("Wheel Colliders")]
    public WheelCollider wheelFL; // Front Left
    public WheelCollider wheelFR; // Front Right
    public WheelCollider wheelRL; // Rear Left
    public WheelCollider wheelRR; // Rear Right

    [Header("Car Specs")]
    public float maxSteerAngle = 15f;
    public float maxTorque = 50f;
    public float topSpeed = 150f;
    public float decelerationSpeed = 10f;

    // Internal variables
    private List<Transform> pathNodes = new List<Transform>();
    private int currentNode = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Set the center of mass from the visual helper object
        if (centerOfMassObject != null)
        {
            rb.centerOfMass = transform.InverseTransformPoint(centerOfMassObject.position);
        }

        GetPath();
    }

    void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
    }

    // Populates the list of waypoints from the pathGroup object
    void GetPath()
    {
        Transform[] childObjects = pathGroup.GetComponentsInChildren<Transform>();

        // Add all child transforms to the pathNodes list, ignoring the parent itself
        foreach (Transform child in childObjects)
        {
            if (child != pathGroup.transform)
            {
                pathNodes.Add(child);
            }
        }
    }

    // Handles steering logic
    void ApplySteer()
    {
        if (pathNodes.Count == 0) return;

        // Calculate a vector from the car to the current waypoint in the car's local space
        Vector3 steerVector = transform.InverseTransformPoint(pathNodes[currentNode].position);

        // Calculate the new steering angle
        float newSteer = maxSteerAngle * (steerVector.x / steerVector.magnitude);

        // Apply the steering angle to the front wheels
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    // Handles acceleration and braking
    void Drive()
    {
        // Calculate current speed in km/h
        float currentSpeed = rb.linearVelocity.magnitude * 3.6f;

        // If below top speed, accelerate. Otherwise, brake.
        if (currentSpeed < topSpeed)
        {
            // Apply torque to rear wheels
            wheelRL.motorTorque = maxTorque;
            wheelRR.motorTorque = maxTorque;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
        else
        {
            // Apply brakes and cut motor torque
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
            wheelRL.brakeTorque = decelerationSpeed;
            wheelRR.brakeTorque = decelerationSpeed;
        }
    }

    // Checks distance to the current waypoint and updates to the next one if close enough
    void CheckWaypointDistance()
    {
        if (pathNodes.Count == 0) return;

        float distance = Vector3.Distance(transform.position, pathNodes[currentNode].position);

        if (distance < distFromPath)
        {
            // If we are close to the last waypoint, loop back to the first one
            if (currentNode == pathNodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                // Move to the next waypoint in the list
                currentNode++;
            }
        }
    }
}