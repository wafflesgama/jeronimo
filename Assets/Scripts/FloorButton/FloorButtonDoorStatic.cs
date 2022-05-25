using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorButtonDoorStatic : MonoBehaviour
{
    public static bool[] sensor = Enumerable.Repeat(true, 10).ToArray();
    public int numDoor;
    public float speed = 2;
    [SerializeField] GameObject[] waypoints;

    void Start()
    {
        sensor[numDoor] = false;

    }

    void Update()
    {
        if(sensor[numDoor]){
            transform.position = Vector3.MoveTowards(transform.position, waypoints[1].transform.position, speed * Time.deltaTime );
        }
        else{
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime );
        }
    }
}

