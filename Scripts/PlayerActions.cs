using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    private GameSetup gameSetup;
    public List<string> availableActions;
    
    // Temporary:
    private GameObject vehicle;
    private GameObject human;

    private void Awake()
    {
        gameSetup = GetComponent<GameSetup>();
        availableActions = new List<string>();
        vehicle = GameObject.Find("Vehicles");
    }

    private void Update()
    {
        
    }

    public void doAction()
    {
        Debug.Log("Do the action.  It will just be enter/exit vehicle for now");
        toggleVehicle();
    }

    private void toggleVehicle()
    {
        if (gameSetup.pawn.GetComponent<HumanMovement>())
        {
            Debug.Log("Ok, so how do I switch this to that vehicle");
            GameObject oldPawn = gameSetup.pawn;
            human = oldPawn;
            GameObject newPawn = vehicle;

            oldPawn.transform.position = newPawn.transform.position;
            oldPawn.SetActive(false);

            gameSetup.pawn = newPawn;
            // newPawn.GetComponent<VehicleMovement>().enabled = true;
            setCameraTarget(newPawn);
        } else if (gameSetup.pawn.GetComponent<VehicleMovement>())
        {
            Debug.Log("Ok, get out of the vehicle");
            GameObject oldPawn = gameSetup.pawn;
            GameObject newPawn = human;
            Transform exitPoint = oldPawn.transform.Find("Trucks").transform.Find("ExitPoint").transform;

            newPawn.transform.position = exitPoint.position;
            newPawn.SetActive(true);

            gameSetup.pawn = newPawn;
            oldPawn.GetComponent<VehicleMovement>().enabled = false;
            setCameraTarget(newPawn);
        }
    }

    private void setCameraTarget(GameObject pawn)
    {
        Debug.Log(pawn);
        gameSetup.cameraRig.GetComponent<CameraManager>().target = pawn.transform; 
    }
}
