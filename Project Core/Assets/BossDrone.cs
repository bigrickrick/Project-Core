using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDrone : EnemyAi
{
    public float aboveHeight = 10f;
    public float belowHeight = 7f;
    private float BaseOffset;
    public LazerAttack lazerAttack;
    public float maxRechargeTime;
    public float rechargeTime;
    
    private void Start()
    {

        agent.autoBraking = false;
        BaseOffset = agent.baseOffset;
    }
    private void Update()
    {
        playerInSightrange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightrange && !playerInAttackRange) Patrolling();
        if (playerInSightrange && !playerInAttackRange) ChasePlayer();
        if (playerInSightrange && playerInAttackRange) StartCoroutine(AttackWithDelay());
        die();
        

    }
    public override void ChasePlayer()
    {
        base.ChasePlayer();
        if (playerInSightrange)
        {
            LookAtPlayer();
        }

        if (player.transform.position.y > gameObject.transform.position.y)
        {
            agent.baseOffset += 1.5f * Time.deltaTime;
        }
        else
        {
            if (agent.baseOffset > BaseOffset)
            {
                agent.baseOffset -= 3f * Time.deltaTime;
            }
        }
        if(rechargeTime > 0)
        {
            rechargeTime -= Time.deltaTime;
        }
        else
        {
            lazerAttack.isLazerAttacking = true;
            rechargeTime = maxRechargeTime;
        }
    }
    
    public override void AttackPlayer()
    {
        if (!lazerAttack.isLazerAttacking)
        {
            transform.LookAt(player);
            base.AttackPlayer();
        }
        else
        {
            LookAtPlayer();
            lazerAttack.attack();
        }
        

    }
}
