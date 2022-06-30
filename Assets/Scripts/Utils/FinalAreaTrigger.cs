using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAreaTrigger : MonoBehaviour
{
    public LevelManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "BigPlayer")
            manager.GameWin();
    }
}
