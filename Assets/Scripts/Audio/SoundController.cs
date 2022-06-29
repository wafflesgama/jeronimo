using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController current;

    private FMOD.Studio.EventInstance instance;

    public void PlayOneShotEvent(string eventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventName);
    }

    public void PlayTrack(string eventName)
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(eventName);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, GetComponent<Transform>(), GetComponent<Rigidbody>());
        instance.start();
    }

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        PlayTrack("event:/Narrative/Main Hall");
    }
}
