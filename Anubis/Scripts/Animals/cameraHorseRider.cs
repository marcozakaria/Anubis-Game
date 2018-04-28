using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHorseRider : MonoBehaviour {

    public float moveSpeed;
    public GameObject mycamera;

    private Camera horsecam;
    Vector3 movement;
    float h, v;

    private void Start()
    {
        horsecam = mycamera.GetComponent<Camera>();
        movement.Set(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
         h += Input.GetAxis("Mouse X");
         v += Input.GetAxis("Mouse Y");

        movement.Set( -v, h, 0f);
        //movement = movement.normalized * moveSpeed ;
        horsecam.transform.eulerAngles = movement;
        

    }
}
