using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorButtonDoor : MonoBehaviour
{
    public static int[] sensor = Enumerable.Repeat(0, 10).ToArray();
    public int numDoor;
    public float speed = 45;
    
    void Start()
    {
        sensor[numDoor] = 0;
    }

    void Update()
    {
        if(sensor[numDoor] > 0){
            transform.Rotate(-Vector3.up * speed * Time.deltaTime);
        }
        else{
            
        }
    }
}
