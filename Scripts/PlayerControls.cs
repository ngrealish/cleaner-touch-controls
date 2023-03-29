using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private GameSetup gameSetup;
    private GameObject UIContainer;
    private PlayerActions playerActions;

    private float touchTimer = 0f;
    private float tapTime = 1f;
    private Vector2 touchPositionInitial;
    private Vector2 touchPositionCurrent;

    public bool tapped = false;

    private float dragDirection;
    private float dragDistance;
    private Vector2 dragFollow;
    private bool touching = false;

    public float moveDirection;
    public float moveSpeed;

    // This is all for the temporary dots within the UI
    private Transform temporaryUI;
    private Transform dot1Transform;
    private Transform dot2Transform;
    private RectTransform dot1;
    private RectTransform dot2;

    private void Awake()
    {
        gameSetup = transform.GetComponent<GameSetup>();
        UIContainer = gameSetup.getPrefab("UI");
        playerActions = transform.GetComponent<PlayerActions>();
        setTemporaryUI();
    }

    private void Update()
    {
        // Get the one-time finger position when finger fist touches screen
        if (Input.GetButtonDown("Fire1"))
        {
            touchPositionInitial = Input.mousePosition;
        }

        // When you put your finger on the screen
        if (Input.GetButton("Fire1"))
        {
            touching = true;
            touchTimer += Time.fixedDeltaTime;
            touchPositionCurrent = Input.mousePosition;
            temporaryUI.gameObject.SetActive(true);

            getMovementDirection();
            getMovementSpeed();
            setPositionFollow();

            handleMovement();

        }
        else
        {

            temporaryUI.gameObject.SetActive(false);
            if (touchTimer < tapTime && touchTimer != 0)
            {
                playerActions.doAction();
                tapped = true;
                
            }

            if (touching && !tapped)
            {
                Debug.Log("Stopped dragging");
                dragDistance = 0;
            }

            touchPositionInitial = touchPositionCurrent;
            getMovementSpeed();
            handleMovement();
            touching = false;
            tapped = false;
            touchTimer = 0;
        }
    }

    private void setTemporaryUI()
    {
        temporaryUI = UIContainer.transform.Find("Canvas").transform.Find("Temporary").transform;
        dot1Transform = temporaryUI.Find("Dot1");
        dot2Transform = temporaryUI.Find("Dot2");
        dot1 = dot1Transform.GetComponent<RectTransform>();
        dot2 = dot2Transform.GetComponent<RectTransform>();
    }

    private void getMovementDirection()
    {
        Vector2 dir = touchPositionCurrent - touchPositionInitial;
        dragDirection = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    private void getMovementSpeed()
    {
        dragDistance = Vector2.Distance(touchPositionInitial, touchPositionCurrent);
    }

    private void setPositionFollow()
    {
        // These dots are temporary, delete when ready
        // Or change them into a UI element showing origin and current position
        dot1.position = touchPositionInitial;
        dot2.position = touchPositionCurrent;

        float rad = dragDirection * Mathf.Deg2Rad;
        float x = touchPositionCurrent.x - Mathf.Cos(rad) * 100;
        float y = touchPositionCurrent.y - Mathf.Sin(rad) * 100;
        dragFollow = new Vector2(x, y);

        if (dragDistance > 100)
        {
            touchPositionInitial = dragFollow;
        }
    }

    private void handleMovement()
    {
        moveDirection = sanitizeDirection();
        moveSpeed = (dragDistance * .01f);
    }

    private float sanitizeDirection()
    {
        float dir = dragDirection;
        if (dir >= -90)
        {
            dir -= 45;
        }
        else
        {
            dir += 315;
        }

        return dir * -1;
    }
}
