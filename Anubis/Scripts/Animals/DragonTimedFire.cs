using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonTimedFire : MonoBehaviour {

    private ParticleSystem myParticles;

	// Use this for initialization
	void Start ()
    {
        myParticles = GetComponent<ParticleSystem>();
        StartCoroutine(FireDragon(2f));
	}

    IEnumerator FireDragon(float time)
    {
        while (true)
        {
            myParticles.Play();
            yield return new WaitForSeconds(time);
            myParticles.Stop();
            yield return new WaitForSeconds(time);
        }
        
    }

}
