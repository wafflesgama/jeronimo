using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonSensor : MonoBehaviour
{
    public int numDoor;
    public SoundController soundController;

    private string eventName = "event:/Interactables/Ground Switch";


    void OnTriggerEnter(Collider other)
    {
        FloorButtonDoor.sensor[numDoor] ++;
        soundController.PlayOneShotEvent(eventName);
    }
 
    void OnTriggerExit(Collider other)
    {
        FloorButtonDoor.sensor[numDoor] --;
        soundController.PlayOneShotEvent(eventName);
    }
    
}
