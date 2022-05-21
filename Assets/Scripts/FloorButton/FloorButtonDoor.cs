using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorButtonDoor : MonoBehaviour
{
    public static bool[] sensor = Enumerable.Repeat(true, 10).ToArray();
    public int numDoor;
    public float speed = 45;
    
    void Start()
    {
        sensor[numDoor] = false;
    }

    void Update()
    {
        if(sensor[numDoor]){
            transform.Rotate(-Vector3.up * speed * Time.deltaTime);
        }
        else{
            
        }
    }
}
