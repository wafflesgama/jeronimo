using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusicController : MonoBehaviour
{
    // Start is called before the first frame update
    private int playerCount = 0;

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
        if (isActive)
        {
            isActive = false;
            soundController.StopPlaying();
        }
    }
}
