using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SwitchLaser : MonoBehaviour, Interactable
{

    public bool inTrigger;
    //public static bool[] isOn = Enumerable.Repeat(true, 10).ToArray();
    public int numLaser;
    private string eventName = "event:/SFX/Click Button";

    public Vector3 displayOffset;
    public Collider[] colliders;

    public GameObject LedON, LedOFF;

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    void Start()
    {
        LedON = transform.GetChild(1).gameObject;
        LedOFF = transform.GetChild(2).gameObject;
        LedON.gameObject.SetActive(true);
        LedOFF.gameObject.SetActive(false);
    }


    private void Awake()
    {
        colliders = this.gameObject.GetComponentsInChildren<Collider>().Where(x => !x.isTrigger).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector3 GetOffset() => displayOffset;

    public void Interact(Player player)
    {

        SoundController.current.PlayOneShotEvent(eventName);
        LedON.gameObject.SetActive(false);
        LedOFF.gameObject.SetActive(true);
        LaserDestroy.isOn[numLaser] = false;
    }

}
