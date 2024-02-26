using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyProjectile : Projectile
{

    public override void ApplyEffect()
    {
        // Instantiate explosion particles
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected");
        if (collision.collider.CompareTag("Player"))
        {
            Entity entity = collision.collider.GetComponent<Entity>();
            if (entity != null)
            {
                entity.DamageRecieve(ProjectileDamage);
                Debug.Log("Damaging player " + ProjectileDamage);
            }
        }

        

        ApplyEffect();
        Destroy(gameObject);
    }
}
