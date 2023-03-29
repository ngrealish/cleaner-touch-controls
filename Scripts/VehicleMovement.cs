using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController controller;
    private VehicleManager vehicle;

    public string defaultType;
    public string defaultVehicle;
    
    private float currentSpeed;
    private float maxSpeed = 10f;
    private float forwardTurningRadius = 25f;
    private float reverseTurningRadius = 25f;
    private float maxForwardSteerAngle = 100;
    private bool rearSteer;

    private GameObject leftWheel;
    private GameObject rightWheel;

    private void Awake()
    {
        controls = GameObject.Find("GameManager").GetComponent<PlayerControls>();
        controller = transform.GetComponent<CharacterController>();
        setVehicle();
    }

    private void setVehicle()
    {
        foreach (Transform truck in transform.Find(defaultType).transform) {
            truck.gameObject.SetActive(false); 
        }
        transform.Find(defaultType).transform.Find("ExitPoint").gameObject.SetActive(true);
        transform.Find(defaultType).transform.Find(defaultVehicle).gameObject.SetActive(true);
        
        vehicle = transform.Find(defaultType).transform.Find(defaultVehicle).GetComponent<VehicleManager>();
        maxSpeed = vehicle.maxSpeed;
        forwardTurningRadius = vehicle.forwardTurningRadius;
        reverseTurningRadius = vehicle.reverseTurningRadius;
        maxForwardSteerAngle = vehicle.maxForwardSteerAngle;
        rearSteer = vehicle.rearSteer;
        leftWheel = vehicle.leftWheel;
        rightWheel = vehicle.rightWheel;
    }

    // Update is called once per frame
    private void Update()
    {
        handleMovement();
    }

    public void handleMovement()
    {

        float angleDiff = getAngleDiff();
        handleThrottle(angleDiff);
        handleSteer(angleDiff);
        
    }

    private float getAngleDiff()
    {
        float moveDirection = controls.moveDirection;
        if (moveDirection < -180)
        {
            float diff = -(moveDirection + 180f);
            moveDirection = 180 - diff;
        }
        float carFacing = -Vector3.SignedAngle(transform.forward, Vector3.forward, transform.up);
        

        float angleDiff = moveDirection - carFacing;
        angleDiff = (angleDiff < -180) ? angleDiff + 360 : angleDiff;
        angleDiff = (angleDiff > 180) ? angleDiff - 360 : angleDiff;
        // Debug.Log(moveDirection + " " + carFacing + " " + angleDiff);
        return angleDiff;
    }

    private void handleThrottle(float angleDiff)
    {
        Vector3 direction = (angleDiff < maxForwardSteerAngle && angleDiff > -maxForwardSteerAngle) ? Vector3.forward : -Vector3.forward;
        Vector3 movement = controls.moveSpeed * maxSpeed * transform.TransformDirection(direction);
        controller.Move(movement * Time.deltaTime);
    }
    
    private void handleSteer(float angleDiff)
    {
        float steerPercent = Mathf.Abs(angleDiff / 90);
        steerPercent = (steerPercent > 1) ? 2 - steerPercent : steerPercent;
        float turnAngle = (angleDiff < maxForwardSteerAngle && angleDiff > -maxForwardSteerAngle)
            ? forwardTurningRadius * steerPercent
            : reverseTurningRadius * steerPercent;
        float rotationSpeed = (1 - (controls.moveSpeed / maxSpeed)) * 2 * controls.moveSpeed * turnAngle;
        float direction = (angleDiff < maxForwardSteerAngle && angleDiff > -maxForwardSteerAngle) ? (angleDiff < 0) ? -1 : 1 : (angleDiff < 0) ? 1 : -1;
        transform.Rotate(Vector3.up * direction * rotationSpeed * Time.fixedDeltaTime);

        float rearSteerTurn = (rearSteer) ? -1 : 1;
        float steer = (angleDiff < 0) ? -steerPercent * rearSteerTurn : steerPercent * rearSteerTurn;

        Quaternion wheelTurn = //(angleDiff < 0) ? 
            Quaternion.Euler(0, steer * 45, 0); //: 
            // Quaternion.Euler(0, steerPercent * 45, 0);
        leftWheel.transform.localRotation = wheelTurn;
        rightWheel.transform.localRotation = wheelTurn;
    }
}
