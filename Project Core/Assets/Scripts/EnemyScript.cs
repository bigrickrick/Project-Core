using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyScript : Entity
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
        ShootingTimer = baseShootingTimer / attackspeedModifier;
    }
    
    
    public void SetTarget(string target)
    {
        Target = GameObject.Find(target).transform;
    }

    public abstract void Patroling();
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
        
    }
    private void Update()
    {

        TargetInSightRange = Physics.CheckSphere(transform.position, detectionRange, whatisplayer);
        TargetInAttackRange = Physics.CheckSphere(transform.position, Attackrange, whatisplayer) ;       
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Attackrange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
