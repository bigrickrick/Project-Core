using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    bool isGrounded;
    private float DefaultDrag = 1f;
    public float groundDrag;
    public GameObject player;
    private Vector3 wallRunDirection;
    public static Player Instance { get; private set; }
    [SerializeField] private GameInput gameInput;
    private float Speed;
    public float SprintSpeed;
    public float WalkSpeed;
    public float CroutchSpeed;
    public float CroutchYScale;
    private float StartYScale;
    public bool isWallRunning = false;
    public bool isJumping;
    public float jumpForce = 5.0f;

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

        gameInput.Onjump += GameInput_Onjump;
        gameInput.OnStartSprint += GameInput_OnStartSprint;
        gameInput.OnStopSprint += GameInput_OnStopSprint;
        gameInput.OnCroutch += GameInput_OnCroutch;
        gameInput.OnstopCroutch += GameInput_OnstopCroutch;
        StartYScale = transform.localScale.y;
        Speed = WalkSpeed;
        rb = GetComponent<Rigidbody>();
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


    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        rb.AddForce(moveDir.normalized * Speed * 10f, ForceMode.Force);
        




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
                Speed = WalkSpeed;
                break;

            case MovementState.sprinting:
                if (isWallRunning)
                {
                    state = MovementState.wallrunning;
                }
                transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
                Speed = SprintSpeed;
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
                Speed = CroutchSpeed;
                transform.localScale = new Vector3(transform.localScale.x, CroutchYScale, transform.localScale.z);
                break;
            case MovementState.wallrunning:
                Speed = SprintSpeed;
                rb.drag = groundDrag / 2;
                break;
        }
    }

   

    private void Update()
    {
        
        stateHandler();
        Debug.Log("speed " + Speed);
        HandleMovement();
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        

        if (isGrounded)
        {
            
            rb.drag = groundDrag;
            isJumping = false;
            

        }
        Debug.Log(isWallRunning);
        
    }

   



}
