using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : Projectile
{
    public override void ApplyEffect()
    {
        for (int i = 0; i < 10; i++)
        {
            // Instantiate a new projectile
            GameObject newProjectile = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);

            

            
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Random.insideUnitSphere * explosionForce, ForceMode.Impulse);
                rb.useGravity = true;
            }
        }
    }
    public Projectile projectile;
    public float explosionForce;
    public LayerMask portalLayer;
    public LayerMask ReflectLayer;
    private void OnCollisionEnter(Collision collision)
    {


        if (collision.collider.CompareTag(Target))
        {
            Entity entity = collision.collider.GetComponent<Entity>();
            if (entity != null)
            {
                entity.DamageRecieve(ProjectileDamage);
                Debug.Log("Damaging " + Target + ": " + ProjectileDamage);
            }
        }

        if (portalLayer == (portalLayer | (1 << collision.gameObject.layer)))
        {

            return;
        }
        else if (ReflectLayer == (ReflectLayer | (1 << collision.gameObject.layer)))
        {
            return;
        }
        else
        {
            

            ApplyEffect();
            Destroy(gameObject);
        }

        

    }
}
