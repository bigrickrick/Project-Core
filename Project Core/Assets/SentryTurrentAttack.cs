using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTurrentAttack : EnemyAttacks
{
    public Transform firepoint;
    public GameObject EnergyBall;
    
    private Vector3 playerDirection;
    public override void attack()
    {

        GameObject projectile = Instantiate(EnergyBall, firepoint.position, Quaternion.identity);


        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.velocity = playerDirection * projectile.GetComponent<Projectile>().ProjectileSpeed;


        projectile.transform.rotation = Quaternion.LookRotation(projectileRb.velocity);
    }

    
    
}
