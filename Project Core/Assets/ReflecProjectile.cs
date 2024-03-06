using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflecProjectile : MonoBehaviour
{
    public float Parrytime;
    public LayerMask enemyBulletLayer;
    private RaycastHit hit; // Declare hit variable

    private void Update()
    {
        if (Parrytime > 0)
        {
            Parrytime -= Time.deltaTime;
            Debug.Log("Hit " + CheckIFHitByEnemyBullet());
            if (CheckIFHitByEnemyBullet())
            {
                Deflect();
            }
        }
        else
        {
            Parrytime = 0.75f;
            gameObject.SetActive(false);
        }
    }

    private bool CheckIFHitByEnemyBullet()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, enemyBulletLayer))
        {
            return true;
        }
        return false;
    }

    private void Deflect()
    {
        Debug.Log("Has deflected");
        Vector3 destination;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(10000);
        }
        Debug.Log("hit " + hit);
        Projectile projectile = hit.collider.GetComponent<Projectile>();

        // Change the ownership of the projectile
        projectile.ChangeBulletOwnershipToOpposite();

        projectile.gameObject.layer = LayerMask.NameToLayer("ProjectilesPlayer");

        projectile.Tracking = true;

        Vector3 direction = Vector3.Reflect(transform.forward, hit.normal).normalized;

        // Update position and rotation of the projectile
        projectile.transform.rotation = Quaternion.LookRotation(direction);
        projectile.transform.position = destination;

        // Apply velocity to the projectile
        projectile.currentVelocity = projectile.ProjectileSpeed * Player.Instance.SprintSpeed;
        projectile.GetComponent<Rigidbody>().velocity = direction * projectile.currentVelocity;
    }
}

