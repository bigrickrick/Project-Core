using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackHole : MonoBehaviour
{
    public LayerMask attractionLayer;   
    public float AttractionRange = 10f;    
    public float AttractionForce = 10f;    
    public float LifeTime = 5f;           
    public float GrowthRate = 0.1f;        

    private void Start()
    {
        StartCoroutine(DestroyAfterLifetime());
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, AttractionRange, attractionLayer);

        foreach (Collider col in colliders)
        {
            
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
            {
                
                Vector3 direction = transform.position - col.transform.position;

                
                rb.AddForce(direction.normalized * AttractionForce * Time.fixedDeltaTime);
            }
        }

        
        transform.localScale += Vector3.one * GrowthRate * Time.deltaTime;
        AttractionRange += 1;
        AttractionForce += 10;
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttractionRange);
    }
}
