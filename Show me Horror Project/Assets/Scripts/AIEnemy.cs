using System;
using System.Collections;
using System.Collections.Generic;
using GentleCat.ScriptableObjects.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AIEnemy : MonoBehaviour
{
    [SerializeField] private TransformVariable playerTransform;
    [SerializeField] private LanternVariable lantern;
    
    [Header("Stats")] 
    [SerializeField] private float hp;
    [SerializeField] private float lanternMultiplier;
    [SerializeField] private Vector3 hitBox;
    
    
    [Space] [Header("Vision")] 
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float viewRange = 10;
    [Range(0,360)][SerializeField] private float viewAngle = 90;

    [Space] [Header("Patrolling")] 
    [SerializeField] private float patrolSpeed;
    [SerializeField] private Vector2 waitTime;
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex;

    [Space] [Header("Chasing")] 
    [SerializeField] private float chaseSpeed;
    [Range(0,360)][SerializeField] private float chasingViewAngle = 270;
    
    



    private enum EnemyState
    {
        WAITING,
        PATROLLING,
        CHASING
    }

    private EnemyState state;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    private Collider shrine;

    private Vector3 lastPlayerPosition;
    private float waitTimer;


    //  if the enemy has caught the player

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex]
            .position); //  Set the destination to the first waypoint
    }

    private void Update()
    {
        LookForPlayer();


        switch (state)
        {
            case EnemyState.WAITING:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                    state = EnemyState.PATROLLING;
                break;
            case EnemyState.PATROLLING:
                Patrolling();
                break;
            case EnemyState.CHASING:
                Chasing();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shrine") && other.GetComponent<Shrine>().lit)
        {
            state = EnemyState.PATROLLING;
            shrine = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shrine"))
        {
            shrine = null;
        }
    }

    private void Chasing()
    {
        Move(chaseSpeed);
        navMeshAgent.SetDestination(lastPlayerPosition);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) 
        {
            if (Vector3.Distance(playerTransform.CurrentValue.position, lastPlayerPosition) > 0.5f)
            {
                Wait();
            }
        }
    }

    private void Wait()
    {
        waitTimer = Random.Range(waitTime.x,waitTime.y);
        state = EnemyState.WAITING;
    }

    private void Patrolling()
    {
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex]
            .position); //  Set the enemy destination to the next waypoint
        
        Move(patrolSpeed);

        //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Wait();
            NextPoint();
        }
    }

    public void NextPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    private void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    private void Move(float speed)
    {
        if (lantern.CurrentValue.IsInside(transform.position))
            speed *= lanternMultiplier;
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    private void LookForPlayer()
    {
        Vector3 dirToPlayer = (playerTransform.CurrentValue.position - transform.position).normalized;
        if (!(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)) return;

        float dstToPlayer = Vector3.Distance(transform.position, playerTransform.CurrentValue.position);
        if (!(dstToPlayer <= (state == EnemyState.CHASING ?  chasingViewAngle : viewRange))) return;
        
        if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
        {
            lastPlayerPosition = playerTransform.CurrentValue.position;
            state = EnemyState.CHASING;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,hitBox);
    }
}