using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class FloorButtonSensorStatic : MonoBehaviour
{
    public int numDoor;

    public static UEvent<int> OnPressed = new UEvent<int>();
    public static UEvent<int> OnReleased = new UEvent<int>();

    void OnTriggerEnter(Collider other)
    {
        OnPressed.TryInvoke(numDoor);
        //FloorButtonDoorStatic.sensor[numDoor] = true;
    }

    void OnTriggerExit(Collider other)
    {
        OnReleased.TryInvoke(numDoor);
        //FloorButtonDoorStatic.sensor[numDoor] = false;
    }

}

