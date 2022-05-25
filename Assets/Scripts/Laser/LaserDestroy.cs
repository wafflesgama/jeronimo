using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LaserDestroy : MonoBehaviour
{
    public bool inTrigger;
    public static bool[] isOn = Enumerable.Repeat(true, 10).ToArray();
    public int numLaser;
    
    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
        if(other.tag == "Player"){
            Application.LoadLevel(0);
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
        if(!isOn[numLaser]){
            this.gameObject.SetActive(false);
        }
    }
}