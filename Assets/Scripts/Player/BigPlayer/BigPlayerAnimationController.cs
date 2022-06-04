using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPlayerAnimationController : MonoBehaviour
{
    private Quaternion direction;
    public BigPlayerMovController movController;

    Animator animator;

    UEventHandler eventHandler = new UEventHandler();

    public float rotationSpeed = 5;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        //movController.OnJump.Subscribe(eventHandler, Jump);
        //movController.OnLand.Subscribe(eventHandler, Land);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("Sprint", movController.isSprinting);

        var vel = movController.rb.velocity;
        vel.y = 0;


        var magnitude = vel.sqrMagnitude;
        animator.SetFloat("Speed", magnitude);

        vel.Normalize();

        if (magnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), rotationSpeed * Time.deltaTime);
            direction = transform.rotation;
            //transform.forward = Vector3.Lerp(transform.forward, vel, Time.deltaTime * rotationSpeed);
        }
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
    }

    void Land()
    {
        animator.SetTrigger("Land");
    }

    public Quaternion getDirection()
    {
        return this.direction;
    }
}
