using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponcontrolerFPS : MonoBehaviour {
    //Switch between two weapons with scrool wheel

    public GameObject weapon1;
    public GameObject weapon2;

	// Update is called once per frame
	void Update ()
    {
       
        if (Input.GetAxis("Mouse ScrollWheel") <0f || Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchWeapon();
        }

	}

    void SwitchWeapon()
    {
        weapon1.SetActive(!weapon1.activeSelf);
        weapon2.SetActive(!weapon2.activeSelf);
    }
}
