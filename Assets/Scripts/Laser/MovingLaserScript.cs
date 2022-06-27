using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaserScript : MonoBehaviour
{
    public Transform transform;

    public Transform startPoint;
    public Transform endPoint;

    private Transform targetPosition;

    private int currentpos = 0;

    public float speed = 10f;

    private void Start()
    {
        targetPosition = startPoint;
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, step);


        if(Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            if(targetPosition == startPoint)
            {
                targetPosition = endPoint;
            }
            else
            {
                targetPosition = startPoint;
            }
        }

    }
}
