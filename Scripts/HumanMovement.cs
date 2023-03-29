using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{

    private PlayerControls controls;
    private CharacterController controller;
    public float maxSpeed = 10f;
    private float currentSpeed;

    private void Awake()
    {
        controls = GameObject.Find("GameManager").GetComponent<PlayerControls>();
        controller = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        handleMovement();
    }

    public void handleMovement()
    {
        transform.rotation = Quaternion.Euler(0, controls.moveDirection, 0);
        Vector3 movement = transform.TransformDirection(Vector3.forward) * controls.moveSpeed * maxSpeed;
        controller.Move(movement * Time.deltaTime);
    }
}
