using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    [Header("Player Health Things")]
    private float playerHealth = 100f;
    private float presentHealth;
    public HealthBar healthBar;

    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float currentPlayerSpeed = 0f;
    public float playerSprint = 3f;
    public float currentPlayerSprint = 0f;
    public Transform Spawn;
    public Transform PlayerCharacter;

    [Header("Player Camera")]
    public Transform playerCamera;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jumping & velocity")]
    public float jumpRange = 1f;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    public bool mobileInputs;
    public FixedJoystick joystick;
    public FixedJoystick Sprintjoystick;

    public Score score;



    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
    }
    void Update()
    {
        if(currentPlayerSpeed > 0)
        {
            Sprintjoystick = null;
        }
        else
        {
            FixedJoystick sprintJS = GameObject.Find("PlayerSprintJoystick").GetComponent<FixedJoystick>();
            Sprintjoystick = sprintJS;
        }
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);

        playerMove();

        Jump();

        Sprint();
    }

    void playerMove()
    {
       if(mobileInputs == true)
        {
            float horizontal_axis = joystick.Horizontal;
            float vertical_axis = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false);
                animator.SetBool("Idle", false);
                animator.SetTrigger("Jump");
                animator.SetBool("AimWalk", false);
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
                currentPlayerSpeed = playerSpeed;

            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetTrigger("Jump");
                animator.SetBool("Walk", false);
                animator.SetBool("Running", false);
                animator.SetBool("AimWalk", false);
                currentPlayerSpeed = 0f;
            }
        }
        else
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false);
                animator.SetBool("Idle", false);
                animator.SetTrigger("Jump");
                animator.SetBool("AimWalk", false);
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
                currentPlayerSpeed = playerSpeed;

            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetTrigger("Jump");
                animator.SetBool("Walk", false);
                animator.SetBool("Running", false);
                animator.SetBool("AimWalk", false);
                currentPlayerSpeed = 0f;
            }
        }
    }
    void Jump()
    {

        if (mobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump") && onSurface)
            {
                animator.SetBool("Walk", false);
                animator.SetTrigger("Jump");
                velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
            }
            else
            {
                animator.ResetTrigger("Jump");
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && onSurface)
            {
                animator.SetBool("Walk", false);
                animator.SetTrigger("Jump");
                velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
            }
            else
            {
                animator.ResetTrigger("Jump");
            }
        }
    }

    void Sprint()
    {
        if (mobileInputs == true)
        {
            float horizontal_axis = Sprintjoystick.Horizontal;
            float vertical_axis = Sprintjoystick.Vertical;

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
              
                
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;

            }
            else
            {
                animator.SetBool("Idle", false);
               
                animator.SetBool("Walk", false);
                
                currentPlayerSpeed = 0f;
            }
        }
        else
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                
                
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;

            }
            else
            {
                animator.SetBool("Idle", false);
              
                animator.SetBool("Walk", false);
              
                currentPlayerSpeed = 0f;
            }
        }
    }
    //playerhitdamage
    public void TeamMatesHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);

        if(presentHealth <=0)
        {
            StartCoroutine(Respawn());
        }
    }
    //playerdie

     IEnumerator Respawn()
    {
      
        playerSpeed = 0f;
        playerSprint = 0f;

       // animator.SetBool("Die", true);
       

        Debug.Log("Dead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        score.enemyKills += 1;

        yield return new WaitForSeconds(1f);

        Debug.Log("Spawn");
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        presentHealth = 100f;
        playerSpeed = 1.9f;
         playerSprint = 3f;


    //animation
   // animator.SetBool("Die", false);
        /*animator.SetBool("Walk", false);
        animator.SetBool("Running", false);
        animator.SetBool("Idle", true);
        animator.SetTrigger("Jump");
        animator.SetBool("AimWalk", false);
        animator.SetBool("IdleAim", true);*/



        //spawn point
        PlayerCharacter.transform.position = Spawn.transform.position;
     }
}
