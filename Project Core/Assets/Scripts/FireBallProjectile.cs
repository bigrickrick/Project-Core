using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : Projectile
{
    public ParticleSystem ExplosionParticles;
    public override void ApplyEffect()
    {
        Instantiate(ExplosionParticles, transform.position + Vector3.up * 2.5f, Quaternion.identity);

        ExplosionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision Detected");
        if (other.CompareTag("Enemy"))
        {

            Entity entity = other.GetComponent<Entity>();
            
            if (entity != null)
            {
                entity.DamageRecieve(ProjectileDamage);
            }



        }
        ApplyEffect();
        Destroy(gameObject);
    }
}
