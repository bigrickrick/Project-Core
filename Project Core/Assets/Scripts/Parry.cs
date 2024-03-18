using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public Transform ParryPoint;
    public float maxParryTimeDuration;
    public float ParryTimer;
    public LayerMask BulletLayer;
    public LayerMask Layertochangeforbullet;
    public float ParrySurfaceArea;
    public float MaxWaitBeforeNextParry;
    public float waittimerfornextparry;
    public ParticleSystem successfullParticulesParry;
    public ParticleSystem ParrySheild;
    private ParticleSystem activeParryShield;
    public AudioClip ParrySoundeffect;
    private void Update()
    {
        if(waittimerfornextparry <= 0)
        {
            
            if (Player.Instance.IsParrying)
            {
                if (ParryTimer > 0)
                {
                    

                    parry();
                    ParryTimer -= Time.deltaTime;
                }
                else
                {
                    ParryTimer = maxParryTimeDuration;
                    waittimerfornextparry = MaxWaitBeforeNextParry;
                    Player.Instance.IsParrying = false;
                }
            }
        }
        else
        {
            waittimerfornextparry -= Time.deltaTime;
        }
       
    }
    private void LateUpdate()
    {
        if (activeParryShield != null && Player.Instance != null)
        {
            
            Vector3 shieldRotation = new Vector3(0, Player.Instance.transform.eulerAngles.y, 0);
            activeParryShield.transform.rotation = Quaternion.Euler(shieldRotation);
        }
    }
    private void parry()
    {
        if (activeParryShield == null) // Check if a parry shield is already active
        {



            Vector3 shieldPosition = Player.Instance.transform.position - new Vector3(0, Player.Instance.GetComponent<Collider>().bounds.extents.y, 0);
            activeParryShield = Instantiate(ParrySheild, shieldPosition, Quaternion.identity);
            activeParryShield.transform.SetParent(Player.Instance.transform); 

            StartCoroutine(DestroyParryShield());

        }
        Collider[] colliders = Physics.OverlapSphere(ParryPoint.position, ParrySurfaceArea, BulletLayer);
        Vector3 destination;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(10000);
        }
        foreach (Collider col in colliders)
        {
            Projectile projectile = col.GetComponent<Projectile>();
            ChangeBulletDirection(projectile, destination);
        }
    }

    
    private IEnumerator DestroyParryShield()
    {
        yield return new WaitForSeconds(maxParryTimeDuration); 
        Destroy(activeParryShield.gameObject); 
        activeParryShield = null; 
    }

    private void ChangeBulletDirection(Projectile projectile, Vector3 Destination)
    {
        
        ParticleSystem particles = Instantiate(successfullParticulesParry, ParryPoint.position, Quaternion.identity);
        particles.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0);
        Destroy(particles.gameObject, particles.main.duration);
        GetComponent<AudioSource>().clip = ParrySoundeffect;
        GetComponent<AudioSource>().Play();
        projectile.whichBullet = Projectile.WhichBullet.Player;
        projectile.RedoTarget();
        
        int layerToChange = Mathf.Clamp(Layertochangeforbullet, 0, 31);
        projectile.gameObject.layer = layerToChange;
        projectile.Tracking = true;
        projectile.GetComponent<Rigidbody>().velocity = (Destination - Player.Instance.firePoint.position).normalized * projectile.ProjectileSpeed;

    }
    private IEnumerator OnParry()
    {
        Player.Instance.GetComponent<Modifiers>().CanBedamage = true;
        yield return new WaitForSeconds(0.2f);
        Player.Instance.GetComponent<Modifiers>().CanBedamage = false;
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ParrySurfaceArea);
    }


}
