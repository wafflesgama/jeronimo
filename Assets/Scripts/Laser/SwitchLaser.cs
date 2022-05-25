using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SwitchLaser : MonoBehaviour
{
    public bool inTrigger;
    //public static bool[] isOn = Enumerable.Repeat(true, 10).ToArray();
    public int numLaser;

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }
 
    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(inTrigger){
            if (Input.GetKeyDown(KeyCode.E))
            {
                LaserDestroy.isOn[numLaser] = false;
            }
        }
    }

    void OnGUI()
    {
        if(inTrigger){
            GUI.Box(new Rect(0, 0, 500, 100), "Press E to turn laser off");
        }
    }
}
