using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class InteractionHandler : MonoBehaviour
{

    public UEvent<Transform, Vector3> OnInteractableAppeared = new UEvent<Transform, Vector3>();
    public UEvent OnInteractableDisappeared = new UEvent();

    public GameObject objectToInteract = null;
    public Interactable interactableToInteract = null;

    public Player player;

    bool canInteract;
    bool isHolding;

    UEventHandler eventHandler = new UEventHandler();

    private void Start()
    {
        canInteract = true;
        player.inputManager.input_interact.Onpressed.Subscribe(eventHandler, TryInteract);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }
    public bool IsInteractableNearby() => objectToInteract != null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null) return;

        if (other.transform.parent.tag == "Interactable")
        {
            if (objectToInteract == null || objectToInteract.transform.position != other.transform.position)
            {
                objectToInteract = other.gameObject;
                if (objectToInteract.transform.parent.TryGetComponent<Interactable>(out Interactable interactable))
                    OnInteractableAppeared.TryInvoke(other.transform, interactable.GetOffset());
            }
        }
        else if (other.transform.parent.tag == "KillBound")
        {
            LevelManager.current.GameOver();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == null) return;

        //if (other.transform.parent.tag == "Interactable" || objectToInteract.transform.parent.tag != "Uninteractable")
        //{
        if (objectToInteract != null && objectToInteract.transform.position == other.transform.position && objectToInteract.name == other.gameObject.name)
        {
            objectToInteract = null;
            OnInteractableDisappeared.TryInvoke();
        }
        //}
    }

    public void TryInteract()
    {
        if (isHolding)
        {
            ((Grabable)interactableToInteract).Release();
            isHolding = false;
            canInteract = true;
            return;
        }

        if (!canInteract || objectToInteract == null || objectToInteract.transform.parent.tag != "Interactable") return;
        //if (PlayerCutsceneManager.isInCutscene) return;

        interactableToInteract = objectToInteract.transform.parent.GetComponent<Interactable>();


        interactableToInteract.Interact(player);

        if (interactableToInteract is Grabable)
        {
            isHolding = true;
            canInteract = false;
        }
        OnInteractableDisappeared.TryInvoke();
    }

}
