using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


enum EnemyState
{
    IDLE,
    PATROLLING,
    FOLLOWING,
    SEARCHING
}

public class EnemyMovController : MonoBehaviour
{

    public float defaultIdleTimer = 3f;

    public Transform[] patrolPoints;

    private bool playerDetected = false;
    private int currentDestination = 0;
    private float idleTimer;
    private Vector3 selectedPosition;
    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.PATROLLING;
    private bool hasSelectedNextPos = false;
    

    


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolPoints[currentDestination].position);
        resetIdleTimer();
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case EnemyState.IDLE:
                if (playerDetected)
                {
                    //add Detection Logic Here
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
                        hasSelectedNextPos=false;
                        currentState = EnemyState.PATROLLING;
                    }
                }
                break;
            case EnemyState.PATROLLING:
                if (playerDetected)
                {
                    //add Detection Logic Here
                }
                else
                {
                    if(!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        currentState = EnemyState.IDLE;
                        resetIdleTimer();
                    }
                        
                }
                break;
            case EnemyState.FOLLOWING:
                //add Detection Logic Here
                break;
            case EnemyState.SEARCHING:
                //add Detection Logic Here
                break;
        }
            
    }

    private Vector3 selectNextPatrolPoint()
    {
        Vector3 nextPos;

        if(currentDestination == patrolPoints.Length - 1)
        {
           currentDestination = 0;
        }
        else
        {
          currentDestination++;
        }

        Debug.Log(currentDestination);

        nextPos = patrolPoints[currentDestination].position;

        return nextPos;
    }

    public void setEnemyDetected(bool detection)
    {
        playerDetected = detection;
    }

    private void resetIdleTimer()
    {
        idleTimer = defaultIdleTimer;
    }
}
