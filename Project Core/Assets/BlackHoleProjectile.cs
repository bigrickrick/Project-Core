using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleProjectile : Projectile
{
    public float AttractionRange;
    public float AttractionForce;
    public override void ApplyEffect()
    {
       //nothing
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = transform.position - other.transform.position;
                float distance = direction.magnitude;
                if (distance > 0) // Check if distance is not zero
                {
                    float forceMagnitude = AttractionForce / distance;
                    // Add enemy's velocity to the force
                    Vector3 totalForce = direction.normalized * forceMagnitude + rb.velocity;
                    rb.AddForce(totalForce, ForceMode.Force);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = transform.position - other.transform.position;
                float distance = direction.magnitude;
                if (distance > 0) // Check if distance is not zero
                {
                    float forceMagnitude = AttractionForce / distance;
                    // Add enemy's velocity to the force
                    Vector3 totalForce = direction.normalized * forceMagnitude + rb.velocity;
                    rb.AddForce(totalForce, ForceMode.Force);
                }
            }
        }
    }
}
