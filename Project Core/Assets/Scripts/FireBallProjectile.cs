using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : Projectile
{
    public ParticleSystem ExplosionParticles;

    private Entity targetToFollow;
    
    public float detectionRange = 20f;
    private float rotationspeed = 50;
    private float LifeTime = 5;
    private Camera mainCamera;
    private Vector3 explosionPoint;
   
    public bool Tracking;
    public LayerMask portalLayer;


    public override void ApplyEffect()
    {
        // Instantiate explosion particles
        var explosionInstance = Instantiate(ExplosionParticles, explosionPoint, Quaternion.identity);
        
        // Rotate the explosion towards the camera
        explosionInstance.transform.LookAt(Camera.main.transform);

        ExplosionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        
        if (collision.collider.CompareTag(Target))
        {
            Entity entity = collision.collider.GetComponent<Entity>();
            if (entity != null)
            {
                entity.DamageRecieve(ProjectileDamage);
                Debug.Log("Damaging "+Target+": "+ ProjectileDamage);
            }
        }
        
        if (portalLayer == (portalLayer | (1 << collision.gameObject.layer)))
        {
            
            return;
        }
        else
        {
            explosionPoint = collision.contacts[0].point;

            ApplyEffect();
            Destroy(gameObject);
        }
        // Calculate explosion point based on the point of collision
        
    }
    
    private void Update()
    {
        if(Tracking == true)
        {
            FindTarget();
            RotateTowardNearestEnemy();
            MoveTowardsTarget();
        }
        Acceleration();
        if(LifeTime > 0)
        {
            LifeTime -= Time.deltaTime;

        }
        else
        {
            Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDistance = Mathf.Infinity;
        targetToFollow = null;
        
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(Target))
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetToFollow = collider.GetComponent<Entity>();
                }
            }
        }
    }
    private void RotateTowardNearestEnemy()
    {
        if (targetToFollow != null)
        {
            Vector3 direction = (targetToFollow.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationspeed);
        }
    }
    private void MoveTowardsTarget()
    {
        if (targetToFollow != null)
        {
            transform.Translate(Vector3.forward * ProjectileSpeed * Time.deltaTime);
        }
    }
    private void Acceleration()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 newVelocity = rb.velocity + transform.forward * Time.deltaTime * 2;
        rb.velocity = Vector3.ClampMagnitude(newVelocity, ProjectileSpeed);
        
    }
}
