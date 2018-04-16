using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour {

    public float damage;
    public float range; // max distance to hit
    public bool canZoom;
    private bool zoomed;

    public AudioClip gunFireClip;
    public ParticleSystem fireParticles;
    public GameObject hitFlare;

    public int maxAmmo;
    public int maxPreservedAmmo;
    public float reloadTime=1f;
    public Text ammoCountText;
    public Text ammoPreservedText;
    private int currentAmmo;
    private int currentPreservrdAmmo;
    private bool isReloading;
    public AudioClip reloadindClip;
    public Animator reloadingAnimation;

    public Camera FPScam;

    private AudioSource gunFireSource;

    private void Start()
    {
        gunFireSource = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
        currentPreservrdAmmo = maxPreservedAmmo;
        ammoCountText.text = currentAmmo.ToString();
        ammoPreservedText.text = currentPreservrdAmmo.ToString();
    }

    private void OnEnable()
    {
        isReloading = false;
        reloadingAnimation.SetBool("reload", false);

    }

    private void OnDisable()
    {
        if (zoomed)
        {
            zoomed = false;
            FPScam.fieldOfView = 60;
            this.transform.localPosition = new Vector3(0.47f, -0.42f, 0.545f);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if ( Input.GetKeyDown(KeyCode.R) )
        {
            if (isReloading)
            {
                return;
            }
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0)
        {
            Shoot();
            ammoCountText.text = currentAmmo.ToString();
        }
        else if (canZoom && Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (zoomed)
            {
                zoomed = false;
                FPScam.fieldOfView = 60;
                this.transform.localPosition = new Vector3(0.47f, -0.42f, 0.545f);
            }
            else
            {
                zoomed = true;
                FPScam.fieldOfView = 40;
                this.transform.localPosition = new Vector3(0f, -0.42f, 0.3f);
            }
            
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        gunFireSource.PlayOneShot(reloadindClip);
        reloadingAnimation.SetBool("reload", true);
        yield return new WaitForSeconds(reloadTime);
        currentPreservrdAmmo -= ( maxAmmo - currentAmmo);
        ammoPreservedText.text = currentPreservrdAmmo.ToString();
        currentAmmo = maxAmmo;
        ammoCountText.text = currentAmmo.ToString();
        reloadingAnimation.SetBool("reload", false);
        isReloading = false;
    }

    private void Shoot()
    {
        currentAmmo--;
        reloadingAnimation.SetTrigger("fire");
        gunFireSource.PlayOneShot(gunFireClip);
        fireParticles.Play();
        RaycastHit hitInfo;
        if(Physics.Raycast(FPScam.transform.position, FPScam.transform.forward, out hitInfo, range) )
        {
            Debug.Log(hitInfo.transform.name);
            GameObject flareobj = Instantiate(hitFlare, hitInfo.point,Quaternion.LookRotation(hitInfo.normal));
            Destroy(flareobj, 2f);

            
            if (hitInfo.transform.tag =="breakable")
            {
                BreakableObjects target = hitInfo.transform.GetComponent<BreakableObjects>();
                StartCoroutine(target.breakableAttacked());
            }
            else if (true)
            {

            }
            
        }
    }
}
