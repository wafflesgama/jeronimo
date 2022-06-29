using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private object Lock = new object();
    public static SoundController current;

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.EventInstance instance2;
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

    public void PlayReviveSound()
    {
        instance2 = FMODUnity.RuntimeManager.CreateInstance("event:/Core Gameplay/Revive");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance2, GetComponent<Transform>(), GetComponent<Rigidbody>());
        instance2.start();
    }

    public void StopReviveSound()
    {
        instance2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance2.release();
    }

    public void PlayDizzySound()
    {
        instance2 = FMODUnity.RuntimeManager.CreateInstance("event:/Core Gameplay/Dizzy");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance2, GetComponent<Transform>(), GetComponent<Rigidbody>());
        instance2.start();
    }

    public void StopDizzySound()
    {
        instance2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance2.release();
    }
}
