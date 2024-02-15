using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public bool isGrounded;
    private float DefaultDrag = 1f;
    public float groundDrag;
    public GameObject player;
    
    public static Player Instance { get; private set; }
    [SerializeField] private GameInput gameInput;
    public Transform Leftfirepoint;
    public Transform Rightfirepoint;
    public SpellInventory spellInventory;
    public float SprintSpeed;
    public float WalkSpeed;
    public float CroutchSpeed;
    public float CroutchYScale;
    private float StartYScale;
    public bool isWallRunning = false;
    public bool isJumping;
    public float basejumpForce;
    public float jumpForce;
    private bool isShooting;
    private bool isShootingAlternate;
    private float TimeBetweenShoots;
    private float TimeBetweenShootsAlternate;
    public float maxSlopeAngle;
    private RaycastHit slopeHIt;
    private Rigidbody rb;
    public float InitialAirForceDownTimer;
    private float AirForceDownTimer;
    private float ForceDownIncrease = 0;
    [SerializeField] private Camera mainCamera;
    private Vector3 moveDir;
    
    public Transform orientation;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air,
        Croutch,
        wallrunning
    }

    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        jumpForce = basejumpForce;
        gameInput.Onjump += GameInput_Onjump;
        gameInput.OnStartSprint += GameInput_OnStartSprint;
        gameInput.OnStopSprint += GameInput_OnStopSprint;
        gameInput.OnCroutch += GameInput_OnCroutch;
        gameInput.OnstopCroutch += GameInput_OnstopCroutch;
        gameInput.OnShoot += GameInput_OnShoot;
        gameInput.OnStopShoot += GameInput_OnStopShoot;
        gameInput.OnShootAlternate += GameInput_OnShootAlternate;
        gameInput.OnStopShootAlternate += GameInput_OnStopShootAlternate;
        gameInput.OnSwitch += GameInput_OnSwitch;
        gameInput.OnSwitchAlternate += GameInput_OnSwitchAlternate;
        StartYScale = transform.localScale.y;
        EntitySpeed = WalkSpeed;
        if (Instance == null)
        {
            
            Instance = this;
        }

        rb = GetComponent<Rigidbody>();
    }

    private void GameInput_OnSwitchAlternate(object sender, System.EventArgs e)
    {
        spellInventory.SpellSwitch(1);
    }

    private void GameInput_OnSwitch(object sender, System.EventArgs e)
    {
        spellInventory.SpellSwitch(0);
    }
    

    private void GameInput_OnStopShootAlternate(object sender, System.EventArgs e)
    {
        isShootingAlternate = false;
    }

    private void GameInput_OnShootAlternate(object sender, System.EventArgs e)
    {

        isShootingAlternate = true;
        Debug.Log("shoot alternate " + isShootingAlternate);
    }

    private void GameInput_OnStopShoot(object sender, System.EventArgs e)
    {
        isShooting = false;
        
    }

    private void GameInput_OnShoot(object sender, System.EventArgs e)
    {
        
        isShooting = true;
        
    }

    private void GameInput_OnstopCroutch(object sender, System.EventArgs e)
    {
        state = MovementState.sprinting;
    }

    private void GameInput_OnCroutch(object sender, System.EventArgs e)
    {
        state = MovementState.Croutch;
    }

    private void GameInput_OnStopSprint(object sender, System.EventArgs e)
    {
        state = MovementState.walking;
    }

    private void GameInput_OnStartSprint(object sender, System.EventArgs e)
    {
        state = MovementState.sprinting;
    }

    private void GameInput_Onjump(object sender, System.EventArgs e)
    {
        if (isGrounded) 
        {
            Jump();
            isJumping = true;
            state = MovementState.air;
        }
    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    public void changeJumpForce(float Force)
    {
        jumpForce += Force;
    }
    public void ResetJumpForce()
    {
        jumpForce = basejumpForce;
    }
    //slope stuff


    private void HandleMovement()
    {
        
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        moveDir = orientation.forward * inputVector.y + orientation.right * inputVector.x;
        moveDir.y = 0;

        
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection(moveDir) * EntitySpeed * 10f, ForceMode.Force);
            
        }
        else
        {
            rb.AddForce(moveDir.normalized * EntitySpeed * 10f, ForceMode.Force);
            
        }
        //rb.useGravity = !OnSlope();

        if (!isGrounded)
        {
            float forceDown = 1 + ForceDownIncrease;
            Debug.Log("ForceDown " + forceDown);
            rb.AddForce(Vector3.down * forceDown, ForceMode.Force);
            if (AirForceDownTimer <= 0)
            {
                AirForceDownTimer = InitialAirForceDownTimer;
                ForceDownIncrease += 0.25f;
            }
        }
        else if (isGrounded ||isWallRunning)
        {
            ForceDownIncrease = 0;
        }
        if(AirForceDownTimer > 0&& !isGrounded)
        {
            AirForceDownTimer -= Time.deltaTime;
        }
    }
    private void stateHandler()
    {
        switch (state)
        {
            case MovementState.walking:
                if (isWallRunning)
                {
                    state = MovementState.wallrunning;
                }
                transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
                EntitySpeed = WalkSpeed;
                break;

            case MovementState.sprinting:
                if (isWallRunning)
                {
                    state = MovementState.wallrunning;
                }
                transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
                EntitySpeed = SprintSpeed;
                break;
            case MovementState.air:
                if (isWallRunning)
                {
                    state = MovementState.wallrunning;
                }
                rb.drag = DefaultDrag;
                break;
            case MovementState.Croutch:
                if (isWallRunning)
                {
                    state = MovementState.wallrunning;
                }
                EntitySpeed = CroutchSpeed;
                transform.localScale = new Vector3(transform.localScale.x, CroutchYScale, transform.localScale.z);
                break;
            case MovementState.wallrunning:
                EntitySpeed = SprintSpeed;
                rb.drag = groundDrag / 2;
                break;
        }
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down,out slopeHIt, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHIt.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        Vector3 slopeDirection = Vector3.ProjectOnPlane(direction, slopeHIt.normal).normalized;
        Debug.Log(slopeDirection);
        return slopeDirection;
    }

    private void Update()
    {
        
        stateHandler();
        
        HandleMovement();
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        

        if (isGrounded)
        {
            
            rb.drag = groundDrag;
            isJumping = false;
            

        }
        else if (!isGrounded)
        {

        }
        Debug.Log(isWallRunning);
        if (isShooting)
        {
            Leftfirepoint.GetComponentInChildren<SpellHandVisualEffect>().StartIncreasingParticles();

            if (TimeBetweenShoots <= 0)
            {
                if (spellInventory.SpellList.Count > 0)
                {
                    Leftfirepoint.GetComponentInChildren<SpellHandVisualEffect>().StopAdjustingParticles();
                    spellInventory.currentSpell.ShootSpell(Leftfirepoint);
                    TimeBetweenShoots = spellInventory.currentSpell.spell.castTime / attackspeedModifier;
                }
                else
                {
                    Debug.Log("You don't have a spell equipped");
                }
            }
        }
        else
        {
            Leftfirepoint.GetComponentInChildren<SpellHandVisualEffect>().StartDecreasingParticles();
        }

        
        if (!isShooting || TimeBetweenShoots > 0)
        {
            Leftfirepoint.GetComponentInChildren<SpellHandVisualEffect>().StopAdjustingParticles();
        }
        if (isShootingAlternate)
        {
            Rightfirepoint.GetComponentInChildren<SpellHandVisualEffect>().StartIncreasingParticles();

            if (TimeBetweenShootsAlternate <= 0)
            {
                if (spellInventory.SpellList.Count > 0)
                {
                    Rightfirepoint.GetComponentInChildren<SpellHandVisualEffect>().StopAdjustingParticles();
                    spellInventory.currentSpell2.ShootSpell(Rightfirepoint);
                    TimeBetweenShootsAlternate = spellInventory.currentSpell2.spell.castTime / attackspeedModifier;
                }
                else
                {
                    Debug.Log("You don't have a spell equipped");
                }
            }
        }
        else
        {
            Rightfirepoint.GetComponentInChildren<SpellHandVisualEffect>().StartDecreasingParticles();
        }

        
        if (!isShootingAlternate || TimeBetweenShootsAlternate > 0)
        {
            Rightfirepoint.GetComponentInChildren<SpellHandVisualEffect>().StopAdjustingParticles();
        }
        if (TimeBetweenShootsAlternate > 0)
        {
            TimeBetweenShootsAlternate -= Time.deltaTime;
        }
        if (TimeBetweenShoots > 0)
        {
            TimeBetweenShoots -= Time.deltaTime;
        }


    }

   



}
