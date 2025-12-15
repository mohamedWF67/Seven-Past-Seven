using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("CAN MOVE")]
    public bool canMove = true;
    
    #region RUN
    
    [Header("Run Tuning")]
    
    [Tooltip("The maximum speed the player can reach.")]
    [SerializeField] float runMaxSpeed; 
    
    [Tooltip("Acceleration when player is on ground.")]
    [SerializeField] float runAcceleration; 
    
    [Tooltip("Deceleration when player is on ground.")]
    [SerializeField] float runDeceleration; 
    
    [Space(10)]
    
    [Tooltip("Acceleration when player is in the air.")]
    [Range(0.01f, 1)] public float accelInAir; 
    
    [Tooltip("Deceleration when player is in the air.")]
    [Range(0.01f, 1)] public float decelInAir;
    
    float runAccelAmount; //* Actual force for acceleration
    float runDecelAmount; //* Actual force for deceleration
    
    #endregion
    
    #region JUMP
    [Header("Jump Tuning")]
    
    [Tooltip("Exact peak height for a single jump (meters)")]
    [Range(0.1f,5f)] public float jumpHeight = 1.5f;

    [Tooltip("Extra jumps allowed in air (0 = only ground jump, 1 = double jump, etc.)")]
    public int extraAirJumps = 1;

    //! NOT WORKING.
    [Tooltip("Time you can still jump after leaving ground (seconds)")]
    [SerializeField] float coyoteTime = 0.10f;

    //! NOT WORKING.
    [Tooltip("Time an early button press is buffered (seconds)")]
    [SerializeField] float jumpBufferTime = 0.10f;

    [Tooltip("Increases gravity when jump key is released early for snappier control")]
    [SerializeField] float jumpCutGravityMultiplier = 2.0f;
    
    //! NOT WORKING.
    [Tooltip("If the user can keep on jumping without unpressing the jump key.")]
    [SerializeField] bool holdJump;
    
    [Space(10)]
    
    [Tooltip("Number of air jumps left.")]
    [SerializeField] int airJumpsLeft;
    
    [Tooltip("If the jump key is being held.")]
    [SerializeField] bool jumpHeld;
    
    [Tooltip("If jumping force is being applied.")]
    [SerializeField] bool isJumping;

    #region GROUNDING
    
    [Header("Grounding")]
    
    [Tooltip("The point that checks if the player is on the ground.")]
    [SerializeField] Transform groundCheck;
    
    [Tooltip("The radius of the point that checks if the player is on the ground.")]
    [SerializeField] float groundCheckRadius = 0.1f;
    
    [Tooltip("If the player is grounded.")]
    [SerializeField] bool isGrounded;
    
    #endregion
    
    [Space(10)]
    
    [Tooltip("Last time that the player was grounded.")]
    float lastGroundedTime = -999f;
    
    [Tooltip("Last time that the player pressed  the jump key.")]
    float lastJumpPressedTime = -999f;
    
    [Tooltip("Last time that the player has jumped.")]
    float timeSinceJump = -999f;
    
    float targetHeight; //* Player's target height's Y position
    
    float timeToApex; //* Time to apex.
    #endregion
    
    #region WALL JUMP
    [Header("Wall Jump Tuning")]
    
    [Tooltip("The needed distance from the player feet to the edge of the wall.")]
    [SerializeField,Range(0.01f,5f)] float wallDetectHeight = 1f;
    
    [Tooltip("The force applied to the player when wall jumping.")]
    [SerializeField] float WallJumpForce = 3;
    
    [Tooltip("Angle of the wall jump (degrees).")]
    [SerializeField] float _angle = 30;
    
    [Tooltip("The maximum speed that the player can fall with while being walled")]
    [SerializeField] float wallFallLimit;
    
    bool isWalled;      //* If the player is walled.
    bool isWalledRight; //* If the player is walled to the right.
    bool isWalledLeft;  //* If the player is walled to the left.
    //! USELESS.
    bool wallJump;      //* If the player is wall jumping.
    #endregion

    #region DASHING
    
    [Header("Dashing")]
    
    [Tooltip("The distance of the player's dash.")]
    [SerializeField] float dashDistance = 5;
    
    [Tooltip("If a dashing force is being applied.")]
    public bool dashing;
    
    [Tooltip("If a dash can be executed.")]
    public bool canDash = true;
    
    [Tooltip("If the player can move.")]
    [HideInInspector] public bool dashFinished = true;
    
    [Space(10)]
    
    [Tooltip("How long does the player stay dashing.")]
    [SerializeField] float dashingTime = 0.1f;
    
    [Tooltip("How long does the player stay floating after dashing ( = 0: falls, > 0: floats ).")]
    [Range(0f,1f),SerializeField] float dashingPause = 0.1f;
    
    [Tooltip("How long before the player can dash again.")]
    [SerializeField] float dashCooldown = 1f;
    #endregion
    
    #region FALL TUNING
    
    [Header("Fall Tuning")]
    
    [Tooltip("Sets the player's maximum fall velocity.")]
    [SerializeField] float fallLimit;
    #endregion

    #region EFFECTS

    [Header("Effects")]
    [Tooltip("Toggle dust particles on stop and jump.")]
    [SerializeField] private bool useParticleEffects;
    
    [Tooltip("Toggle Trail on dash.")]
    [SerializeField] private bool useTrailRenderer;
    
    private ParticleSystem dustTrail;
    private TrailRenderer TR;
    private SpriteRenderer sp;
 
    #endregion
    
    #region AUDIO
    [Header("Audio Source")]
    [Tooltip("Player's audio source.")]
    [SerializeField] AudioSource audioSource;
    [Header("Audio Clips")]
    [Tooltip("Sound of the player moving on ground")]
    [SerializeField] AudioClip moveClip;
    [Tooltip("Sound of the player jumping.")]
    [SerializeField] AudioClip jumpClip;
    [Tooltip("Sound of the player Dashing.")]
    [SerializeField] AudioClip dashClip;

    private Coroutine groundMovingCoroutine;
    #endregion

    #region ANIMATION
    [Header("Animations")]
    [Tooltip("The player's animation controller.")]
    [SerializeField] private Animator playerAnimator;

    #endregion
    
    #region EXPERIMENTATION

    [Header("Experimentation")] 
    
    #endregion
    
    #region COMPONENTS

    [Header("Components")] 
    
    Rigidbody2D rb;             //* The player's rigidbody
    PlayerInput PlayerInput;    //* the player's input
    
    #endregion
    
    #region INPUT ACTIONS
    InputAction moveAction; //* Move action { < , > }
    Vector2 _moveInput;     //* The move axis
    InputAction jumpAction; //* Jump action { Space }
    InputAction dashAction; //* Dash action { Z }
    
    #endregion
    
    #region LAYERS & TAGS
    LayerMask _groundLayer;
    #endregion

    #region ON VALIDATE
    
    private void OnValidate()
    {
        
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDecelAmount = (50 * runDeceleration) / runMaxSpeed;
        
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDeceleration = Mathf.Clamp(runDeceleration, 0.01f, runMaxSpeed);
        
        fallLimit = Mathf.Abs(fallLimit);
       if (rb != null) 
           timeToApex = TimeToApexFromHeight(jumpHeight, rb); //* Stores time to apex.
        
    }
    
    #endregion

    #region START
    
    void Start()
    {
        startComponents();
        startInputActions();
    }
    
    #endregion

    #region START INPUT
    
    void startInputActions()
    {
        moveAction = PlayerInput.actions.FindAction("Move");
        jumpAction = PlayerInput.actions.FindAction("Jump");
        dashAction = PlayerInput.actions.FindAction("Dash");
    }
    
    #endregion
    
    #region START COMPONENTS
    
    void startComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerInput = GetComponent<PlayerInput>();
        
        playerAnimator = GetComponent<Animator>();
        
        audioSource = GetComponent<AudioSource>();
        
        dustTrail = GetComponentInChildren<ParticleSystem>();
        TR = GetComponentInChildren<TrailRenderer>();
        sp = GetComponent<SpriteRenderer>();
        
        _groundLayer = LayerMask.GetMask("Ground");
    }
    
    #endregion

    #region UPDATE
    void Update()
    {
        updateActions();
        checkCollisions();
        if (canMove && !GameManagerScript.instance.inUI)
        {
            Jump();
            Flip();
        }
    }

    #endregion

    #region UPDATE ACTIONS
    
    void updateActions()
    {
        _moveInput = moveAction.ReadValue<Vector2>();
        jumpHeld = jumpAction.IsPressed();
    }
    
    #endregion
    
    #region COLLISION CHECKS
    
    private void checkCollisions()
    {   //* Checks if the player is near the upper edge of a wall.
        bool raycastHit = Physics2D.Raycast(transform.position + Vector3.down * 1f + (isWalledRight? Vector3.right : Vector3.left) * 0.5f, Vector2.up, wallDetectHeight, _groundLayer);
        //* Checks if the player is touching a wall to the right or left.
        isWalledRight = Physics2D.OverlapCircle(transform.position + Vector3.down * 0.4f  + Vector3.right * 0.4f, groundCheckRadius, _groundLayer);
        isWalledLeft = Physics2D.OverlapCircle(transform.position + Vector3.down * 0.4f  + Vector3.left * 0.4f, groundCheckRadius, _groundLayer);
        //* Checks if the player is on the ground or walled.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, _groundLayer) || (raycastHit && (isWalledLeft || isWalledRight));
        
        //Debug.Log($"rayCastHit: {raycastHit} isWalledRight: {isWalledRight} isWalledLeft: {isWalledLeft}");
        
        isWalled = (isWalledLeft || isWalledRight) && !isGrounded;//* Checks if the player is walled.
        
        if (isGrounded)
        {   //* Reset the air jumps left.
            lastGroundedTime  = Time.time;
            airJumpsLeft = extraAirJumps;
        }
    }
    
    #endregion
    
    #region JUMP ACTION
    
    private void Jump()
    {
        if (jumpHeld) 
            lastJumpPressedTime = Time.time; //* Stamps the jump time.
        
        bool canCoyote = Time.time - lastGroundedTime <= coyoteTime; //* Compares time since last grounded to time. 
        bool bufferedJump = Time.time - lastJumpPressedTime <= jumpBufferTime; //* Compares time since jump to time. 
        
        //* The player can jump if he is on ground or in air and airJumpsLeft >= 1.
        if (bufferedJump && jumpAction.WasPressedThisFrame() && (isGrounded || canCoyote || airJumpsLeft > 0 || isWalled))
        {   //* Checks which direction the player is walled to.
            int dir = isWalledLeft && !isWalledRight ? 1: -1;
            
            //* If the player is in the air, use 1 air jump.
            if(!isGrounded && !isWalled) airJumpsLeft--;
            
            if (isWalled)
            {   //* If walled and !grounded, execute wall jump.
                StartCoroutine(WallJumpCoroutine(dir));
                //* Play a sound when jumping.
                SoundFXManagerScript.instance.Play3DSFXSound(jumpClip,transform);
                
                return; //* Stops so it doesn't execute a normal jump.
            }
            
            isJumping = true;
            targetHeight = rb.position.y + jumpHeight;  //* Calculates target height Y position.
            CreateDustTrail();//* play Dust effect.
            SoundFXManagerScript.instance.Play3DSFXSound(jumpClip,transform);//* Play jump Sound.
            timeSinceJump = Time.time;                  //* Stamps time since the jump key was pressed.
            timeToApex = TimeToApexFromHeight(jumpHeight, rb); //* Stores time to apex.
        }
        //* Checks if the player has reached the apex.
        bool reachedApex = Time.time > timeSinceJump + timeToApex;
        
        //* Cancels jumping force if the jump key is unpressed or the player has reached the apex.
        if (!jumpHeld || rb.position.y > targetHeight || reachedApex) 
            isJumping = false;

        //* Executes jump.
        if (isJumping)
            DoJump();

        //* Slows down the player after jump.
        if (!jumpHeld && rb.linearVelocity.y > 0f && !dashing)
        {
            float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale; //* Get the G force acting on the player.
            //* This emulates higher gravity while rising after release.
            rb.AddForce(Vector2.down * g * (jumpCutGravityMultiplier - 1f) * rb.mass * Time.deltaTime, ForceMode2D.Force);
        }
    }
    
    #endregion

    #region DO JUMP 

    void DoJump()
    {
        //* Get the G force acting on player.
        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        //* Get the initial force acting on player.
        float v0 = Mathf.Sqrt(2f * g * jumpHeight);
        
        //* Resets the players Y velocity if falling.
        if (rb.linearVelocity.y < 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        
        //* Calculate the force required for the jump.
        float impulse = rb.mass * (v0 - rb.linearVelocity.y);
        rb.AddForce(Vector2.up * impulse, ForceMode2D.Impulse);
    }    

    #endregion
    
    #region  WALL JUMP
    
    IEnumerator WallJumpCoroutine(int dir)
    {
        wallJump = true;
        //* Zero Velocity.
        rb.linearVelocity = Vector2.zero;
        //* Calculate angle.
        float angle = _angle * Mathf.Deg2Rad;
        //* Calculate gravity.
        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        //* Calculate initial velocity magnitude.
        float v0 = Mathf.Sqrt((jumpHeight * g) / Mathf.Sin(2 * angle));
        //* convert velocity to force (impulse = mass * velocity)
        Vector2 velocity = new Vector2(Mathf.Cos(angle) * dir, Mathf.Sin(angle)) * v0;
        Vector2 impulse = velocity * rb.mass;
        //* Apply force to player.
        rb.AddForce(impulse * WallJumpForce, ForceMode2D.Impulse);
        //! Needs to be tested.
        float time = (2f * v0 * Mathf.Sin(angle))  / g;
        if (time >= 0.00f)
            yield return new WaitForSeconds(time);
        wallJump = false;
    }

    #endregion

    #region TIME TO APEX
    
    float TimeToApexFromHeight(float height, Rigidbody2D rb)
    {
        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        return Mathf.Sqrt(2f * height / g);
    }
    
    #endregion
    
    #region FIXED UPDATE
   
    void FixedUpdate()
    {
        if (canMove && !GameManagerScript.instance.inUI)
            dash();
        run();
        fall();

        PlayerAnimation();
        PlayerSounds();
    }

    #endregion

    #region RUN
    
    private void run()
    {
        //* End speed.
        
        float moveInputX = 0;
        if (canMove && !GameManagerScript.instance.inUI)
            moveInputX = _moveInput.x;
        float targetSpeed = moveInputX * runMaxSpeed;
        
        #region Calcualte acceleration

        float acceleration;
        if (isGrounded)
            acceleration = Mathf.Abs(targetSpeed) >0.01f ? runAccelAmount : runDecelAmount;
        else
            acceleration = Mathf.Abs(targetSpeed) >0.01f ? runAccelAmount * accelInAir : runDecelAmount * decelInAir;
        
        #endregion
        
        float speedDif = targetSpeed - rb.linearVelocityX;
        if (dashing) return;
        //* Force needed to move player.
        float movement = speedDif * acceleration;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        
        //* If the player switches direction add dust
        if (Mathf.Sign(rb.linearVelocity.normalized.x) != Mathf.Sign(_moveInput.normalized.x) && Mathf.Abs(rb.linearVelocityX) > 0.05f && isGrounded)
            CreateDustTrail();
    }
    
    #endregion
    
    #region DASH
    void dash()
    {
        if (dashAction.IsPressed() && canDash && !dashing && dashFinished)
            StartCoroutine(doDash());
    }

    IEnumerator doDash()
    {
        //* Play Dash Sound.
        SoundFXManagerScript.instance.Play3DSFXSound(dashClip,transform);
        dashFinished = false;
        dashing = true;
        
        TR.emitting = true; //* Enable the trail renderer.
        //* Zero G.
        float oldGravity = rb.gravityScale;
        rb.gravityScale = 0f;                   
        rb.linearVelocity = Vector2.zero;
        //* Normalizes the direction to the local transform if no move input.
        Vector2 dir = _moveInput.normalized;
        if (_moveInput == Vector2.zero)
        {
            dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }
        //* Splits the movement to physics update steps.
        int steps = Mathf.Max(1, Mathf.RoundToInt(dashingTime / Time.fixedDeltaTime));
        float stepDist = dashDistance / steps;
        for (int i = 0; i < steps; i++) {
            rb.MovePosition(rb.position + dir * stepDist);
            yield return new WaitForFixedUpdate();
        }              
        //* optional pause
        if (dashingPause > 0f)
            yield return new WaitForSeconds(dashingPause);

        //* Restores gravity
        rb.gravityScale = oldGravity;
        dashing = false;
        
        TR.emitting = false;//* Disable the trail renderer.
        
        if (dashCooldown > 0f)
            yield return new WaitForSeconds(dashCooldown);
        dashFinished = true;
    }
    
    #endregion
    
    #region FALL
    
    private void fall()
    {
        //* Checks if there is a fall limit and adjust the speed to it.
        if (fallLimit <=0) return;
        float finalFallLimit = (isWalled && _moveInput.y >= 0 ? -wallFallLimit :-fallLimit) + 0.49f;
        if (rb.linearVelocityY  < finalFallLimit)
            rb.linearVelocityY = finalFallLimit;
    }
    
    #endregion
    
    #region FLIP PLAYER
    //TODO: needs to be remodeled.
    private void Flip()
    {
        sp.flipX = _moveInput.x < 0;
    }

    #endregion
    
    #region SOUNDS
    void PlayerSounds()
    {
        //TODO Change to a more advanced way.
        if (isGrounded && _moveInput.x != 0)
        {
            if (groundMovingCoroutine != null) return;
            groundMovingCoroutine = StartCoroutine(MovingSound());
        }else if (groundMovingCoroutine != null)
        {
            StopCoroutine(groundMovingCoroutine);
            groundMovingCoroutine = null;
        }
    }

    IEnumerator MovingSound()
    {
        while (true)
        {
            audioSource.PlayOneShot(moveClip);
            yield return new WaitForSeconds(moveClip.length);
        }
    }
    #endregion
    
    #region ANIMATION
   
    void PlayerAnimation()
    {
        playerAnimator.SetBool("IsRunning",isGrounded && _moveInput.x != 0);
        playerAnimator.SetBool("IsJumping",!isGrounded && !dashing);
        //playerAnimator.SetBool("IsWallJumping",wallJump);
        playerAnimator.SetBool("IsDashing",dashing);
    }
    #endregion
    
    #region DUST EFFECT
    void CreateDustTrail()
    {
        if (!useParticleEffects) return;
        dustTrail.Play();
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            //* Draws the ground check sphere visually.
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            //* Draws the jump height.
            Gizmos.color = Color.blue;
            Vector3 targetVectorStart2 = new Vector3(transform.position.x - 0.5f, targetHeight, transform.position.z);
            Vector3 targetVectorEnd2 = new Vector3(transform.position.x + 0.5f, targetHeight, transform.position.z);
            Gizmos.DrawLine(targetVectorStart2, targetVectorEnd2);
            //* Draws the wall check sphere visually distance.
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.4f  + Vector3.right * 0.4f * transform.localScale.x, groundCheckRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.4f + Vector3.left * 0.4f * transform.localScale.x, groundCheckRadius);
            //* Draws the wall's edge check line.
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position + Vector3.down * 1f + (isWalledRight? Vector3.right : Vector3.left) * 0.51f, transform.position + Vector3.down * 1f + (isWalledRight? Vector3.right : Vector3.left) * 0.51f + Vector3.up * wallDetectHeight);
        }
    }
}
