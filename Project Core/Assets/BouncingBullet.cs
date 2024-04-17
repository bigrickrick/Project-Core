using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : Projectile
{
    public ParticleSystem ExplosionParticles;
    public float detectionRange = 20f;
    private float LifeTime = 5;
    private int bouncesLeft;
    public int maxBounces;
    private Vector3 explosionPoint;
    public float explosionRadius = 5f;
    public int explosionDamage = 50;
    public LayerMask Ground;
    public LayerMask Floor;
    private Rigidbody rb;
    public AudioClip bouncingSoundEffect;
    private void Start()
    {
        if (whichBullet == WhichBullet.Player)
            Target = "Enemy";
        else
            Target = "Player";

        bouncesLeft = maxBounces;
        rb = GetComponent<Rigidbody>();
    }

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
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatisGround") ||
        collision.gameObject.layer == LayerMask.NameToLayer("whatisWall"))
        {
            if (bouncesLeft > 0)
            {
                if (bouncingSoundEffect != null)
                {
                   GetComponent<AudioSource>().PlayOneShot(bouncingSoundEffect);
                }

                Vector3 targetPosition = Player.Instance.transform.position;
                Vector3 directionToPlayer = (targetPosition - transform.position).normalized;

                
                Vector3 incomingVelocity = rb.velocity;
                Vector3 normal = collision.contacts[0].normal;
                Vector3 reflectionDir = Vector3.Reflect(incomingVelocity, normal).normalized;

                
                Vector3 targetDirection = Vector3.Lerp(reflectionDir, directionToPlayer, 0.65f).normalized; 

                
                rb.velocity = targetDirection * ProjectileSpeed;
                bouncesLeft--;
            }
            else
            {
                explosionPoint = collision.contacts[0].point;
                ApplyEffect();
                Destroy(gameObject);
            }
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
            RedoTarget();

        Acceleration();

        if (hasalifeTime)
        {
            if (LifeTime > 0)
                LifeTime -= Time.deltaTime;
            else
            {
                Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    private void Acceleration()
    {
        Vector3 newVelocity = rb.velocity + transform.forward * Time.deltaTime * 2;
        rb.velocity = Vector3.ClampMagnitude(newVelocity, ProjectileSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
