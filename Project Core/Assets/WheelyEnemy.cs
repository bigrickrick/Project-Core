using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelyEnemy : EnemyBasedScrpt
{
    

    public override void Attack()
    {

        GameObject projectile = Instantiate(EnemyProjectile.gameObject, firepoint.position, firepoint.rotation);

        projectile.GetComponent<Rigidbody>().velocity = firepoint.forward.normalized * EnemyProjectile.ProjectileSpeed * EntitySpeed;
    }

}
