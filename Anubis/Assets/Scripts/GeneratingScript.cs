using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratingScript : MonoBehaviour {

    public GameObject prephap;
    public Transform newTransform;

    private MeshRenderer meshrender;

    private void Start()
    {
        meshrender = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GeneratePrephap();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            meshrender.material.color = Color.red;
            GeneratePrephap();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        meshrender.material.color = Color.white;
    }

    private void GeneratePrephap()
    {
        Debug.Log("Child count : " + newTransform.childCount);
        if (newTransform.childCount < 1)
        {  // if it has no child instantiate object 
            GameObject sword = Instantiate(prephap, newTransform);  // sword is new object
            sword.transform.localPosition = Vector3.zero;
            sword.transform.localRotation = Quaternion.identity;
        }
        else
        {  // if it has childs destroy child then regenerate it
            Destroy(newTransform.GetChild(0).gameObject);
            GameObject sword = Instantiate(prephap, newTransform);  // sword is new object
            sword.transform.localPosition = Vector3.zero;
            sword.transform.localRotation = Quaternion.identity;
        }
        
    }
}
