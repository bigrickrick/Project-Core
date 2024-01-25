using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemy : EnemyScript
{
    
    public override void ChaseTarget()
    {
        Vector3 direction = Target.position - transform.position;
        direction.Normalize();

        
        transform.Translate(direction * gameObject.GetComponent<Entity>().EntitySpeed * Time.deltaTime);
        
    }
    public override void EnemyLookAtTarget()
    {
        transform.LookAt(Target);
    }
    public override void EnemyAttack()
    {
        
        EnemyLookAtTarget();

        
    }
    

    


}
