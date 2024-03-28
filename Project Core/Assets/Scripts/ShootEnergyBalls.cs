using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnergyBalls : EnemyAttacks
{
    public Transform firepoint;
    public GameObject EnergyBall;
    public override void attack()
    {
        GameObject projectile = Instantiate(EnergyBall, firepoint.position, firepoint.rotation);

        projectile.GetComponent<Rigidbody>().velocity = firepoint.forward.normalized * projectile.GetComponent<Projectile>().ProjectileSpeed;
    }

    

}

