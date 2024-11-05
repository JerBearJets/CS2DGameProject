using UnityEngine;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
    //----------------
    //Define Variables
    //----------------
    //Components

    private Rigidbody2D playerRigidbody;

    private Animator anim;

    public Transform mouseFollowTarget;

    private CharacterHealth characterHealth;

    // Audio

    [SerializeField] private AudioSource dodgeAudio;

    [SerializeField] private AudioSource walkAudio;

    [SerializeField] private AudioSource missAudio;

    [SerializeField] private AudioSource hitAudio;

    //Player Movement

    private Vector3 moveDirection;

    public float playerSpeed;

    public float turnSpeed;

    //Stamina

    [SerializeField] private Slider stamina;

    private float staminaStartTime = 0f;

    private float staminaTimer = 0f;

    private float staminaDelay = 1.0f;

    private float staminaIncreaseTimer = 0f;

    //Dodge

    private Vector3 dodgeDirection;

    private Vector3 lastDodgeDirection;

    private float dodgeSpeed;

    //Attack

    [SerializeField] private Slider chargeAttack;

    private float attackStartTime = 0f;

    private const float attackHoldTime = 1.0f;

    private bool attackHeld = false;

    private bool chargingAttack = false;

    //Attack Logic

    private RaycastHit2D[] attackHit;

    [SerializeField] private LayerMask attackable;

    [SerializeField] private Transform attackTransform;

    [SerializeField] private float attackRange = 4.5f;

    private const float attackDamage = 1f;

    public bool isAttacking = false;

    //KnockedBack

    private float knockBackTimer = 0f;

    //Player State
    private enum State
    {
        Normal,
        Dodging,
        Attacking,
        KnockedBack,
    }

    private State state;

    //Field of View

    [SerializeField] private FieldOfView fieldOfView;

    private Vector3 fovDir;
    
    //----------------
    //End of Variables
    //----------------
    


    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        characterHealth = GameObject.Find("Player").GetComponent<CharacterHealth>();

        stamina = GameObject.Find("Stamina").GetComponent<Slider>();

        chargeAttack = GameObject.Find("ChargeAttack").GetComponent<Slider>();

        state = State.Normal;
    }



    private void Update()
    {
        if (characterHealth.isKnockedBack != Vector3.zero)
        {
            attackHeld = false;

            chargingAttack = false;

            isAttacking = false;

            anim.SetTrigger("trPlayerIdle");

            state = State.KnockedBack;
        }

        switch (state)
        {
            //Player is in Normal State

            case State.Normal:

                //Stamina Regen Timer, Increased Stamina by 5 every 1 Second after Delay

                if (!chargingAttack && stamina.value < 100)
                {
                    staminaTimer += Time.deltaTime;

                    if (staminaTimer >= (staminaStartTime + staminaDelay))
                    {
                        staminaIncreaseTimer += Time.deltaTime;

                        if (staminaIncreaseTimer >= staminaDelay)
                        {
                            staminaIncreaseTimer = 0f;

                            stamina.value = Mathf.Min(stamina.value + 5, 100);
                        }
                    }
                }   else
                {
                    staminaTimer = 0f;

                    staminaIncreaseTimer = 0f;
                }


                //Checks Player Input and Moves the Sprite

                float moveX = Input.GetAxisRaw("Horizontal");

                float moveY = Input.GetAxisRaw("Vertical");

                moveDirection = new Vector3(moveX, moveY).normalized;

                if (moveDirection != Vector3.zero)
                {
                    anim.SetBool("boPlayerWalk", true);

                    lastDodgeDirection = moveDirection;
                }   
                else
                {
                    anim.SetBool("boPlayerWalk", false);
                }

                //Checks if Player has Stamina and pressed Dodge

                if (stamina.value >= 5)
                {

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        dodgeAudio.Play();

                        stamina.value -= 5;

                        dodgeDirection = lastDodgeDirection;

                        dodgeSpeed = 200f;

                        state = State.Dodging;
                    }
                }

                //Checks if Player pressed Attack

                if (Input.GetMouseButtonDown(0) && stamina.value >=5)
                {
                    stamina.value -= 5;

                    chargeAttack.value = 0;

                    attackStartTime = Time.time;

                    chargingAttack = true;

                    attackHeld = false;
                }

                //If Attack Button is being held down

                if (Input.GetMouseButton(0) && chargingAttack && !attackHeld)
                {
                    float timeHeld = Time.time - attackStartTime;

                    chargeAttack.value = timeHeld;

                    if (timeHeld >= attackHoldTime  )
                    {
                        attackHeld = true;
                    }
                }

                //If Attack Button is Released
                if (Input.GetMouseButtonUp(0) && chargingAttack)
                {
                    chargeAttack.value = 0;

                    chargingAttack = false;

                    //If Attack Button was held for required time
                    if (attackHeld)
                    {
                        isAttacking = true;

                        anim.SetTrigger("trPlayerAttack");

                        Attack();

                        state = State.Attacking;

                    }   

                    else
                    {
                        //If Attack Button wasn't held for required time
                        staminaStartTime = Time.time;
                    }
                }

                //Sets Character Sprite to follow Mouse Target

                Vector3 fovDir = (mouseFollowTarget.transform.position - transform.position).normalized;

                //Converts Vector3 to Angle

                float angle = Mathf.Atan2(fovDir.y, fovDir.x) * Mathf.Rad2Deg;

                //Grabs the Desired End Rotation Based on Mouse Target Position

                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

                //Rotates the Player in steps to Desired End Rotation

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                //Passes current Player Direction to Field of View Script

                float currentAngle = transform.eulerAngles.z;

                fieldOfView.SetFoVDirection(currentAngle);

                break;

            //Player is Currently Dodging

            case State.Dodging:

                //Decreases the Dodge Speed over time while Dodging

                float dodgeSpeedDropMultiplier = 10f;

                dodgeSpeed *= 1 - (dodgeSpeedDropMultiplier * Time.deltaTime);

                //When Dodge Speed reaches set value player returns to normal Walking

                float dodgeSpeedMinimum = 50f;

                if (dodgeSpeed < dodgeSpeedMinimum)
                {
                    staminaTimer = staminaStartTime = Time.time;

                    state = State.Normal;
                }
                break;

            //Player is Currently Attacking

            case State.Attacking:

                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    anim.SetTrigger("trPlayerIdle");

                    attackHeld = isAttacking = chargingAttack = false;

                    staminaTimer = staminaStartTime = Time.time;

                    state = State.Normal;
                }
                break;

            // Player is Currently Stunned

            case State.KnockedBack:

                //Increases Knockback Timer

                knockBackTimer += Time.deltaTime;

                //Applies Knockback to Player
                if (knockBackTimer <= .3f)
                {
                    //Decays Knockback force gradually

                    characterHealth.isKnockedBack *= 1 - (1f * Time.deltaTime);
                }   
                else
                {
                    //Resets Knockback and Timer

                    characterHealth.isKnockedBack = Vector3.zero;

                    knockBackTimer = 0f;

                    state = State.Normal;
                }
                break;
        }

        //Ensures the FoV cone follows the player

        fieldOfView.SetOrigin(transform.position);

    }


    

    private void FixedUpdate()
    {

        //Nulls out Angular Velocity

        playerRigidbody.angularVelocity = 0f;

        switch (state)
        {
            case State.Normal:

                //Converts the Inputs into Velocity for the Rigidbody and modifies it by set Player Speed
                
                playerRigidbody.linearVelocity = moveDirection * playerSpeed;

                break;

            case State.Dodging:

                //Converts the last Inputs into Velocity for the Rigidbody and modifies it by Dodge Speed

                playerRigidbody.linearVelocity = dodgeDirection * dodgeSpeed;

                break;

            case State.Attacking:

                //Prevents the Player from Moving while Attacking

                playerRigidbody.linearVelocity = moveDirection * 0f ;

                break;

            case State.KnockedBack:

                playerRigidbody.AddForce(characterHealth.isKnockedBack, ForceMode2D.Impulse);

                break;
        }
    }


    public void playWalkSound()
    {
        walkAudio.Play();
    }

    public void playSwingSound()
    {
        missAudio.Play();
    }

    //Preforms the Attack Scan and if it Hits it deals Damage
    private void Attack()
    {

        //Check to see if Attack hit Attackable Collider within Range

        attackHit = Physics2D.CircleCastAll(attackTransform.position, attackRange, Vector3.zero, 0f, attackable);

        foreach (var hit in attackHit)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>(); 

            if(damageable != null)
            {
                hitAudio.Play();
                damageable.Damage(attackDamage, Vector3.zero);
            }
            
        }
    }
}
