//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    #region PublicVariables
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float wanderRadius = 20f;
    public float health = 30f;
    public float attackDamage = 30f;
    public float stunDuration = 2.0f;

    public Collider bodyCollider;
    public Collider headCollider;
    #endregion

    #region PrivateVariables
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isStunned = false;
    private float attackCooldown = 5f;
    private bool canAttack = true;
    #endregion

    #region UnityEngine
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(WanderRoutine());
    }

    void Update()
    {
        if (isStunned || health <= 0) return;

        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer <= attackDistance && canAttack)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            ChasePlayer();
        }

        // Handle animation transitions based on movement and state
        HandleAnimation();
    }
    #endregion

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (!isStunned && health > 0 && !agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
            {
                WanderRandomly();
            }
            yield return new WaitForSeconds(15f);
        }
    }

    void WanderRandomly()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, -1))
        {
            finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }
        else
        {
            // If no valid point is found, don't change the wandering state
            Debug.Log("No valid wander point found.");
        }

        agent.SetDestination(finalPosition);
    }

    void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
    }

    void AttackPlayer()
    {
        animator.SetTrigger("Attack");

        // Implement the logic to deal damage to the player here
        // For example:
        // if (playerTransform.GetComponent<PlayerHealth>() != null)
        // {
        //     playerTransform.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        // }

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(float damage, bool isHeadShot = false)
    {
        if (isHeadShot) damage *= 3;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Stun()); // Keep the stun behavior when taking damage
        }
    }

    IEnumerator Stun()
    {
        // Start Stun
        isStunned = true;
        agent.isStopped = true;
        animator.SetTrigger("isStaggered");

        yield return new WaitForSeconds(stunDuration);

        // Stop Stun
        agent.isStopped = false;
        isStunned = false;

        // Start chasing the player upon taking damage
        if (!isStunned && health > 0) // Check if not stunned or dead to start chasing
        {
            ChasePlayer();
        }
    }

    void Die()
    {
        animator.SetTrigger("isDead");
        agent.isStopped = true;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>(); // Assuming Bullet script contains damage info
            if (bullet != null)
            {
                bool isHeadShot = collision.collider == headCollider; // Check if the collision is with the head collider
                TakeDamage(bullet.damage, isHeadShot);
            }
        }
    }

    void HandleAnimation()
    {
        bool isMoving = (agent.velocity.magnitude > 0.1f); // Check if the agent is moving
        animator.SetBool("isChasing", isMoving); // Use "isChasing" or your actual running animation parameter
        
        //animator.SetBool("isIdle", !isMoving);

    }
}
