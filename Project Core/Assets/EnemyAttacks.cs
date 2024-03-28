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
    public virtual void attack()
    {

    }

    private void Awake()
    {
        maxTimeBetweenAttacks = maxTimeBetweenAttacks /enemy.attackspeedModifier;
        timeBetweenAttacks = maxTimeBetweenAttacks;
    }
    private void Update()
    {
        if(alreadyAttacked == true)
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
    private void ResetAttack()
    {
        alreadyAttacked = false;
        timeBetweenAttacks = maxTimeBetweenAttacks;
    }
}
