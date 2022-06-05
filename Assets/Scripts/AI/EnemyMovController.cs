using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


enum EnemyState
{
    IDLE,
    PATROLLING,
    SUSPICIOUS,
    FOLLOWING,
    SEARCHING
}

public class EnemyMovController : MonoBehaviour
{

    public float defaultIdleTimer = 3f;
    private float idleTimer;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int currentDestination = 0;

    [Header("Animations")]
    public Animator animator;

    [Header("Chase")]
    public float chaseSpeedFactor = 1.5f;

    private Vector3 selectedPosition;
    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.PATROLLING;
    private bool hasSelectedNextPos = false;


    private bool playerDetected = false;
    private bool selectedLastKnownLocation = false;
    private int closestPlayer;
    private GameObject[] players;


    private float baseSpeed;



    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolPoints[currentDestination].position);
        baseSpeed = agent.speed;

        ResetIdleTimer();
    }

    void Update()
    {

        switch (currentState)
        {
            case EnemyState.IDLE:
                animator.SetFloat("Speed", 0);
                if (playerDetected)
                {
                    closestPlayer = findClosestPlayer();
                    agent.SetDestination(players[closestPlayer].transform.position);
                    currentState = EnemyState.FOLLOWING;
                }
                else
                {
                    if (!hasSelectedNextPos)
                    {
                        selectedPosition = selectNextPatrolPoint();
                        hasSelectedNextPos = true;
                    }

                    if (idleTimer > 0)
                    {
                        idleTimer -= Time.deltaTime;
                    }
                    else
                    {
                        agent.SetDestination(selectedPosition);
                        hasSelectedNextPos = false;
                        currentState = EnemyState.PATROLLING;
                    }
                }
                break;
            case EnemyState.SUSPICIOUS:
                animator.SetFloat("Speed", 0);
                break;
            case EnemyState.PATROLLING:
                animator.SetFloat("Speed", 1);
                if (playerDetected)
                {
                    closestPlayer = findClosestPlayer();
                    agent.SetDestination(players[closestPlayer].transform.position);
                    currentState = EnemyState.FOLLOWING;
                }
                else
                {

                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        currentState = EnemyState.IDLE;
                        ResetIdleTimer();
                    }

                }
                break;
            case EnemyState.FOLLOWING:
                animator.SetFloat("Speed", 2f);
                if (playerDetected)
                {

                    agent.SetDestination(players[closestPlayer].transform.position);
                    currentState = EnemyState.FOLLOWING;
                }
                else
                {
                    Debug.Log("Is going to search");
                    currentState = EnemyState.SEARCHING;
                    selectedLastKnownLocation = false;
                }
                break;
            case EnemyState.SEARCHING:
                animator.SetFloat("Speed", 1f);
                if (playerDetected)
                {
                    closestPlayer = findClosestPlayer();
                    agent.SetDestination(players[closestPlayer].transform.position);
                    currentState = EnemyState.FOLLOWING;
                }
                else
                {
                    if (!selectedLastKnownLocation)
                    {
                        Debug.Log("LAST KNOWN LOCATION");
                        selectedLastKnownLocation = true;
                        agent.SetDestination(players[closestPlayer].transform.position);
                    }

                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        currentState = EnemyState.IDLE;
                    }
                }
                break;
        }

    }

    private Vector3 selectNextPatrolPoint()
    {
        Vector3 nextPos;

        if (currentDestination == patrolPoints.Length - 1)
        {
            currentDestination = 0;
        }
        else
        {
            currentDestination++;
        }

        nextPos = patrolPoints[currentDestination].position;

        return nextPos;
    }


    public int findClosestPlayer()
    {
        int counter = 0;
        float closestDistance = Vector3.Distance(transform.position, players[0].transform.position);

        for (int i = 0; i < players.Length; i++)
        {
            float temp = Vector3.Distance(transform.position, players[i].transform.position);

            if (closestDistance > temp)
            {
                closestDistance = temp;
                counter = i;
            }

        }

        return counter;
    }

    public async void SetEnemyDetected(bool detection)
    {
        playerDetected = detection;

        if (detection)
        {
            animator.SetTrigger("Chase");
            agent.speed = baseSpeed * chaseSpeedFactor;
            agent.isStopped = false;
        }
        else
        {
            animator.SetTrigger("Stop Chase");
            agent.speed = baseSpeed;
            agent.isStopped = true;
            await Task.Delay(3000);
            agent.isStopped = false;
            animator.SetTrigger("Stop Sus");
        }

    }

    public void SetEnemySuspicious(bool sus)
    {
        if (sus)
            animator.SetTrigger("Sus");
        else
            animator.SetTrigger("Stop Sus");

        agent.isStopped = sus;
        currentState = sus ? EnemyState.IDLE : EnemyState.PATROLLING;

    }


    private void ResetIdleTimer()
    {
        idleTimer = defaultIdleTimer;
    }
}
