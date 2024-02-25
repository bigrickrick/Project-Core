using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    public Player pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;
    public int maxdashCharges;
    public int dashCharges;
    [Header("CameraEffects")]
    public PlayerCam cam;
    public float dashFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;
    public float tiltAngle = 20f;
    [Header("Cooldown")]
    public float dashCd;
    public float dashCdTimer;

   

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashCharges = maxdashCharges;
    }

    private void Update()
    {
        if (dashCharges < maxdashCharges)
        {
            if (dashCdTimer > 0)
                dashCdTimer -= Time.deltaTime;
            else
            {
                dashCharges++;
                dashCdTimer = dashCd;
            }
            
        }

    }
    
    public void dash()
    {
        if(dashCharges > 0)
        {
            dashCharges -= 1;
            cam.Dofov(dashFov);

            Transform forwardT;

            if (useCameraForward)
                forwardT = playerCam; /// where you're looking
            else
                forwardT = orientation; /// where you're facing (no up or down)

            Vector3 direction = GetDirection(forwardT);

            Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

            if (disableGravity)
                rb.useGravity = false;
            float tiltAmount = Vector3.Dot(direction.normalized, forwardT.right) * tiltAngle;

            cam.DoTilt(-tiltAmount); 


            delayedForceToApply = forceToApply;
            Invoke(nameof(DelayedDashForce), 0.025f);

            Invoke(nameof(ResetDash), dashDuration);
        }
        else
        {
            return;
        }
        

        
        

        
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f); 

        Vector3 newVelocity = new Vector3(delayedForceToApply.x, rb.velocity.y, delayedForceToApply.z);
        rb.velocity = newVelocity;
    }

    private void ResetDash()
    {

        pm.dashing = false;

        cam.Dofov(85f);

        if (disableGravity)
            rb.useGravity = true;
        
        pm.state = Player.MovementState.walking;
        cam.DoTilt(0f);
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
