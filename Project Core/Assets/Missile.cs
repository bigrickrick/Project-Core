using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    public ParticleSystem ExplosionParticles;
    private Vector3 explosionPoint;
    public float explosionRadius = 5f;
    public int explosionDamage = 50;

    // Additional properties for tracking
    public float trackingDelay = 1f;      // Delay before the missile starts tracking
    private float trackingTimer = 0f;     // Timer to keep track of delay
    public float trackingStrength;
    public float trackingLossRate;
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

    private void Update()
    {
        if (Tracking)
        {
            
            trackingTimer += Time.deltaTime;

            // Start tracking after the delay
            if (trackingTimer >= trackingDelay)
            {
                // Determine the target based on projectile ownership
                Vector3 targetPosition = (whichBullet == WhichBullet.Player) ? FindClosestEnemy() : FindClosestPlayer();

                // Calculate direction towards the target
                Vector3 targetDirection = targetPosition - transform.position;
                gameObject.transform.LookAt(targetDirection);
                // Adjust tracking based on distance to target
                float distanceToTarget = targetDirection.magnitude;
                float adjustedTrackingStrength = trackingStrength / (1 + trackingLossRate * distanceToTarget);

                // Update missile velocity towards the target
                Vector3 newVelocity = adjustedTrackingStrength * ProjectileSpeed * targetDirection.normalized;
                currentVelocity = newVelocity.magnitude;
                GetComponent<Rigidbody>().velocity = newVelocity;
            }
        }
    }

    private Vector3 FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            return Vector3.zero;
        }

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy != null ? closestEnemy.transform.position : Vector3.zero;
    }

    private Vector3 FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            return Vector3.zero;
        }

        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestPlayer = player;
                closestDistance = distance;
            }
        }

        return closestPlayer != null ? closestPlayer.transform.position : Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        explosionPoint = collision.contacts[0].point;
        ApplyEffect();
        Destroy(gameObject);
    }

}
