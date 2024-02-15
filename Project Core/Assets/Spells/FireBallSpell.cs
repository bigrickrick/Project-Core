using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpell : Spell
{

    public override void ShootSpell(Transform firepoint)
    {
        Vector3 destination;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000);
        }

        GameObject projectile = Instantiate(spell.spellProjectile.gameObject, firepoint.position, firepoint.rotation);

        projectile.GetComponent<Rigidbody>().velocity = (destination - firepoint.position).normalized * spell.spellProjectile.ProjectileSpeed * Player.Instance.SprintSpeed * 0.8f;

        
    }
}
