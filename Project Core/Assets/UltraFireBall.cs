using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraFireBall : Projectile
{
    [SerializeField] private FireBallProjectile fireBall;
    private Vector3 explosionPoint;
    private Entity targetToFollow;
    
    public float detectionRange = 300f;
    private float rotationspeed = 50;
    public override void ApplyEffect()
    {
        int numFireballs = 10; // Number of fireballs to shoot out
        float explosionRadius = 5f; // Radius of the explosion sphere

        for (int i = 0; i < numFireballs; i++)
        {
            // Calculate a random direction around the explosion point
            Vector3 randomDirection = Random.onUnitSphere;

            // Calculate a random position within the explosion sphere
            Vector3 randomPosition = explosionPoint + randomDirection * Random.Range(0f, explosionRadius);

            // Instantiate a fireball at the calculated position
            FireBallProjectile fireballInstance = Instantiate(fireBall, randomPosition, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.collider.CompareTag(Target))
        {
            Entity entity = collision.collider.GetComponent<Entity>();
            if (entity != null)
            {
                entity.DamageRecieve(ProjectileDamage);
                Debug.Log("Damaging " + Target + ": " + ProjectileDamage);
                explosionPoint = collision.contacts[0].point;
                ApplyEffect();
                Destroy(gameObject);
            }
        }
       

    }
    private void Update()
    {
        if (Tracking == true)
        {
            FindTarget();
            RotateTowardNearestEnemy();
            MoveTowardsTarget();
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
}
