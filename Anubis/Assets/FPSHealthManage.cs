using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSHealthManage : MonoBehaviour {

    public int intialHealth=100;
    public int currentHealth;
    public Slider HealthBarSlider;
    public Text healthText;

    // Use this for initialization
    void Start()
    {
        currentHealth = intialHealth;
        HealthBarSlider.value = currentHealth/100;
        healthText.text =  currentHealth+"%";
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        HealthBarSlider.value = currentHealth / 100;
        healthText.text = "Health : " + currentHealth + "%";
    }
}
