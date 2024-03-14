using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BlackHoleProjectile : Projectile
{
    public float AttractionRange;
    public float AttractionForce;
    private HashSet<NavMeshAgent> agentsInBlackHole = new HashSet<NavMeshAgent>();

    public override void ApplyEffect()
    {
        // Nothing
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
                if (distance > 0) 
                {
                    float forceMagnitude = AttractionForce / distance;
                    
                    Vector3 totalForce = direction.normalized * forceMagnitude + rb.velocity;
                    rb.AddForce(totalForce, ForceMode.Force);
                }
            }

         
            NavMeshAgent navMeshAgent = other.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
                agentsInBlackHole.Add(navMeshAgent);
            }
            AIController aIController = other.GetComponent<AIController>();
            if(aIController != null)
            {
                aIController.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            
            NavMeshAgent navMeshAgent = other.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null && agentsInBlackHole.Contains(navMeshAgent))
            {
                navMeshAgent.enabled = true;
                agentsInBlackHole.Remove(navMeshAgent);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
       
        if (other.CompareTag("Enemy"))
        {
            other.transform.position = transform.position;
        }
    }
}
