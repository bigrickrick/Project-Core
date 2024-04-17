using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : MonoBehaviour
{
    public int amount;
    private AudioSource audioSource;
    public AudioClip audioClip;
    public abstract void Apply(GameObject target);
    private Collider potionCollider;
    [SerializeField] private GameObject potionvisual;
    private void OnTriggerEnter(Collider collision)
    {
        GameObject collidedObject = collision.gameObject;


        if (collidedObject.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
           
            Apply(collidedObject);
            potionCollider.enabled = false;
            potionvisual.SetActive(false);
        }
    }
    
    void Start()
    {
        potionCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

    }
    public float rotationSpeed = 50f; 
   

    
    void Update()
    {
        
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
