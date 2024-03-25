using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    public Transform orientation;
    public Transform playerobj;
    private Rigidbody rb;
    private Player pm;
    public bool IsSuperSliding;
    public LayerMask EnemyLayer;
    public float SuperSlideHurtBox;
    public float superslideknockback;
    public int superslideDamage;
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
        IsSuperSliding = false;
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
        if (pm.isGrounded)
        {
           
        }
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            slideJumpTimer -= Time.deltaTime;
            StartSlide();
            if (slideJumpTimer <= 0)
            {
                slideJumpTimer = 0.5f;

            }
        }
        if (Input.GetKeyUp(slideKey) && sliding)
        {
            StopSlide();


        }



    }

    private void FixedUpdate()
    {
        if (sliding)
        {
            
            SlidingMovement();
            if (IsSuperSliding)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, SuperSlideHurtBox, EnemyLayer);

                foreach (Collider col in colliders)
                {
                    Rigidbody rb = col.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        Vector3 direction = transform.position + col.transform.position;
                        rb.AddForce(direction.normalized * superslideknockback * Time.fixedDeltaTime);

                        Entity entity = col.GetComponent<Entity>();
                        if(entity != null)
                        {
                            entity.DamageRecieve(superslideDamage);
                        }



                    }

                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SuperSlideHurtBox);
    }
}
