using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttacks: MonoBehaviour
{
    public int Priority;
    public float timeBetweenAttacks;
    public float maxTimeBetweenAttacks;
    public bool alreadyAttacked;
    public EnemyAi enemy;
    public AudioClip attackWarning;
    protected AudioSource audioSource;
    public bool isPlaying;
    
    public virtual void attack()
    {

    }

    private void Awake()
    {
        maxTimeBetweenAttacks = maxTimeBetweenAttacks /enemy.attackspeedModifier;
        timeBetweenAttacks = maxTimeBetweenAttacks;
        
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (alreadyAttacked == true)
        {
            if (timeBetweenAttacks > 0)
            {

                timeBetweenAttacks -= Time.deltaTime;
            }
            else
            {
                ResetAttack();
            }
        }



    }
    public void ResetAttack()
    {
        alreadyAttacked = false;
        isPlaying = false;
        timeBetweenAttacks = maxTimeBetweenAttacks;
        
    }
    public bool warningShotfinished;
    public void WarningShot()
    {
        if(audioSource != null)
        {
            audioSource.clip = attackWarning;
            audioSource.Play();
        }
        
        
    }
    
}
