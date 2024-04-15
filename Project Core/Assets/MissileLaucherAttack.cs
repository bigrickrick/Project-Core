using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaucherAttack : MonoBehaviour
{
    public Projectile missilePrefab;
    public Transform launchPoint;    // Point where missiles will be launched from
    public int missileCount;     // Number of missiles to launch
    public float launchForce;  // Force to apply to each missile when launched
    public float coneAngle;    // Angle of the cone in degrees
    public float shotDelay;
    public float currentShotDelay;
    public float maxRechargeTime;
    private float RechargeTimer;
    public EnemyAi enemy;
    private void Update()
    {
        if (RechargeTimer > 0)
        {
            RechargeTimer -= Time.deltaTime;
        }
        else
        {
            if (enemy.playerInSightrange)
            {
                RechargeTimer = maxRechargeTime;
                LauchMissiles();
            }
            
        }   
    }
   
    public void LauchMissiles()
    {
        // Check if missile prefab and launch point are assigned
        if (missilePrefab == null || launchPoint == null)
        {
            Debug.LogWarning("Missile prefab or launch point not assigned.");
            return;
        }

        // Calculate spread angle per missile
        float angleStep = coneAngle / (missileCount - 1);

        // Launch missiles
        for (int i = 0; i < missileCount; i++)
        {
            Quaternion randomRotation = Quaternion.Euler(Random.Range(-coneAngle / 2f, coneAngle / 2f), Random.Range(-coneAngle / 2f, coneAngle / 2f), 0f);
            Vector3 launchDirection = randomRotation * launchPoint.forward;

            StartCoroutine(ShotDelayRoutine(shotDelay));
            GameObject missile = Instantiate(missilePrefab.gameObject, launchPoint.position, launchPoint.rotation * randomRotation);


            Rigidbody missileRigidbody = missile.GetComponent<Rigidbody>();
            if (missileRigidbody != null)
            {
                // Apply launch force to the missile in the calculated direction
                missileRigidbody.AddForce(launchDirection * launchForce * missilePrefab.ProjectileSpeed, ForceMode.Impulse);
            }



        }
        
    }

    IEnumerator ShotDelayRoutine(float delay)
    {
      
        yield return new WaitForSeconds(delay);
        
    }
}
