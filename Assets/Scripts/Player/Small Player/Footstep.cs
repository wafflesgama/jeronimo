using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    public SoundController controller;
    public int player;

    private string eventName;

    private void Start()
    {
        eventName = string.Format("event:/Core Gameplay/Player {0} Footsteps", player);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(player);
        controller.playOneShotEvent(eventName);
    }
}
