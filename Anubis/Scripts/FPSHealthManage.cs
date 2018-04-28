using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSHealthManage : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject PauseUiPanel;

    public int intialHealth=100;
    public int currentHealth;
    public Image damageImage;
    public Slider HealthBarSlider;
    public Text healthText;
    public Text ScoreText;

    public float MaxTimeInAir;
    private float timeInAir;
    public CharacterController PlayerCharacterController;
    public GameObject WeaponHolder;
    public static int GameScore = 0;

    // Use this for initialization
    void Start()
    {
        currentHealth = intialHealth;
        HealthBarSlider.value = currentHealth/100f;
        healthText.text =  currentHealth+"%";
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        // take damage when failing from high places.
        if (!PlayerCharacterController.isGrounded)
        {
            timeInAir += Time.deltaTime;
            if (timeInAir >= MaxTimeInAir)
            {
                StartCoroutine( TakeDamage(10) );
                timeInAir = 0f;
            }
        } else if(PlayerCharacterController.isGrounded) { timeInAir = 0f; }
        ScoreText.text = "Score : " + GameScore;
	}
    // enumeratr for damage image
    public IEnumerator TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            GameOver();
            HealthBarSlider.value = 0f;
            healthText.text = "Health : 0%";
            damageImage.enabled = true;
            yield return null;
        }
        else
        {
            damageImage.enabled = true;
            HealthBarSlider.value = (currentHealth / 100f);
            //Debug.Log(HealthBarSlider.value);
            healthText.text = "Health : " + currentHealth + "%";
            yield return new WaitForSeconds(0.5f);
            damageImage.enabled = false;
        }
        
    }
    
    // player Death
    public void GameOver()
    {
        PlayerCharacterController.enabled = false;
        Debug.Log("GameOver");
    }

    void Pause()
    {
        WeaponHolder.SetActive(false);
        PauseUiPanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Resume()
    {
        WeaponHolder.SetActive(true);
        PauseUiPanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

}
