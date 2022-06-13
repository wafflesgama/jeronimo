using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class Destructible : MonoBehaviour
{


    public Renderer[] renderers;
    public float colYvelocityToDestroy = 5;
    public float colVelocityToDestroy = 5;
    public int delayToKillObj = 1000;

    public VisualEffect destroyFX;
    public string eventName = "OnDestroy";

    public GameObject loot;
    private Rigidbody rb;
    private Collider[] colliders;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = rb.GetComponentsInChildren<Collider>().Where(x => !x.isTrigger).ToArray();
    }



    private async void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < colVelocityToDestroy || Mathf.Abs(collision.relativeVelocity.y) < colYvelocityToDestroy) return;
        rb.isKinematic = true;

        EnableColliders(false);

        foreach (var rend in renderers)
            rend.enabled = false;

        destroyFX.SendEvent(eventName);

        if (loot != null)
            Instantiate(loot, transform.position, Quaternion.identity);

        await Task.Delay(delayToKillObj);


        Destroy(gameObject);


    }


    private void EnableColliders(bool enable)
    {
        foreach (var col in colliders)
        {
            col.isTrigger = !enable;
        }
    }
}
