using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform m_GrabPoint;

    [SerializeField]
    private Transform m_rayPoint;

    [SerializeField]
    private float m_rayDistance;

    private GameObject m_grabbedObject;
    private int m_layerIndex;
    private bool grabbed = false;
    GameObject item;
    private GameObject playerAnimator;



    private void Awake()
    {
        playerAnimator = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion direction = playerAnimator.GetComponent<SmallPlayerAnimController>().getDirection();
        Debug.Log(direction);
        Vector3 playerPos = this.transform.position;
        Vector3 playerDirection = this.transform.forward;
        float spawnDistance = 2;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        if (Input.GetKeyDown(KeyCode.V))
        {
            RaycastHit hit;

            if (Physics.Raycast(this.transform.position, this.transform.forward , out hit, 5))
            {

                if (hit.collider.tag == "Boxes")
                {
                    if (grabbed == false)
                    {
                        item = hit.collider.gameObject;
                        grabbed = true;
                        Debug.Log("You hit a pickObject!");
                    }
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(grabbed==true)
            {
                grabbed = false;
            }
        }
        if (grabbed == true)
        {
            item.transform.position = spawnPos;
        }
    }
}

