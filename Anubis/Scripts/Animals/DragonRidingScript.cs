using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonRidingScript : MonoBehaviour {

    public GameObject player;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController playercontrollerscript;
    public float speed;
    public float FlyMagnitude;

    public AudioClip DragonSound2;
    public AudioClip DragonSound1;
    private AudioSource myAudioSource;

    private Animator DragonAnimator;
    private Rigidbody playerRG;
  
    private CharacterController playerCC;
    private bool playerInrange;
    private bool playerRidingHorse;
    private Vector3 DragonPosition;
 
    private Collider DragonCollider;
    //private Rigidbody DragonRG;

    void Start()
    {
        DragonAnimator = GetComponent<Animator>();
        DragonCollider = GetComponent<Collider>();
        myAudioSource = GetComponent<AudioSource>();
        //DragonRG = GetComponent<Rigidbody>();

        playerRG = player.GetComponent<Rigidbody>();
        playerCC = player.GetComponent<CharacterController>();

    }

    void Update()
    {  // player mount on the horse
        if (Input.GetKeyDown(KeyCode.B) && !playerRidingHorse && playerInrange )
        {
            playerRidingHorse = true;
            DragonPosition = this.transform.position;
            PlayerRideDragon();
            DragonCollider.isTrigger = false;

        }
        // player dismount from horse
        else if (playerRidingHorse && Input.GetKeyDown(KeyCode.B) )
        {
            playerRidingHorse = false;
            playerDismountDragon();
            DragonCollider.isTrigger = true;

        }
        else if (playerRidingHorse)  // player is on the horse
        {
            
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                var move = new Vector3(0, 0f, vertical * 3);
                transform.Rotate(0f, horizontal, 0f);

                if (move.magnitude > 0) // player moving
                {
                    if (!myAudioSource.isPlaying)
                    {
                        myAudioSource.Play();
                    }

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //Fly dragon up.
                        transform.position += transform.up * FlyMagnitude * Time.deltaTime;                        
                    }
                    else if (Input.GetKey(KeyCode.LeftControl) )
                    {
                        //fly dragon down
                        transform.position -= transform.up * FlyMagnitude * Time.deltaTime; 
                    }
                    else
                    {                      
                        transform.position += transform.forward * vertical * speed * Time.deltaTime;
                    }
                    //myAudioSource.PlayOneShot();
                }
                else  // player not moving
                {
                    // stuck in the air
                    myAudioSource.Pause();
                    
                }
        }
    }

    private void PlayerRideDragon()
    {
        
        player.transform.SetParent(this.transform);
        playerCC.enabled = false;
        playerRG.useGravity = false;
        player.transform.position = new Vector3(DragonPosition.x, DragonPosition.y + 1.5f, DragonPosition.z-1f);
        playercontrollerscript.enabled = false;
        //player.transform.Rotate(0f, 180f, 0f);
        GetComponent<cameraHorseRider>().enabled = true;
        
    }

    private void playerDismountDragon()
    {
        myAudioSource.Pause();

            GetComponent<cameraHorseRider>().enabled = false;
            player.transform.SetParent(null);
            playerCC.enabled = true;
            playerRG.useGravity = true;
            playercontrollerscript.enabled = true;
            player.transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y + 1f, this.transform.position.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInrange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInrange = false;
        }
    }

}
