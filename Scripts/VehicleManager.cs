using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float forwardTurningRadius = 25f;
    public float reverseTurningRadius = 25f;
    public float maxForwardSteerAngle = 100;
    public bool rearSteer = false;
    
    public GameObject leftWheel;
    public GameObject rightWheel;
}
 