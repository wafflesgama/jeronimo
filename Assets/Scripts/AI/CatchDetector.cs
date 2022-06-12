using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class CatchDetector : MonoBehaviour
{
    public UEvent<Transform, Vector3> OnCatch = new UEvent<Transform, Vector3>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        var pos = other.ClosestPoint(transform.position);

        OnCatch.TryInvoke(other.transform, pos);

    }
}
