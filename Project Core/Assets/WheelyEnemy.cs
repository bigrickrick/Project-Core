using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelyEnemy : EnemyAi
{
    public Animator animator;

    private void Update()
    {
        playerInSightrange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightrange && !playerInAttackRange) Patrolling();
        if (playerInSightrange && !playerInAttackRange) ChasePlayer();
        if (playerInSightrange && playerInAttackRange) StartCoroutine(AttackWithDelay());
        die();
        if (playerInSightrange)
        {
            LookAtPlayer();
        }
        if (IsMoving())
        {
            if(animator != null)
            {
                animator.SetBool("isMoving", true);
            }
            
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }

        }
    }
    




}
