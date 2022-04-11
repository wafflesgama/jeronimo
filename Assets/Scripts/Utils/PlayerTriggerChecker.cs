using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class PlayerTriggerChecker : MonoBehaviour
{
    public bool searchInRigidbody;
    public LayerMask layersToCheck;
    public string tagToSearch;

    public bool isPlayerIn { get; private set; }

    public static bool DoesMaskContainsLayer(LayerMask layermask, int layer)
    {
        return layermask == (layermask | (1 << layer));
    }

    private void Awake()
    {
        isPlayerIn = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;

        isPlayerIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;

         isPlayerIn = false;
    }

    private bool IsPlayer(Collider other)
    {
        if (searchInRigidbody)
        {
            if (!DoesMaskContainsLayer(layersToCheck, other.attachedRigidbody.gameObject.layer)) return false;
            if (!string.IsNullOrEmpty(tagToSearch) && LayerMask.LayerToName(other.attachedRigidbody.gameObject.layer) == tagToSearch) return false;
        }
        else
        {
            if (!DoesMaskContainsLayer(layersToCheck, other.gameObject.layer)) return false;
            if (!string.IsNullOrEmpty(tagToSearch) && LayerMask.LayerToName(other.gameObject.layer) == tagToSearch) return false;
        }

        return true;    
    }

    
 
}
