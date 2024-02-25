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
    private void Start()
    {
        mainCamera = Camera.main;
    }

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
        Debug.Log("Collision Detected");
        if (collision.collider.CompareTag("Enemy"))
        {
            Entity entity = collision.collider.GetComponent<Entity>();
            if (entity != null)
            {
                entity.DamageRecieve(ProjectileDamage);
                Debug.Log("Damaging enemy " + ProjectileDamage);
            }
        }

        // Calculate explosion point based on the point of collision
        explosionPoint = collision.contacts[0].point;

        ApplyEffect();
        Destroy(gameObject);
    }
    
    private void Update()
    {
        FindTarget();
        RotateTowardNearestEnemy();
        MoveTowardsTarget();
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
            if (collider.CompareTag("Enemy"))
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
}
