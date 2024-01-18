using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;
    private float DefaultDrag = 1f;
    public float groundDrag;
    public GameObject player;
    public static Player Instance { get; private set; }
    [SerializeField] private GameInput gameInput;
    private float Speed;
    public float SprintSpeed;
    public float WalkSpeed;
    public float CroutchSpeed;
    public float CroutchYScale;
    private float StartYScale;
    private bool IsWallRunning;
    public float jumpForce = 5.0f; 
    
    private Rigidbody rb;
    [SerializeField] private Camera mainCamera;
    
    public Transform orientation;
    

    
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
        Speed = WalkSpeed;
        transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
    }

    private void GameInput_OnCroutch(object sender, System.EventArgs e)
    {
        Speed = CroutchSpeed;
        transform.localScale = new Vector3(transform.localScale.x, CroutchYScale, transform.localScale.z);
    }

    private void GameInput_OnStopSprint(object sender, System.EventArgs e)
    {
        Speed = WalkSpeed;
    }

    private void GameInput_OnStartSprint(object sender, System.EventArgs e)
    {
        Speed = SprintSpeed;
    }

    private void GameInput_Onjump(object sender, System.EventArgs e)
    {
        if (isGrounded) 
        {
            Jump();
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
    private bool CanWallRun()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, transform.right, out hit, 1.0f) && hit.collider.CompareTag("Wall") ||
               Physics.Raycast(transform.position, -transform.right, out hit, 1.0f) && hit.collider.CompareTag("Wall");
    }


    private void Update()
    {
        HandleMovement();
        isGrounded = Physics.Raycast(transform.position, Vector3.down,playerHeight * 0.5f + 0.2f, whatIsGround);
        if (isGrounded)
        {
            rb.drag = groundDrag;

        }
        else
        {
            rb.drag = DefaultDrag;
        }
        if (CanWallRun())
        {
            Debug.Log("Can wall run");
        }
    }

    
}
