using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initialWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);

        Player player = collision.gameObject.GetComponent<Player>();

        if (collision.gameObject.tag == "Player")
        {
           transform.parent.GetComponent<Wall>().CollisionDetected();
        }

    }
}
