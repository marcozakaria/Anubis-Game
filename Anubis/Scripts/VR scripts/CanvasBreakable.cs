using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBreakable : MonoBehaviour {

    // future plans.
    // Switch between camerarig and player prephap VR
    public GameObject camerarig;
    public GameObject playerobj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        playerobj.transform.position = camerarig.transform.position;
        playerobj.transform.rotation = camerarig.transform.rotation;

        if (Input.GetKeyDown(KeyCode.Z) )
        {
            camerarig.SetActive (!camerarig.activeSelf);
            playerobj.SetActive(!playerobj.activeSelf);

        }
		
	}
}
