using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{

    public GameObject body; // for the cameraRig

    public ViveHand hand;
    public SteamVR_TrackedObject controller;

    public Vector3 prevPosition;
    [HideInInspector]
    public bool canGrip;

    // Use this for initialization
    void Start()
    {
        prevPosition = controller.transform.localPosition;
        //Debug.Log(prevPosition);
        //prevPosition = controller.transform.position;
    }


    // add a trigger collider to the controller model && add rigidbody to controller to detect onTrigerEnter make it is kinametic.
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Climbing: TriggerEnter -> " + other.gameObject.name);
        if (other.gameObject.tag == "grap")
        {
            //Debug.Log("Climbing: Level 2 " + other.gameObject.name);

            
            canGrip = true;
            Debug.Log("Climbing: Level 2 canGrip: " + canGrip);

            //Debug.Log(canGrip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "grap")
        {
            canGrip = false;
        }
    }
}
