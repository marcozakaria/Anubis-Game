using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseScript : MonoBehaviour
{
    public GameObject player;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController playercontrollerscript;
    public float speed;
    public float sprint;
    public bool VRMode;
    public Text debugText;
    public Transform cameraRigHead;

    public AudioClip MountiingHorse;
    public AudioClip MovingHorse;
    private AudioSource myAudioSource;
    public AudioSource childAudioSource;

    private Animator horseAnimator;
    private Rigidbody playerRG;
    //private Rigidbody horseRG;
    private CharacterController playerCC;
    private bool playerInrange;
    private bool playerRidingHorse;
    private Vector3 horsePosition;
    //private Vector3 horseGlobalPosition;
    private Collider horseCollider;
    private ClimbingManager climbingManagerScript;
    private Vector3 playerToHorse;
    

	// Use this for initialization
	void Start ()
    {
        horseAnimator = GetComponent<Animator>();
        horseCollider = GetComponent<Collider>();
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.clip = MovingHorse;

        playerRG = player.GetComponent<Rigidbody>();
        if (!VRMode)
        {
            //horseRG = player.GetComponent<Rigidbody>();
            playerCC = player.GetComponent<CharacterController>();
            
        }
        else // VR
        { climbingManagerScript = player.GetComponent<ClimbingManager>(); }
        
	}
	
	// Update is called once per frame
	void Update ()
    {  // player mount on the horse
        if (!playerRidingHorse && playerInrange && (Input.GetKeyDown(KeyCode.B) || ViveInput.GetButtonDown(ViveHand.Left, ViveButton.Trigger)) )
        {
            playerRidingHorse = true;
            horsePosition = this.transform.position;
            Debug.Log("Ride horse");
            PlayerRideHorse();
            horseCollider.isTrigger = false;

        }
        // player dismount from horse
        else if (playerRidingHorse && (Input.GetKeyDown(KeyCode.B) || ViveInput.GetButtonDown(ViveHand.Left, ViveButton.Trigger)) )
        {
            playerRidingHorse = false;
            playerDismountHorse();
            horseCollider.isTrigger = true;
             
        }
        else if(playerRidingHorse)  // player is on the horse
        {
            if (!VRMode)
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
                        horseAnimator.SetBool("running", true);
                        horseAnimator.SetBool("moving", false);
                        transform.position += transform.forward * vertical * speed * sprint * Time.deltaTime;
                       
                    }
                    else
                    {
                        horseAnimator.SetBool("moving", true);
                        horseAnimator.SetBool("running", false);
                        transform.position += transform.forward * vertical * speed * Time.deltaTime;
                    }
                    //myAudioSource.PlayOneShot();
                }
                else  // player not moving
                {
                    myAudioSource.Pause();
                    horseAnimator.SetBool("moving", false);
                    horseAnimator.SetBool("running", false);
                }
            }
            // VR mode
            else
            {
                debugText.text = "Player Riding horse";
                Vector2 vrMove = ViveInput.GetTouchPoint(ViveHand.Left);
                if (vrMove.magnitude >0)
                {
                    horseAnimator.SetBool("moving", true);
                    transform.Rotate(0f, vrMove.x, 0f);
                    transform.position += transform.forward * vrMove.y * speed * Time.deltaTime;

                    myAudioSource.Play();
                }
                else
                {
                    horseAnimator.SetBool("moving", false);
                    myAudioSource.Pause();
                }
            }
                       
            //transform.position += move * speed * Time.deltaTime;
           
        }
	}

    private void PlayerRideHorse()
    {
        if (!VRMode)
        {
            player.transform.SetParent(this.transform);
            playerCC.enabled = false;
            playerRG.useGravity = false;
            player.transform.position = new Vector3(horsePosition.x, horsePosition.y + 1.2f, horsePosition.z);
            playercontrollerscript.enabled = false;
            GetComponent<cameraHorseRider>().enabled = true;
        }
        else // vrmode
        {
            playerToHorse = cameraRigHead.position - transform.position;
            Debug.Log(playerToHorse);
            climbingManagerScript.enabled = false;
            debugText.text = "Player Ride Horse";
            //player.transform.position = new Vector3(horsePosition.x, horsePosition.y + 1.2f, horsePosition.z);
            player.transform.SetParent(this.transform);
            playerRG.useGravity = false;
            playerRG.isKinematic = true;
            player.GetComponent<BoxCollider>().size = new Vector3(0f,0f,0f);
            player.GetComponent<BoxCollider>().enabled = false;

            //playerRG.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            //horseRG.constraints = RigidbodyConstraints.FreezeRotation ;
            //player.transform.position = new Vector3(horsePosition.x, horsePosition.y + 1.2f, horsePosition.z) - playerToHorse;
            //player.transform.position = new Vector3(horsePosition.x + playerToHorse.x, horsePosition.y + 1f, horsePosition.z + playerToHorse.z) ;
            //player.transform.position -= new Vector3( playerToHorse.x , horsePosition.y + 1f, playerToHorse.z) ;
            // player must be in the center of the playing area to get best position
            player.transform.localPosition = new Vector3(playerToHorse.x-0.5f, 0.5f, playerToHorse.z-0.3f);

        }

        childAudioSource.PlayOneShot(MountiingHorse);
    }

    private void playerDismountHorse()
    {
        myAudioSource.Pause();

        if (!VRMode)
        {
            GetComponent<cameraHorseRider>().enabled = false;
            player.transform.SetParent(null);
            playerCC.enabled = true;
            playerRG.useGravity = true;
            playercontrollerscript.enabled = true;
            player.transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y + 1f, this.transform.position.z);
        }
        else
        {
            debugText.text = "Player Dismount Horse";
            climbingManagerScript.enabled = true;
            player.transform.SetParent(null);
            player.GetComponent<BoxCollider>().enabled = true;
            playerRG.useGravity = true;
            playerRG.isKinematic = false;
            player.GetComponent<BoxCollider>().size = new Vector3(0.5f, 1f, 0.5f);
            player.transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y + 1f, this.transform.position.z);           
        }

        horseAnimator.SetBool("moving", false);
        horseAnimator.SetBool("running", false);

        childAudioSource.PlayOneShot(MountiingHorse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {
            Debug.Log("playerInrange");
            //debugText.text = "Player in range";
            playerInrange = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"  )
        {
            Debug.Log("playerLeftRange");
            playerInrange = false;
        }
    }
}
