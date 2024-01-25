using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyScript : MonoBehaviour
{
    
    public int damage;
    protected Transform Target;

    public string targetstring;

    public LayerMask whatisground, whatisplayer;
    
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;
    public bool AlreadyAttacked;
    public float Attackrange;
    public float detectionRange;
    

    public bool TargetInSightRange, TargetInAttackRange;

    public enum EnemyState
    {
        Patroling,
        ChaseTarget,
        AttackTarget,
    }
    public EnemyState enemyState = EnemyState.Patroling;
    public int CreditCost;
    public GameObject bullet;
    public Transform firePoint;
    
    public float bulletSpeed = 35f;
    public float baseShootingTimer;
    
    public static EnemyScript Instance { get; private set; }
    [SerializeField] protected Transform[] BulletSpawnPoint;
    private float ShootingTimer;

    private void Start()
    {
        SetTarget(targetstring);
        SetShootingTimer();
    }
    public void SetShootingTimer()
    {
        ShootingTimer = baseShootingTimer / gameObject.GetComponent<Entity>().attackspeedModifier;
    }
    
    
    public void SetTarget(string target)
    {
        Target = GameObject.Find(target).transform;
    }
    
    public  void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            GoToWalkPoint();
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void GoToWalkPoint()
    {
        Vector3 direction = walkPoint - transform.position;
        direction.Normalize();

        
        transform.Translate(direction * gameObject.GetComponent<Entity>().EntitySpeed * Time.deltaTime);
    }
    public abstract void ChaseTarget();
    public abstract void EnemyAttack();
    public abstract void EnemyLookAtTarget();
    
    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatisground))
        {
            walkPointSet = true;
        }
    }
    private void Update()
    {

        TargetInSightRange = Physics.CheckSphere(transform.position, detectionRange, whatisplayer);
        TargetInAttackRange = Physics.CheckSphere(transform.position, Attackrange, whatisplayer) ;

        switch (enemyState)
        {
            case EnemyState.Patroling:
                
                
                
                if (TargetInSightRange)
                {
                    enemyState = EnemyState.ChaseTarget;
                }
                else
                {
                    Patroling();
                }

                break;
            case EnemyState.ChaseTarget:
                ChaseTarget();
                
                if (TargetInAttackRange)
                {
                    enemyState = EnemyState.AttackTarget;
                }
                

                break;
            case EnemyState.AttackTarget:
                if (!TargetInAttackRange)
                {
                    enemyState = EnemyState.ChaseTarget;
                }
                else
                {
                    if(ShootingTimer <= 0)
                    {
                        ChaseTarget();

                        EnemyAttack();
                        AlreadyAttacked = true;
                        SetShootingTimer();
                    }
                    else
                    {
                        ChaseTarget();
                        ShootingTimer -= Time.deltaTime;
                    }
                    
                }
                if (!TargetInSightRange)
                {
                    enemyState = EnemyState.Patroling;
                }

                break;
        }
        

        
        
       
    }
    public bool HasDied = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Attackrange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
