using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingManager : MonoBehaviour
{
    // manager to avoid unity synchorization problems between left & right. attach to camera rig.

    public GameObject body; // for the cameraRig

    public Climbing right;
    public Climbing left;

    Rigidbody rg;
    bool isGriped;

    // Use this for initialization
    void Start()
    {
        rg = body.GetComponent<Rigidbody>(); //add rigidBody to the cameraRig, Extra:Box collider.
    }

    // Update is called once per frame for rigidbody
    void FixedUpdate()
    {
        var lhand = left.hand;
        var rhand = right.hand;
        isGriped = left.canGrip || right.canGrip;
        //Debug.Log("Climbing: Left -> " + left.canGrip + ", Right -> " + right.canGrip);

        //Debug.Log("Climbing: isGriped -> " + isGriped);
        if (isGriped)
        {
            if (left.canGrip && ViveInput.GetButtonState(lhand, ViveButton.Grip))
            {
               // Debug.Log("Climbing: Left Hand enterd");

                rg.useGravity = false;
                rg.isKinematic = true;
                //make body move when you move your hands.
                body.transform.position += (left.prevPosition - left.transform.localPosition);
                left.prevPosition = left.controller.transform.localPosition;
                //Debug.Log(body.transform.position);
                //Debug.Log((left.prevPosition - left.transform.localPosition));
            }
            else if (left.canGrip && ViveInput.GetButtonUp(lhand, ViveButton.Grip))
            {
                Debug.Log("Climbing: Left Hand leaved");

                rg.useGravity = true;
                rg.isKinematic = false;
                rg.velocity = (left.prevPosition - left.transform.localPosition) / Time.deltaTime;
            }

            if (right.canGrip && ViveInput.GetButtonState(rhand, ViveButton.Grip))
            {
                //Debug.Log("Climbing: Right Hand enterd");

                rg.useGravity = false;
                rg.isKinematic = true;
                //make body move when you move your hands.
                body.transform.position += (right.prevPosition - right.transform.localPosition);
                right.prevPosition = right.controller.transform.localPosition;
                //Debug.Log(body.transform.position);
            }
            else if (right.canGrip && ViveInput.GetButtonUp(rhand, ViveButton.Grip))
            {
                Debug.Log("Climbing: Right Hand leaved");

                rg.useGravity = true;
                rg.isKinematic = false;
                rg.velocity = (right.prevPosition - right.transform.localPosition) / Time.deltaTime;
            }
        }
        else
        {
            rg.useGravity = true;
            rg.isKinematic = false;

        }

        left.prevPosition = left.transform.localPosition;
        right.prevPosition = right.transform.localPosition;
    }
}
