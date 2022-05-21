using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonSensorStatic : MonoBehaviour
{
    public int numDoor;

    void OnTriggerEnter(Collider other)
    {
        FloorButtonDoorStatic.sensor[numDoor] = true;
    }
 
    void OnTriggerExit(Collider other)
    {
        FloorButtonDoorStatic.sensor[numDoor] = false;
    }
    
}

