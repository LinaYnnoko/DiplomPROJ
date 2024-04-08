using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int healthCount;
    public int damage;

    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer, whatIsGround;

    [Header("Patroling")]
    public float timeBetweenPatroling = 5f;
    public Vector3 walkPoint;
    bool walkPointSet, walkPointCreated;
    public float walkPointRange;

    [Header("Attaking")]
    public float timeBetweenAttacks = 1.6f;
    public AudioClip enemyAttack;
    bool alreadyForAttacked;
    PlayerAnimation playerController;

    [Header("Chase")]
    public float sightRange = 15f;
    public float attackRange = 2f;
    public AudioClip chasingSound;
    bool playerInSightRange, playerInAttackRange, isSoundPlayed;


    AudioSource enemySounds;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemySounds = GetComponent<AudioSource>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }

    }

    void Patroling()
    {
        if (!walkPointSet)
        {
            if (!walkPointCreated)
            {
                Invoke(nameof(SearchWalkPoint), timeBetweenPatroling);
                walkPointCreated = true;
            }
        }
        else
        {
            agent.SetDestination(walkPoint);
            animator.SetBool("Running", false);
            animator.SetBool("IsAttaking", false);
            animator.SetBool("Walking", true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 0.5f)
        {
            animator.SetBool("Walking", false);
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 3f, whatIsGround))
        {
            walkPointSet = true;
            walkPointCreated = false;
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("IsAttaking", false);
        animator.SetBool("Walking", false);
        animator.SetBool("Running", true);
        if (!enemySounds.isPlaying && playerInSightRange)
        {
            enemySounds.PlayOneShot(chasingSound);
        }
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyForAttacked)
        {
            enemySounds.PlayOneShot(enemyAttack);
            playerController.TakeDamage(damage);
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            animator.SetBool("IsAttaking", true);
            alreadyForAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyForAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        healthCount -= damage;
        if (healthCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerAnimation>() != null)
        {
            playerController = other.gameObject.GetComponent<PlayerAnimation>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerController = null;
    }
}
