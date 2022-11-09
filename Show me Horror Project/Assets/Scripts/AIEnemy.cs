using System;
using System.Collections;
using System.Collections.Generic;
using GentleCat.ScriptableObjects.Properties;
using GentleCat.ScriptableObjects.Sets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AIEnemy : MonoBehaviour
{
    [SerializeField] private TransformVariable playerTransform;
    [SerializeField] private LanternVariable lantern;
    [SerializeField] private ShrineSet shrines;
    [SerializeField] private GameObjectSet monsters;
    [SerializeField] private GameObject particles;
    [SerializeField] private PlayerMovement player;

    [Header("Stats")] [SerializeField] private float maxHp;
    [SerializeField] private Image healthBar;
    [SerializeField] private float lanternMultiplier;
    [SerializeField] private Bounds hitBox;
    [SerializeField] private UnityEvent onDeath;


    [Space] [Header("Vision")] [SerializeField]
    private LayerMask obstacleMask;

    [SerializeField] private float viewRange = 10;
    [Range(0, 360)] [SerializeField] private float viewAngle = 90;

    [Space] [Header("Patrolling")] [SerializeField]
    private float patrolSpeed;

    [SerializeField] private Vector2 waitTime;
    [SerializeField] private Transform[] waypoints;
    public int currentWaypointIndex;

    [Space] [Header("Chasing")] [SerializeField]
    private float chaseSpeed;

    [Range(0, 360)] [SerializeField] private float chasingViewAngle = 270;
    [SerializeField] private float chaseRange;

    private enum EnemyState
    {
        WAITING,
        PATROLLING,
        CHASING
    }

    private EnemyState state;
    private NavMeshAgent navMeshAgent;

    private Vector3 lastPlayerPosition;
    private float waitTimer = 2f;
    private float hp;


    private void OnEnable()
    {
        monsters.Add(gameObject);
    }

    private void OnDisable()
    {
        monsters.Remove(gameObject);
    }


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex]
            .position); //  Set the destination to the first waypoint
        hp = maxHp;
    }

    private bool IsInShrine(Vector3 point)
    {
        foreach (Shrine shrine in shrines.Items)
        {
            if (shrine.IsInside(point))
                return true;
        }

        return false;
    }

    private void Update()
    {
        LookForPlayer(viewRange, viewAngle);
        if (IsInShrine(lastPlayerPosition) && state == EnemyState.CHASING)
        {
            state = EnemyState.PATROLLING;
        }

        int pointsInLantern = 0;
        for (int x = -1; x <= 1; x += 2)
        {
            for (int z = -1; z <= 1; z += 2)
            {
                if (lantern.CurrentValue.IsInside(transform.position + hitBox.center +
                                                  Vector3.Scale(hitBox.extents, new Vector3(x, 0, z))))
                    pointsInLantern++;
            }
        }
        //add inshrine check

        if (pointsInLantern == 4 && !player.isInShrine)
        {
            hp -= Time.deltaTime;
            if (hp <= 0)
            {
                onDeath.Invoke();
                Destroy(gameObject);
                Destroy(Instantiate(particles, transform.position, quaternion.identity), 1f);
                return;
            }
        }
        else if (hp < maxHp)
        {
            hp += Time.deltaTime * 0.3f;
        }

        healthBar.fillAmount = hp / maxHp;
        healthBar.transform.parent.gameObject.SetActive(hp < maxHp);


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

    private void Chasing()
    {
        LookForPlayer(chaseRange, chasingViewAngle);
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
        waitTimer = Random.Range(waitTime.x, waitTime.y);
        state = EnemyState.WAITING;
    }

    private void Patrolling()
    {
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex]
            .position); //  Set the enemy destination to the next waypoint

        Move(patrolSpeed);

        //Debug.Log(navMeshAgent.remainingDistance);
        //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                Wait();
                NextPoint();
            }
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
        {
            LookForPlayer(chaseRange, 360);
            speed *= lanternMultiplier;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    private void LookForPlayer(float range, float angle)
    {
        Vector3 dirToPlayer = (playerTransform.CurrentValue.position - transform.position).normalized;
        if (!(Vector3.Angle(transform.forward, dirToPlayer) < angle / 2)) return;

        float dstToPlayer = Vector3.Distance(transform.position, playerTransform.CurrentValue.position);
        if (!(dstToPlayer <= range)) return;

        if (Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask)) return;

        lastPlayerPosition = playerTransform.CurrentValue.position;
        state = EnemyState.CHASING;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + hitBox.center, hitBox.size);
    }
}