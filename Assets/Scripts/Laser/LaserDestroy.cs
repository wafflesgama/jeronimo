using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class LaserDestroy : MonoBehaviour
{
    public bool inTrigger;
    public static bool[] isOn = Enumerable.Repeat(true, 10).ToArray();
    public int numLaser;

    public MergeBehaviour unmerge;

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
        if (other.tag == "Player")
        {
            PlayerManager.current.KnockPlayers(other.attachedRigidbody.transform, other.ClosestPointOnBounds(transform.position));
        }
        if (other.tag == "BigPlayer")
        {
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
        await Task.Delay(2000);
        if(inTrigger){
            //Application.LoadLevel(0);
            unmerge.UnmergePlayers();
        }
    }
}