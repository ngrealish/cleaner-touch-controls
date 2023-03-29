using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameSetup : MonoBehaviour
{

    public GameObject pawn;
    [HideInInspector] public GameObject cameraRig;
    private GameObject UIContainer;
    

    private void Awake()
    {
        setRequired();
        cameraRig.GetComponent<CameraManager>().target = pawn.transform;

        VehicleMovement[] vehicles = Resources.FindObjectsOfTypeAll<VehicleMovement>();
        foreach (VehicleMovement vehicle in vehicles)
        {
            vehicle.enabled = false;
        }
    }

    private void setRequired()
    {
        pawn = getPrefab("Pawns/Farmer");
        cameraRig = getPrefab("CameraRig");
        UIContainer = getPrefab("UI");
    }

    public GameObject getPrefab(string prefab)
    {
        GameObject go = GameObject.Find(prefab);
        if (!go) {
            go = Instantiate(Resources.Load("prefabs/" + prefab, typeof(GameObject)) as GameObject);
            go.name = prefab;
        }
        return go;
    }
}