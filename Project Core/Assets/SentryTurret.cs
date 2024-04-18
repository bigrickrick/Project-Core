using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTurret : EnemyAi
{
    public float aboveHeight = 10f; 
    public float belowHeight = 7f;  
    private float BaseOffset;
    private void Start()
    {
        
        agent.autoBraking = false; 
        BaseOffset = agent.baseOffset;
    }
    
    public override void ChasePlayer()
    {
        base.ChasePlayer();
        transform.LookAt(player);

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
    }
    public override void AttackPlayer()
    {
        transform.LookAt(player);
        base.AttackPlayer();
        
    }
}
