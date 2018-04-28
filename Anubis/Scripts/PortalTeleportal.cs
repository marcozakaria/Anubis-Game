using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleportal : MonoBehaviour {

    public Transform destination;
    public Transform destinationTrigerColider;
    public Transform player;
    //public GameObject sphereButton;
    public bool VRmode ;

    public bool spherepressed;

    private bool playerEntersTriger = false;
    

    private void Start()
    {
        if (VRmode)
        {
            GetComponent<Collider>().enabled = false;
            //SphereButtoncol = sphereButton.GetComponent<Collider>();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (playerEntersTriger || spherepressed)
        {
            if (VRmode)
            {
                Vector3 portaltoplayer = player.position - transform.position;
                Vector3 positionOffset = Quaternion.Euler(0f, 0f, 0f) * portaltoplayer * 2;  // for accurate teleporting.
                player.position = destination.position + positionOffset;
            }
            else
            {
                
                Vector3 portaltoplayer = player.position - transform.position;
                float dot = Vector3.Dot(transform.up, portaltoplayer);
                // if true player had passed the portal
                if (dot < 0f)
                {
                    Debug.Log("enter portal");
                    StartCoroutine(Mywaitseconds());

                    // teleport player
                    //float rotationDiff = -Quaternion.Angle(transform.rotation, destination.rotation);
                    //rotationDiff += 180;
                    
                    //Debug.Log(rotationDiff);
                    player.position = destination.position;
                    player.localRotation= Quaternion.Euler( 0f,180f,0f);
                    
                    playerEntersTriger = false;

                }
            }
            
        }
	}

    IEnumerator Mywaitseconds()
    {
        destinationTrigerColider.gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(10);
        destinationTrigerColider.gameObject.GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {
            playerEntersTriger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" )
        {
            playerEntersTriger = false;
        }
    }
}
