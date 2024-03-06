using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBasedScrpt
{
    public float teleportAttackMaxTimer;
    public float teleportTimer;
    public float SpecialAttackMaxTimer;
    public float SpecialAttackTimer;
    public float MaxSpecialAttackProjectile;
    private float SpecialAttackProjectile;
    public float MaxDashTimer;
    public float DashTimer;
    public int BaseTeleportationstack;
    private int teleportationstack;
    public Transform Target;
    public ParticleSystem teleportParticles;
    public bool IsDashing;
    public float dashSpeed = 10;
    public Projectile specialAttackPrefab;
    public Transform SpecialFirepoint;
    public float maxAngle;
    private void SpecialAttack()
    {
        for (int i = 0; i < MaxSpecialAttackProjectile; i++)
        {
            // Generate a semi-random direction
            Vector3 randomDirection = Quaternion.Euler(Random.Range(-maxAngle, maxAngle), Random.Range(0, 360), 0) * SpecialFirepoint.up;

            // Instantiate a new projectile with the random direction
            Projectile projectile = Instantiate(specialAttackPrefab, SpecialFirepoint.position, Quaternion.identity);
            projectile.hasalifeTime = false;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = randomDirection.normalized*30;
                rb.useGravity = true;
                
            }
        }
    }
    private void Start()
    {
        attackTimer = baseAttackTimer / attackspeedModifier;
        
    }
    public override void Attack()
    {
        GameObject projectile = Instantiate(EnemyProjectile.gameObject, firepoint.position, firepoint.rotation);

        projectile.GetComponent<Rigidbody>().velocity = firepoint.forward.normalized * EnemyProjectile.ProjectileSpeed * EntitySpeed;
    }
    public void TeleportingAttack()
    {
        StartCoroutine(TeleportWithDelay());
    }
    public void DashAttack()
    {
        
        IsDashing = true;
        StartCoroutine(DashTowardsTarget(Target.position));


    }
    private void LateUpdate()
    {
        UpdateHpbar();
        if (CheckifPlayerInAttackRange())
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attackTimer = baseAttackTimer / attackspeedModifier;
                Attack();
            }
            if (DashTimer > 0)
            {
                DashTimer -= Time.deltaTime;
            }
            else
            {
                DashTimer = MaxDashTimer;
                DashAttack();
            }
            if (teleportTimer > 0)
            {
                teleportTimer -= Time.deltaTime;
            }
            else
            {
                teleportTimer = teleportAttackMaxTimer;
                TeleportingAttack();
            }
            if(SpecialAttackTimer > 0)
            {
                SpecialAttackTimer -=Time.deltaTime;
            }
            else
            {
                SpecialAttackTimer = SpecialAttackMaxTimer;
                SpecialAttack();
            }
        }


    }
    IEnumerator TeleportWithDelay()
    {
        float teleportRange = 15f;

        for (int i = 0; i < BaseTeleportationstack; i++)
        {
            if (teleportationstack != BaseTeleportationstack)
            {
                float randomX = Random.Range(Target.position.x - teleportRange, Target.position.x + teleportRange);
                float randomZ = Random.Range(Target.position.z - teleportRange, Target.position.z + teleportRange);
                Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);

                if (teleportParticles != null)
                {
                    var particleInstance = Instantiate(teleportParticles, transform.position, Quaternion.identity);
                    particleInstance.transform.parent = transform;
                    particleInstance.Play();
                    Destroy(particleInstance.gameObject, particleInstance.main.duration);
                }

                yield return new WaitForSeconds(0.5f);
                
                transform.position = randomPosition;

                Attack();
            }
        }
    }
    IEnumerator DashTowardsTarget(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(targetPosition*dashSpeed);
            yield return null;
        }

        IsDashing = false;
        
    }
}
