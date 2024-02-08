using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpell : Spell
{
    public override void ShootSpell(Transform firepoint)
    {
        GameObject projectile = Instantiate(spell.spellProjectile.gameObject, firepoint.position, firepoint.rotation);
        
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.AddForce(firepoint.forward * spell.spellProjectile.ProjectileSpeed, ForceMode.Impulse);
    }
}
