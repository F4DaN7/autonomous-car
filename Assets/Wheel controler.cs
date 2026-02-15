using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    public float acceleration = 1500f;
    public float breakingForce = 30000f;
    public float maxSpeed = 30f; 

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    public float currentTurnAngle = 0f;
    public float maxTurnAngle = 15f;

    private void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakForce = breakingForce;
        }
        else
        {
            currentBreakForce = 0f;
        }


        float currentSpeed = Mathf.Max(frontRight.rpm, frontLeft.rpm, backRight.rpm, backLeft.rpm) * (2 * Mathf.PI * frontRight.radius) / 60f;


        if (currentSpeed > maxSpeed)
        {
            currentAcceleration = 0f;
        }

        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
    }
}
