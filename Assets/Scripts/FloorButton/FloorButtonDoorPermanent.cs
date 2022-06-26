using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorButtonDoorPermanent : MonoBehaviour
{
public static bool[] sensor = Enumerable.Repeat(true, 50).ToArray();
    public int numDoor;
    public float speed = 2;
    float dist = 0;

    public Vector3 FinalPoint = new Vector3(2.0f, 0.0f, 0.0f);  
    Vector3 FinalPosition;

    void Start()
    {
        sensor[numDoor] = false;
        FinalPosition = transform.position + FinalPoint;

    }

    void Update()
    {
        if(sensor[numDoor]){
            dist = Vector3.Distance(FinalPosition, transform.position);
            if(dist > 0.1){
                transform.position = Vector3.MoveTowards(transform.position, FinalPosition, speed * Time.deltaTime );
            }
        }
    }
}
