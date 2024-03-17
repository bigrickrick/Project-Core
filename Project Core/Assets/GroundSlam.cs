using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : MonoBehaviour
{

    public float slamForce = 10f;
    public float slamRadius = 5f;
    public int groundSlamDamage;
    public LayerMask slamLayerMask;
    public ParticleSystem groundSlamParticles;

    private bool isSlamming = false;
    public float cameraShakeIntensity = 0.5f;
    public float cameraShakeDuration = 0.2f;
    public AudioClip groundslamsoundeffect;

    void Update()
    {
        if (!GetComponent<Player>().isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && !isSlamming)
            {
                GroundSlamAttack();
            }
        }
    }

    void GroundSlamAttack()
    {
        isSlamming = true;

        GetComponent<Rigidbody>().AddForce(Vector3.down * slamForce, ForceMode.Impulse);

        Invoke("ResetSlam", 1f);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isSlamming)
        {
            if(groundSlamParticles != null)
            {
                Instantiate(groundSlamParticles, collision.contacts[0].point, Quaternion.identity);
            }
            GetComponent<AudioSource>().clip = groundslamsoundeffect;
            GetComponent<AudioSource>().Play();
            Camera.main.GetComponent<CameraShake>().Shake(cameraShakeIntensity, cameraShakeDuration);

            // Detect objects within the slam radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, slamRadius, slamLayerMask);

            if (colliders != null && colliders.Length > 0)
            {
                foreach (Collider col in colliders)
                {
                    if (col != null)
                    {
                        Rigidbody rb = col.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            Vector3 direction = col.transform.position - transform.position;
                            rb.AddForce(direction.normalized * 10, ForceMode.Impulse);
                        }
                        Entity entity = col.GetComponent<Entity>();
                        if (entity != null)
                        {
                            entity.DamageRecieve(groundSlamDamage);
                        }
                    }
                }
            }
            

        }
    }
    void ResetSlam()
    {
        isSlamming = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }
}
