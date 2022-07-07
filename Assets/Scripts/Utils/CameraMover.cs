using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            transform.position += transform.up * speed;
        }

        if (Input.GetKey(KeyCode.L))
        {
            transform.position += transform.right * speed;
        }
        if (Input.GetKey(KeyCode.J))
        {
            transform.position += -transform.right * speed;
        }

        if (Input.GetKey(KeyCode.K))
        {
            transform.position += -transform.up * speed;
        }
    }
}
