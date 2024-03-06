using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : EnemyBasedScrpt
{
    public Transform additionalFirePoint;
    public override void Attack()
    {
        GameObject projectile = Instantiate(EnemyProjectile.gameObject, firepoint.position, firepoint.rotation);
        GameObject projectile2 = Instantiate(EnemyProjectile.gameObject, additionalFirePoint.position, additionalFirePoint.rotation);

        projectile.GetComponent<Rigidbody>().velocity = firepoint.forward.normalized * EnemyProjectile.ProjectileSpeed * EntitySpeed;
        projectile2.GetComponent<Rigidbody>().velocity = additionalFirePoint.forward.normalized * EnemyProjectile.ProjectileDamage * EntitySpeed;
    }
}
