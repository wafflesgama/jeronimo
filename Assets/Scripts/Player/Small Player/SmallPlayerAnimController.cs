using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPlayerAnimController : MonoBehaviour
{
    public Rigidbody body;

    Animator animator;

    public float rotationSpeed = 5;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var vel = body.velocity;
        vel.y = 0;


        var magnitude = vel.sqrMagnitude;
        animator.SetFloat("Speed", magnitude);

        vel.Normalize();

        if (magnitude > 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), rotationSpeed * Time.deltaTime);
        //transform.forward = Vector3.Lerp(transform.forward, vel, Time.deltaTime * rotationSpeed);
    }
}
