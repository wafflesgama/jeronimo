using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public GameObject Destroyed;
    public GameObject DestroyPushPieces;
    public GameObject Normal;
    private int destroyVelocity = 1;

    private void OnCollisionEnter(Collision collider)
    {
        Destroy(collider);
    }

    [ContextMenu("Destroy Wall")]
    public void DestroyTest()
    {
        Destroy(null, true);
    }

    public void Destroy(Collision collider, bool test = false)
    {
        if (test || collider.gameObject.tag == "BigPlayer")
        {
            Destroyed.SetActive(true);
            Normal.SetActive(false);
            //GameObject tmp = Destroyed.transform.GetChild(1).gameObject;
            //GameObject BrokenWall = tmp.transform.GetChild(0).gameObject;
            //Destroy(gameObject);

            foreach (Transform child in DestroyPushPieces.transform)
            {
                child.GetComponent<Rigidbody>().AddForce(50f, 0, 50f, ForceMode.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
