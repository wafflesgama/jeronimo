using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class SmallPlayerAnimController : MonoBehaviour
{

    public SmallPlayerMovController movController;
    public Transform headBone;

    public Quaternion rotation { get; private set; }
    public Vector3 horizontalDirection { get; private set; }
    public Rig holdRig;
    public float holdDuration;
    public Ease holdEaseIn;
    public Ease holdEaseOut;

    private float holdValue = 0;

    Animator animator;

    UEventHandler eventHandler = new UEventHandler();

    public float rotationSpeed = 5;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        movController.OnJump.Subscribe(eventHandler, Jump);
        movController.OnLand.Subscribe(eventHandler, Land);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Sprint", movController.isSprinting);

        var vel = movController.rb.velocity;
        vel.y = 0;


        var magnitude = vel.sqrMagnitude;
        animator.SetFloat("Speed", magnitude);

        vel.Normalize();

        if (magnitude > 0 && vel != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), rotationSpeed * Time.deltaTime);
            //transform.forward = Vector3.Lerp(transform.forward, vel, Time.deltaTime * rotationSpeed);
        }
        rotation = transform.rotation;
        horizontalDirection = Vector3.Scale(transform.forward, new Vector3(1, 0, 1));
        Debug.DrawRay(transform.position, horizontalDirection);
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
    }

    void Land()
    {
        animator.SetTrigger("Land");
    }

    public void Knock()
    {
        animator.SetTrigger("Knock");
    }

    public void Recover()
    {
        animator.SetTrigger("Recover");
    }

    public void Steal()
    {
        animator.SetTrigger("Steal");
    }

    public void HoldObj()
    {
        DOTween.To(() => holdValue, x => holdValue = x, .75f, holdDuration).SetEase(holdEaseIn)
    .OnUpdate(() =>
    {
        holdRig.weight = holdValue;
    });
    }

    public void ReleaseObj()
    {
        DOTween.To(() => holdValue, x => holdValue = x, 0, holdDuration).SetEase(holdEaseOut)
     .OnUpdate(() =>
     {
         holdRig.weight = holdValue;
     });
    }
}
