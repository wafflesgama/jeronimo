using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonSensorPermanent : MonoBehaviour
{
    public int numDoor;

    void OnTriggerEnter(Collider other)
    {
        FloorButtonDoorPermanent.sensor[numDoor] = true;
    }
 
    void OnTriggerExit(Collider other)
    {
        FloorButtonDoorPermanent.sensor[numDoor] = false;
    }
}
