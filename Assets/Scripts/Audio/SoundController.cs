using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private object Lock = new object();
    public static SoundController current;

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.Bus MasterBus;

    public void PlayOneShotEvent(string eventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventName);
    }

    public void PlayTrack(string eventName)
    {

        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();

        instance = FMODUnity.RuntimeManager.CreateInstance(eventName);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, GetComponent<Transform>(), GetComponent<Rigidbody>());
        instance.start();

        
    }

    public bool IsPlaying(string eventName)
    {
        FMOD.Studio.EventDescription description;
        string result;

        instance.getDescription(out description);
        description.getPath(out result);

        return eventName == result;
    }

    public void StopPlaying()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    public void StopAllEvents()
    {
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Awake()
    {
        current = this;
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    private void Start()
    {
        PlayTrack("event:/Narrative/Main Hall");
    }
}
