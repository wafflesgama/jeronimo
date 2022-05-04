using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public GameObject wall;
    public GameObject destroyedWall;

    public GameObject playerController1;
    public GameObject playerController2;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < destroyedWall.transform.hierarchyCount; i++)
        {
            GameObject Go = destroyedWall.transform.GetChild(i).gameObject;

            Physics.IgnoreCollision(Go.GetComponent<Collider>(), playerController1.GetComponent<Collider>());
            Physics.IgnoreCollision(Go.GetComponent<Collider>(), playerController2.GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollisionDetected()
    {
        Destroy(wall);
        destroyedWall.SetActive(true);

        Debug.Log(destroyedWall.transform.hierarchyCount);

    }
}