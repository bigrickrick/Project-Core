using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;


    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private Player pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Player>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.isWallRunning)
        {
            WallRunningMovement();
        }
            
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        
        Debug.Log("wall left " + wallLeft + " wall right " + wallRight);
        Debug.Log("verticale input " + verticalInput);
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround()&&!exitingWall)
        {
            if (!pm.isWallRunning)
            {
                StartWallRun();
            }
            if(Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }
                
        }
        else if (exitingWall)
        {
            if (pm.isWallRunning)
            {
                StopWallRun();
            }
            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            if(exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }

        
        else
        {
            if (pm.isWallRunning)
            {
                StopWallRun();
            }
                
        }
    }

    private void StartWallRun()
    {
        pm.isWallRunning = true;

        cam.Dofov(90f);
        if (wallLeft)
        {
            cam.DoTilt(-5f);

        }
        if (wallRight)
        {
            cam.DoTilt(5f);
        }
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
            

        
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        
        if (upwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        }
          
        if (downwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
        }
           

        
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
            
    }

    private void StopWallRun()
    {
        pm.isWallRunning = false;
        rb.useGravity = true;

        cam.Dofov(80f);
        cam.DoTilt(0f);
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
