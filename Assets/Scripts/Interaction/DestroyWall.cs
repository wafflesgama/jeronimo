using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public GameObject Destroyed;
    public GameObject Normal;
    private int destroyVelocity = 1;

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "BigPlayer" && collider.relativeVelocity.magnitude>destroyVelocity)
        {
            Destroyed.SetActive(true);
            GameObject tmp=Destroyed.transform.GetChild(1).gameObject;
            GameObject BrokenWall = tmp.transform.GetChild(0).gameObject;

            foreach (Transform child in BrokenWall.transform)
            {
                child.gameObject.GetComponent<Rigidbody>().AddForce(50f, 50f,0,ForceMode.Impulse);
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
