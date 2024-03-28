using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : Entity
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    

    public float sightRange, attackRange;
    public bool playerInSightrange, playerInAttackRange;
    public List<EnemyAttacks> ListOfAttack = new List<EnemyAttacks>();
    private EnemyAttacks CurrentAttack;
    

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        RandomiseAttack();
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
    private void Patrolling()
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
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
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
    private IEnumerator AttackWithDelay()
    {
        transform.LookAt(player);
        yield return new WaitForSeconds(3f);
        AttackPlayer();



    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        RandomiseAttack();
        if(CurrentAttack.alreadyAttacked == false)
        {
            CurrentAttack.attack();
            CurrentAttack.alreadyAttacked = true;
        }
        


    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
