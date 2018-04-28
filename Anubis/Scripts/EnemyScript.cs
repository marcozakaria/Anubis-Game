using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {

    public int health;
    public int damageAmount;
    private float currentHealth;
    public Image HealthBar;

    //private GameObject myplayer;
    public Transform player;
    public FPSHealthManage playerHealth;

    public AudioClip attack;
    public AudioClip Dead;
    private AudioSource myAudioSource;

    public ParticleSystem DeathParticles;

    private Animator myAnimator;
    private NavMeshAgent agent;
    private bool attacking;
    private bool Attacked;
    private bool dead;

    // Use this for initialization
    void Start ()
    {
        currentHealth = health;
        myAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        myAudioSource = GetComponent<AudioSource>();
       
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = GameObject.FindWithTag("gamemanager").GetComponent<FPSHealthManage>();
        //agent.stoppingDistance = 8f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!dead)
        {
            var distance = Vector3.Distance(player.position, transform.position);
           // Debug.Log(distance);
            if (distance <= 4f && !attacking)
            {
                StartCoroutine(Attack());
            }
            else if (agent.enabled && distance > 4f)
            {
                agent.destination = player.position;
            }

            if (agent.hasPath)
            {
                myAnimator.SetBool("walk", true);
            }
            //else { myAnimator.SetBool("walk", false); }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("enter");
        if (collision.gameObject.tag =="Player")
        {
            //Debug.Log("player enter");
            myAnimator.SetTrigger("attack");
        }
    }

    public IEnumerator Attack()
    {
        attacking = true;
        myAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(2f);
        if (!Attacked)
        {
            StartCoroutine(playerHealth.TakeDamage(damageAmount));
        }
        
        yield return new WaitForSeconds(4f);
        attacking = false;
    }

    public IEnumerator TakeDamage(int amount)
    {
        Attacked = true;
        currentHealth -= amount;
        if (currentHealth <=0)
        {
            HealthBar.fillAmount = 0;
            Death();
            yield return null;
        }
        else
        {
            myAnimator.SetTrigger("damage");
            HealthBar.fillAmount = (currentHealth / health);
            agent.isStopped = true;// = false;
            yield return new WaitForSeconds(2f);
            if (!dead)
            {
                agent.isStopped = false;
            }          
            
        }
        Attacked = false;
    }

    public void Death()
    {
        dead = true;
        myAudioSource.Stop();
        GetComponent<Collider>().enabled = false;
        
        Debug.Log("EnemyDead");
        myAnimator.SetTrigger("dead");
        agent.enabled = false;
        ParticleSystem partDeath =  Instantiate(DeathParticles);
        partDeath.transform.position = this.transform.position;
        Destroy(partDeath, 4f);
        Destroy(this.gameObject,5f);
        
    }
}
