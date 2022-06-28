using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

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
    public VisualEffect vfx;

    [Header("Chase")]
    public float chaseSpeedFactor = 1.5f;

    private Vector3 selectedPosition;
    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.PATROLLING;
    private bool hasSelectedNextPos = false;


    private bool playerDetected = false;
    private bool selectedLastKnownLocation = false;

    private Transform playerToChase;

    private float baseSpeed;



    void Awake()
    {
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
                vfx.SetFloat("Rate", 0);
                if (playerDetected)
                {

                    agent.SetDestination(playerToChase.position);
                    currentState = EnemyState.FOLLOWING;
                }
                else
                {
                    if (!hasSelectedNextPos)
                    {
                        selectedPosition = SelectNextPatrolPoint();
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
                vfx.SetFloat("Rate", 0);
                break;
            case EnemyState.PATROLLING:
                animator.SetFloat("Speed", 1);
                vfx.SetFloat("Rate", 1);
                if (playerDetected)
                {
                    playerToChase = FindClosestPlayer();
                    agent.SetDestination(playerToChase.position);
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
                vfx.SetFloat("Rate", 1);
                if (playerDetected)
                {

                    agent.SetDestination(playerToChase.position);
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
                vfx.SetFloat("Rate", 1);
                if (playerDetected)
                {
                    playerToChase = FindClosestPlayer();
                    agent.SetDestination(playerToChase.position);
                    currentState = EnemyState.FOLLOWING;
                }
                else
                {
                    if (!selectedLastKnownLocation)
                    {
                        Debug.Log("LAST KNOWN LOCATION");
                        selectedLastKnownLocation = true;
                        agent.SetDestination(playerToChase.position);
                    }

                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        currentState = EnemyState.IDLE;
                    }
                }
                break;
        }

    }

    private Vector3 SelectNextPatrolPoint()
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


    public Transform FindClosestPlayer()
    {
        var p1Distance = Vector3.Distance(transform.position, PlayerManager.current.player1.smallMovController.transform.position);
        var p2Distance = Vector3.Distance(transform.position, PlayerManager.current.player2.smallMovController.transform.position);


        if (p1Distance <= p2Distance)
            return PlayerManager.current.player1.smallMovController.transform;

        return PlayerManager.current.player2.smallMovController.transform;
    }

    public async void SetEnemyDetected(bool detection, bool p1InRange, bool p2InRange)
    {
        playerDetected = detection;
        if (p1InRange && p2InRange)
        {
            playerToChase = FindClosestPlayer();
        }
        else
        {
            playerToChase = p1InRange ? PlayerManager.current.player1.smallMovController.transform : PlayerManager.current.player2.smallMovController.transform;
        }
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
        {
            playerDetected = false;
            animator.SetTrigger("Stop Sus");
        }

        agent.isStopped = sus;
        currentState = sus ? EnemyState.IDLE : EnemyState.PATROLLING;

    }


    private void ResetIdleTimer()
    {
        idleTimer = defaultIdleTimer;
    }
}
