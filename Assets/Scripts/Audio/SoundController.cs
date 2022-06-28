using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public void playOneShotEvent(string eventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventName);
    }
}
