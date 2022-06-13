using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[RequireComponent(typeof(Rigidbody))]
public class Grabable : MonoBehaviour, Interactable
{
    public float throwThres = 1f;
    public float throwPower = 3;
    public Vector3 displayOffset;
    public Vector3 followOffset;

    public TrailRenderer trailRenderer;
    public float trailSpedThres=3;
    public Vector3 GetOffset() => displayOffset;

    public FollowSimple follower;

    private Player playerHolding;
    Rigidbody rb;

    public Collider[] colliders;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = rb.GetComponentsInChildren<Collider>().Where(x => !x.isTrigger).ToArray();
    }

    public void Interact(Player player)
    {
        playerHolding = player;
        player.GrabObject();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        follower.followTarget = player.smallAnimController.headBone.transform;
        EnableColliders(false);
        follower.SetFollowOffset(followOffset);
    }

    private void EnableColliders(bool enable)
    {
        foreach (var col in colliders)
        {
            col.isTrigger = !enable;
        }
    }


    public void Release()
    {
        follower.followTarget = null;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (playerHolding.smallMovController.rb.velocity.magnitude > throwThres)
            Throw();
        else
            Drop();
    }
    public async void Throw()
    {
        rb.isKinematic = false;
        rb.AddForce(playerHolding.smallMovController.rb.velocity * throwPower);
        EnableColliders(true);
    }

    public async void Drop()
    {
        rb.isKinematic = false;
        //rb.AddForce(rb.velocity * 2);
        EnableColliders(true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        trailRenderer.enabled = rb.velocity.sqrMagnitude > trailSpedThres;
        //if (rb.velocity.sqrMagnitude > trailSpedThres)
        //{

        //}
    }
}
