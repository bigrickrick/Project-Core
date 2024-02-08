using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public int ProjectileDamage;
    public int ProjectileSpeed;

    public abstract void ApplyEffect();
    public virtual void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {

            Entity entity = other.GetComponent<Entity>();
            ApplyEffect();
            if (entity != null)
            {
                entity.DamageRecieve(ProjectileDamage);
            }

            
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }
}
