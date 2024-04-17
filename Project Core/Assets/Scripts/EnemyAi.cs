using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyAi : Entity
{
    public NavMeshAgent agent;

    public Transform player;
    public GameObject EnemyHead;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    

    public float sightRange, attackRange;
    public bool playerInSightrange, playerInAttackRange;
    public List<EnemyAttacks> ListOfAttack = new List<EnemyAttacks>();
    private EnemyAttacks CurrentAttack;
    public AudioClip IdleSoundEffect;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = EntitySpeed;
        RandomiseAttack();
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if(IdleSoundEffect != null)
        {
            audioSource.clip = IdleSoundEffect;
            audioSource.loop = true;
            audioSource.Play();
        }
        

    }
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
        
    }
    protected void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    public virtual void ChasePlayer()
    {
        agent.SetDestination(player.position);
        sightRange = 3*sightRange;
        
    }
    private void RandomiseAttack()
    {
        List<EnemyAttacks> availableAttacks = new List<EnemyAttacks>();

        int totalPriority = 0;

        foreach (EnemyAttacks attack in ListOfAttack)
        {
            if (!attack.alreadyAttacked)
            {
                totalPriority += attack.Priority;
                availableAttacks.Add(attack);
            }
        }

        if (availableAttacks.Count == 0)
        {
            
            return;
        }

        int randomValue = Random.Range(0, totalPriority);

        int cumulativeProbability = 0;

        foreach (EnemyAttacks attack in availableAttacks)
        {
            cumulativeProbability += attack.Priority;

            if (randomValue < cumulativeProbability)
            {
                CurrentAttack = attack;
                break;
            }
        }
    }
    public IEnumerator AttackWithDelay()
    {
        
        yield return new WaitForSeconds(3f);
        AttackPlayer();



    }
    public virtual void AttackPlayer()
    {
        
        
        RandomiseAttack();
        if (CurrentAttack.alreadyAttacked == false)
        {
            CurrentAttack.WarningShot();
            
            CurrentAttack.attack();
            
            CurrentAttack.alreadyAttacked = true;

        }
        


    }
    public void LookAtPlayer()
    {
        
        if(EnemyHead != null)
        {
            Vector3 directionToPlayer = (player.position - EnemyHead.transform.position).normalized;


            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);


            EnemyHead.transform.rotation = Quaternion.Slerp(EnemyHead.transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
       
    }
    protected bool IsMoving()
    {
        return agent.velocity.magnitude > 0.1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
