using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rifle : MonoBehaviour
{

    [Header("Rifle")]
    public Camera cam;

    public float giveDamage = 10f;

    public float shootingRange = 100f;
    public float fireCharge = 15f;
    public Player player;
    public Animator animator;

    [Header("Rifle Ammunition and shooting")]
    private float nextTimeToShoot = 0f;
    private int maximumAmmunition = 30;
    private int mag = 20;
    private int presentAmmunition;
    public float reloadingTime = 1.2f;
    private bool setReloading = false;

    private void Awake()
    {
        presentAmmunition = maximumAmmunition;
    }

     [Header("Rifle effects")]
    public ParticleSystem muzzleSpark;
    public GameObject WoodedEffect;
    public GameObject bloodEffect;

    [Header("Sound Effect")]
    public AudioSource audioSource;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;

    private void Update()
    {
        if (setReloading)
            return;
        if (presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if(player.mobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButton("Shoot") && Time.time >= nextTimeToShoot)

            {
                animator.SetBool("Fire", true);
                animator.SetBool("Idle", false);
                nextTimeToShoot = Time.time + 1f / fireCharge;
                Shoot();
            }
            else if(CrossPlatformInputManager.GetButton("Shoot") &&  player.currentPlayerSpeed > 0)
            {
                animator.SetBool("Idle", false);

                animator.SetBool("FireWalk", true);

            }
            else if(CrossPlatformInputManager.GetButton("Shoot") && CrossPlatformInputManager.GetButton("Aim"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("FireWalk", true);
                animator.SetBool("Walk", true);
                animator.SetBool("Reloading", false);
            }
            else
            {
                animator.SetBool("Fire", false);
                animator.SetBool("Idle", true);
                animator.SetBool("FireWalk", false);

            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)

            {
                animator.SetBool("Fire", true);
                animator.SetBool("Idle", false);
                nextTimeToShoot = Time.time + 1f / fireCharge;
                Shoot();
            }
            else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("Idle", false);

                animator.SetBool("FireWalk", true);

            }
            else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("FireWalk", true);
                animator.SetBool("Walk", true);
                animator.SetBool("Reloading", false);
            }
            else
            {
                animator.SetBool("Fire", false);
                animator.SetBool("Idle", true);
                animator.SetBool("FireWalk", false);

            }
        }
    }
    void Shoot()

    {
        if (mag == 0)
        {
            //Show ammo out text
        }
        presentAmmunition--;
        if (presentAmmunition == 0)
        {
            mag--;
        }

        //Update UI
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurrence.UpdateMagText(mag);
        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {

            Debug.Log(hitInfo.transform.name);
            Objects objects = hitInfo.transform.GetComponent<Objects>();

            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (objects != null)
            {
                objects.objectHitDamage(giveDamage);
                GameObject WoodGo = Instantiate(WoodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(WoodGo, 1f);
            }

            else if(enemy != null)
            {
                enemy.enemyHitDamage(giveDamage);
                GameObject bloodGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodGo, 1f);
            }
        }

    }
    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);
        audioSource.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;
        setReloading = false;
    }
}
