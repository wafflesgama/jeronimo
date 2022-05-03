using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator leftDoor = null;
    [SerializeField] private Animator rightDoor = null;


    [SerializeField] private bool openTrigger = false;

    [SerializeField] private bool closeTrigger = false;

    [SerializeField] private string leftDoorOpen = "LeftDoorOpen";
    [SerializeField] private string leftDoorClose = "LeftDoorClose";

    [SerializeField] private string rightDoorOpen = "RightDoorOpen";
    [SerializeField] private string rightDoorClose = "RightDoorClose";

    public GameObject key = null;




    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.gameObject.tag);
        if (other.gameObject.tag == "Key")
        {

            if (openTrigger)
            {

                leftDoor.Play(leftDoorOpen, 0, 0.0f);
                rightDoor.Play(rightDoorOpen, 0, 0.0f);

                other.gameObject.GetComponent<Renderer>().enabled = false;

                other.gameObject.GetComponent<Key>().used = true;
                gameObject.SetActive(false);
                         

            }
            if (closeTrigger && other.gameObject.GetComponent<Key>().used)
            {

                leftDoor.Play(leftDoorClose, 0, 0.0f);
                rightDoor.Play(rightDoorClose, 0, 0.0f);

                Destroy(other.gameObject);
                gameObject.SetActive(false);
            }

        }
    }
}