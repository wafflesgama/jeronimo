using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovController : MonoBehaviour
{
    
    public Transform[] patrolPoints;
    

    private NavMeshAgent agent;
    private Transform lastKnownPlayerLocation = null;
    private Vector3 selectedPos;
    private int currentPoint = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        selectNextPosition();

        Debug.Log(selectedPos);

        agent.destination = selectedPos;
    }

    private void selectNextPosition()
    {
        if(lastKnownPlayerLocation == null)
        {
            if(!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                if(currentPoint == patrolPoints.Length - 1)
                    currentPoint = 0;
                else
                    currentPoint += 1;
            }
            Debug.Log(currentPoint);

            selectedPos = patrolPoints[currentPoint].position; 
            
        }
        else
        {
            selectedPos = lastKnownPlayerLocation.position;
        }
    }

    private void setLastKnownPlayerLocation(Transform pos)
    {
        lastKnownPlayerLocation = pos;
    }
}
