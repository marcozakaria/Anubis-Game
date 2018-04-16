using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayer : MonoBehaviour {

    public ViveHand hand;
    public GameObject myhand;
    public GameObject weaponHolder;

    Collider[] mycol;
    Collider trrigercol;

    GameObject parent;
   

	// Use this for initialization
	void Start ()
    {
        trrigercol = GetComponent<Collider>();
	}

    // Update is called once per frame
    void Update()
    {

        if (ViveInput.GetButtonDown(hand, ViveButton.Trigger))
        {
            trrigercol.enabled = false;

            mycol =  Physics.OverlapSphere(myhand.transform.position, 0.05f );
            
            if (mycol!=null)
            {
                foreach (Collider col in mycol)
                {
                    if (col.gameObject.tag == "weapon" && col.GetComponent<Rigidbody>()!=null)
                    {
                        col.GetComponent<Rigidbody>().isKinematic = true;
                        parent = col.transform.parent.gameObject;
                        col.gameObject.transform.SetParent(myhand.transform);
                        col.gameObject.transform.rotation = myhand.transform.rotation;
                        col.gameObject.transform.Rotate(60, 0, 0);
                    }
                    

                }
            }         
        }

        else if (ViveInput.GetButtonUp(hand, ViveButton.Trigger))
        {
            if (mycol != null)
            {
                foreach (Collider col in mycol)
                {
                    if (col.GetComponent<Rigidbody>() != null && col.gameObject.tag == "weapon")
                    {
                        col.GetComponent<Rigidbody>().isKinematic = false;
                        col.gameObject.transform.SetParent(  parent.transform);
                        col.GetComponent<Rigidbody>().velocity = ViveInput.GetVelocity(hand);
                        col.GetComponent<Rigidbody>().angularVelocity = ViveInput.GetAngularVelocity(hand);
                        Destroy(col.gameObject, 3f);
                    }

                }
            }

            trrigercol.enabled = true;
        }



    }
}
