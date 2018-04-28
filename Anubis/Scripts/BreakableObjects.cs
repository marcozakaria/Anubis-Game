using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjects : MonoBehaviour {

    public GameObject breakablePieces;
    public GameObject MainComponent;
    public float DestroyTime = 10f;
    public AudioSource breakClip;

    private Collider mycol;

	// Use this for initialization
	void Start ()
    {
        breakClip = GetComponent<AudioSource>();
        mycol = GetComponent<Collider>();  // to avoid multible hits. 
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
           /*
            MainComponent.SetActive(false);
            breakablePieces.SetActive(true);
            breakClip.Play();
            Destroy(this.gameObject, DestroyTime);  */
            StartCoroutine(breakableAttacked());
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "weapon" || collision.gameObject.tag == "Player")
        {
            StartCoroutine(breakableAttacked());
        }
    }
    
    public IEnumerator breakableAttacked()
    {
        mycol.enabled = false;
        MainComponent.SetActive(false);
        var pieces = Instantiate(breakablePieces,this.transform);
        pieces.SetActive(true);
        breakClip.Play();
        yield return new WaitForSeconds(DestroyTime);
        Destroy(pieces);
        //Destroy(this.gameObject);
        MainComponent.SetActive(true);
        mycol.enabled = true;
    }
}
