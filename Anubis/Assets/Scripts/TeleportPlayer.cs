using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TeleportPlayer : MonoBehaviour {

    public LineRenderer linePrephap;
    // Dictionary for multible arrow fire .
    public Dictionary<Arrow, LineRenderer> arrowLines = new Dictionary<Arrow, LineRenderer>();

	// Use this for initialization
	void Start () {
        Arrow.OnArrowRelease += Arrow_OnArrowRelease;
        Arrow.OnArrowLanded += Arrow_OnArrowLanded;
	}

    private void Arrow_OnArrowRelease(Arrow me)
    {
        LineRenderer line = Instantiate(linePrephap);
        line.positionCount = 0;

        arrowLines.Add(me, line);
    }

    private void Arrow_OnArrowLanded(Arrow me)
    {
        LineRenderer l = arrowLines[me];
        arrowLines.Remove(me);       
        Destroy(me.gameObject);

        StopAllCoroutines(); // to avoid problems when launching to arrows.
        StartCoroutine(arrowTeleport(l));
    }

    IEnumerator arrowTeleport(LineRenderer l)
    {
        Vector3[] pos = new Vector3[l.positionCount];
        l.GetPositions(pos);
        for (int i = 0; i < pos.Length; i+=5)  // variable i to control speed.
        {
            Player.instance.transform.position = l.GetPosition(i);
            yield return null;
        }
        
        Destroy(l);
    }


    // Update is called once per frame
    void Update ()
    {
        foreach (Arrow arrow in arrowLines.Keys)
        {
            arrowLines[arrow].positionCount++;
            arrowLines[arrow].SetPosition(arrowLines[arrow].positionCount - 1, arrow.transform.position);
        }
	}
}
