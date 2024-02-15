using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : MonoBehaviour
{
    public int amount;

    public abstract void Apply(GameObject target);
    private Collider potionCollider;
    [SerializeField] private GameObject potionvisual;
    private void OnTriggerEnter(Collider collision)
    {
        GameObject collidedObject = collision.gameObject;


        if (collidedObject.CompareTag("Player"))
        {

            Apply(collidedObject);
            potionCollider.enabled = false;
            potionvisual.SetActive(false);
        }
    }
    
    void Start()
    {
        potionCollider = GetComponent<Collider>(); // Get the collider component attached to the potion
    }
    public float rotationSpeed = 50f; 
   

    
    void Update()
    {
        
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
