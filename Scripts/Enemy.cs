using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Health and Damage")]
    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float enemySpeed;

    [Header("Enemy Things")]
    public NavMeshAgent enemyAgent;
    public Transform LookPoint;
    public GameObject ShootingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    public Transform Spawn;
    public Transform EnemyCharacter;

    [Header("Enemy Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;

    [Header("Enemy animation and Spark effect")]
    public Animator anim;
    public ParticleSystem muzzleSpark;

    [Header("Enemy States")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;
    public bool isPlayer = false;

    public Score score;

    [Header("Sound Effect")]
    public AudioSource audioSource;
    public AudioClip shootingSound;

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        presentHealth = enemyHealth;

    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);

        if (playerInvisionRadius && !playerInshootingRadius) Pursueplayer();
        if (playerInvisionRadius && playerInshootingRadius) ShootPlayer();

    }



    private void Pursueplayer()
    {
        if (enemyAgent.SetDestination(playerBody.position))
        {

            anim.SetBool("Running", true);
            anim.SetBool("Shooting", false);

        }
        else
        {
            anim.SetBool("Running", false);
            anim.SetBool("Shooting", false);
        }
    }

    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);
        if (!previouslyShoot)

        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);
            RaycastHit hit;

            if (Physics.Raycast(ShootingRaycastArea.transform.position, ShootingRaycastArea.transform.forward, out hit, shootingRadius))

            {
                Debug.Log("Shooting" + hit.transform.name);

                if(isPlayer == true)
                {
                    Player playerBody = hit.transform.GetComponent<Player>();
                    if (playerBody != null)
                    {
                        playerBody.TeamMatesHitDamage(giveDamage);
                    }
                }
               else
                {
                    TeamMates playerBody = hit.transform.GetComponent<TeamMates>();
                    if (playerBody != null)
                    {
                        playerBody.TeamMatesHitDamage(giveDamage);
                    }
                }

                anim.SetBool("Running", false);
                anim.SetBool("Shooting", true);


            }

          
            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);

        }


        
    }
    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if (presentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInvisionRadius = false;
        playerInshootingRadius = false;
        anim.SetBool("Die", true);
        anim.SetBool("Running", false);
        anim.SetBool("Shooting", false);

        Debug.Log("Dead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        score.kills += 1;

        yield return new WaitForSeconds(5f);

        Debug.Log("Spawn");

        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        presentHealth = 120f;
        enemySpeed = 1f;
        shootingRadius = 10f;
        visionRadius = 100f;
        playerInvisionRadius = true;
        playerInshootingRadius = false;

        //animation
        anim.SetBool("Die", false);
        anim.SetBool("Shooting", true);

        //spawn point
        EnemyCharacter.transform.position = Spawn.transform.position;
        Pursueplayer();
    }
}


