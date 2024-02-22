using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : Projectile
{
    public ParticleSystem ExplosionParticles;

    private Entity targetToFollow;
    private Vector3 targetPosition;
    public float detectionRange = 20f;
    private float rotationspeed = 50;
    
    public override void ApplyEffect()
    {
        Instantiate(ExplosionParticles, transform.position + Vector3.up * 2.5f, Quaternion.identity);

        ExplosionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision Detected");
        if (other.CompareTag("Enemy"))
        {

            Entity entity = other.GetComponent<Entity>();
            
            if (entity != null)
            {
                entity.DamageRecieve(ProjectileDamage);
            }



        }
        ApplyEffect();
        Destroy(gameObject);
    }
    private void Update()
    {
        FindTarget();
        RotateTowardNearestEnemy();
        MoveTowardsTarget();
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
