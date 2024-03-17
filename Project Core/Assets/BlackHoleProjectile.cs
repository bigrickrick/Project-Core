using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BlackHoleProjectile : Projectile
{
    private Vector3 explosionPoint;
    public BlackHole blackHole;
    public override void ApplyEffect()
    {
        GameObject blackhole = Instantiate(blackHole.gameObject, explosionPoint, Quaternion.identity);
        Destroy(gameObject);
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Target))
        {
            explosionPoint = collision.contacts[0].point;

            ApplyEffect();
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Projectile"))
        {
            //do nothing
        }
        else
        {
            explosionPoint = collision.contacts[0].point;

            ApplyEffect();
            Destroy(gameObject);
        }
    }

}
