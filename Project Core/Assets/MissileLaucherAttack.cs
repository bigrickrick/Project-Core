using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaucherAttack : MonoBehaviour
{
    public Projectile missilePrefab;
    public Transform launchPoint;
    public int missileCount;
    public float launchForce;
    public float coneAngle;
    public float shotDelay;
    public float maxRechargeTime;

    public float rechargeTimer;
    public EnemyAi enemy;
    private bool hasAlreadyAttacked;

    private void Update()
    {
        if (rechargeTimer > 0)
        {
            rechargeTimer -= Time.deltaTime;
        }
        else if (!hasAlreadyAttacked && enemy.playerInSightrange)
        {
            hasAlreadyAttacked = true;
            StartCoroutine(LaunchMissilesWithDelay());
            rechargeTimer = maxRechargeTime;
        }
    }

    private IEnumerator LaunchMissilesWithDelay()
    {
        for (int i = 0; i < missileCount; i++)
        {
            Quaternion randomRotation = Quaternion.Euler(
                Random.Range(-coneAngle / 2f, coneAngle / 2f),
                Random.Range(-coneAngle / 2f, coneAngle / 2f),
                0f
            );

            Vector3 launchDirection = randomRotation * launchPoint.forward;

            GameObject missileObject = Instantiate(missilePrefab.gameObject, launchPoint.position, launchPoint.rotation * randomRotation);
            Rigidbody missileRigidbody = missileObject.GetComponent<Rigidbody>();

            if (missileRigidbody != null)
            {
                missileRigidbody.AddForce(launchDirection * launchForce * missilePrefab.ProjectileSpeed, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(shotDelay);
        }

        hasAlreadyAttacked = false; // Reset attack flag after all missiles are launched
    }
}
