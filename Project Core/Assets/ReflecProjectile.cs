using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflecProjectile : MonoBehaviour
{
    public float Parrytime;
    public LayerMask enemyBulletLayer;
    private RaycastHit hit;
    private void Update()
    {
        if(Parrytime > 0)
        {
            Parrytime -= Time.deltaTime;
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

        Destroy(hit.collider.gameObject);
    }
}
