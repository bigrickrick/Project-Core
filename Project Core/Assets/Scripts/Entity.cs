using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int HealthPoints;
    public int maxHealthPoints;
    public GameObject entity;
    public float EntitySpeed;
    public float attackspeedModifier;
    public AudioClip gettinghitSoundEffect;
    
    public void DamageRecieve(int damage)
    {
        
        HealthPoints = HealthPoints - damage;
        
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gettinghitSoundEffect;
        audioSource.Play();
    }
    private void Update()
    {
        die();
        
    }
    private void Start()
    {
        maxHealthPoints = HealthPoints;
    }
    private void die()
    {
        if (HealthPoints <= 0)
        {
            Destroy(entity);
            
            
        }
    }

}
