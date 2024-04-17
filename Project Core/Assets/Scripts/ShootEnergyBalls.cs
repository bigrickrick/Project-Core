using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnergyBalls : EnemyAttacks
{
    public Transform firepoint;
    public GameObject EnergyBall;
    public int numberOfShots = 4; 
    public float timeBetweenShots = 0.8f; 
    public float waveInterval = 1.5f; 
    private Vector3 playerDirection; 
    public override void attack()
    {
        StartCoroutine(ShootWave());
    }

    IEnumerator ShootWave()
    {
        for (int i = 0; i < numberOfShots; i++)
        {
           
            playerDirection = (enemy.player.transform.position - transform.position).normalized;

            
            //RotateHeadTowardsPlayer();

            
            GameObject projectile = Instantiate(EnergyBall, firepoint.position, Quaternion.identity);

            
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.velocity = playerDirection * projectile.GetComponent<Projectile>().ProjectileSpeed;

            
            projectile.transform.rotation = Quaternion.LookRotation(projectileRb.velocity);

            yield return new WaitForSeconds(timeBetweenShots);
        }
        
        yield return new WaitForSeconds(waveInterval);
    }
    private void RotateHeadTowardsPlayer()
    {
        
        Vector3 directionToPlayer = (enemy.player.transform.position - transform.position).normalized;

        
        directionToPlayer.y = 0;

        
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        enemy.EnemyHead.transform.rotation= Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }




}

