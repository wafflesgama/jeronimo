using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class LaserDestroy : MonoBehaviour
{
    public bool inTrigger;
    public static bool[] isOn = Enumerable.Repeat(true, 50).ToArray();
    public int numLaser;

    public MergeBehaviour unmerge;

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
        if (other.tag == "Player")
        {
            SoundController.current.PlayOneShotEvent("event:/Interactables/Laser");
            PlayerManager.current.KnockPlayers(other.attachedRigidbody.transform, other.ClosestPointOnBounds(transform.position));
        }
        if (other.tag == "BigPlayer")
        {
            SoundController.current.PlayOneShotEvent("event:/Interactables/Laser");
            BigPlayerTimer();
        }
    }

    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        isOn[numLaser] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn[numLaser])
        {
            this.gameObject.SetActive(false);
        }
    }

    async void BigPlayerTimer()
    {
        await Task.Delay(100);
        if(inTrigger){
            unmerge.UnmergePlayers();
        }
    }
}