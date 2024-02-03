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
    public Transform firepoint;
    public SpellInventory spellInventory;
    public float SprintSpeed;
    public float WalkSpeed;
    public float CroutchSpeed;
    public float CroutchYScale;
    private float StartYScale;
    public bool isWallRunning = false;
    public bool isJumping;
    public float basejumpForce;
    private float jumpForce;
    private bool isShooting;
    private bool isShootingAlternate;
    private float TimeBetweenShoots;
    private float TimeBetweenShootsAlternate;
    public float maxSlopeAngle;
    private Rigidbody rb;
    [SerializeField] private Camera mainCamera;
    
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
        StartYScale = transform.localScale.y;
        EntitySpeed = WalkSpeed;
        
        rb = GetComponent<Rigidbody>();
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
        jumpForce += basejumpForce;
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        rb.AddForce(moveDir.normalized * EntitySpeed * 10f, ForceMode.Force);
        




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
        Debug.Log(isWallRunning);
        if(isShooting == true)
        {
            if(TimeBetweenShoots <= 0)
            {
                if (spellInventory.SpellList.Count > 0)
                {
                    spellInventory.currentSpell.ShootSpell(firepoint);
                    TimeBetweenShoots = spellInventory.currentSpell.spell.castTime / attackspeedModifier;
                }
                else
                {
                    Debug.Log("you don't have a spell equiped");
                }
            }

        }
        if (isShootingAlternate == true)
        {
            if (TimeBetweenShootsAlternate <= 0)
            {
                if (spellInventory.SpellList.Count > 0)
                {
                    spellInventory.currentSpell2.ShootSpell(firepoint);
                    TimeBetweenShootsAlternate = spellInventory.currentSpell2.spell.castTime / attackspeedModifier;
                }
                else
                {
                    Debug.Log("you don't have a spell equiped");
                }
            }

        }
        if (TimeBetweenShoots > 0)
        {
            TimeBetweenShoots -= Time.deltaTime;
        }
        if (TimeBetweenShootsAlternate > 0)
        {
            TimeBetweenShootsAlternate -= Time.deltaTime;
        }

    }

   



}
