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

    public LayerMask portalLayer;
    public LayerMask ReflectLayer;

    public float explosionRadius = 5f; 
    public int explosionDamage = 50;    

    public override void ApplyEffect()
    {
        
        var explosionInstance = Instantiate(ExplosionParticles, explosionPoint, Quaternion.identity);

        
        explosionInstance.transform.LookAt(Camera.main.transform);

        Destroy(explosionInstance.gameObject, explosionInstance.main.duration);

        
        DamageNearbyEnemies();
    }

    private void DamageNearbyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(explosionPoint, explosionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(Target))
            {
                Entity entity = hitCollider.GetComponent<Entity>();
                if (entity != null)
                {
                    entity.DamageRecieve(explosionDamage);
                    Debug.Log("Damaging " + Target + ": " + explosionDamage);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (portalLayer == (portalLayer | (1 << collision.gameObject.layer)))
        {
            return;
        }
        else if (ReflectLayer == (ReflectLayer | (1 << collision.gameObject.layer)))
        {
            return;
        }
        else
        {
            explosionPoint = collision.contacts[0].point;

            ApplyEffect();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Tracking)
        {
            RedoTarget();
            FindTarget();
            RotateTowardNearestEnemy();
            MoveTowardsTarget();
        }
        Acceleration();
        if (hasalifeTime)
        {
            if (LifeTime > 0)
            {
                LifeTime -= Time.deltaTime;
            }
            else
            {
                Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
