using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusicController : MonoBehaviour
{
    private bool isActive = false;

    public string eventName;
    public SoundController soundController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isActive)
        {
            isActive = true;
            soundController.PlayTrack(eventName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;   
    }
}
