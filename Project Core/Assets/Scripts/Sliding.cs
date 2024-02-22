using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    public Transform orientation;
    public Transform playerobj;
    private Rigidbody rb;
    private Player pm;

    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    float slideJumpTimer = 0.5f;

    public bool sliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Player>();

        startYScale = playerobj.localScale.y;
    }

    private void StartSlide()
    {
        sliding = true;

        playerobj.localScale = new Vector3(playerobj.localScale.x, slideYScale, playerobj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;

    }

    private void StopSlide()
    {
        sliding = false;
        playerobj.localScale = new Vector3(playerobj.localScale.x, startYScale, playerobj.localScale.z);
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

       
        if(!pm.OnSlope()|| rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            if (pm.isGrounded)
            {
                slideTimer -= Time.deltaTime;
            }
            
        }
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce*2f, ForceMode.Force);
        }
        

        if (slideTimer <=0)
        {
            StopSlide();
        }
    }
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(slideKey)&& (horizontalInput !=0 || verticalInput != 0))
        {
            slideJumpTimer -= Time.deltaTime;
            StartSlide();
            if (slideJumpTimer <= 0)
            {
                slideJumpTimer = 0.5f;
                
            }
        }
        if(Input.GetKeyUp(slideKey) && sliding)
        {
            StopSlide();
            

        }


    }

    private void FixedUpdate()
    {
        if (sliding)
        {
            SlidingMovement();
        }
    }
}
