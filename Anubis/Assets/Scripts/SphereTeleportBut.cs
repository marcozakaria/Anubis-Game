using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTeleportBut : MonoBehaviour {

    public PortalTeleportal myprt;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            myprt.spherepressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            myprt.spherepressed = false;
        }
    }
}
