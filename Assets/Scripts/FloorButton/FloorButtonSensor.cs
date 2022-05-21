using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonSensor : MonoBehaviour
{
    public int numDoor;

    void OnTriggerEnter(Collider other)
    {
        FloorButtonDoor.sensor[numDoor] = true;
    }
 
    void OnTriggerExit(Collider other)
    {
        FloorButtonDoor.sensor[numDoor] = false;
    }
    
}
