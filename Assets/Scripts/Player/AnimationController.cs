using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Rigidbody body;

    Animator animator;

    public float rotationSpeed=5;
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
        var vel=body.velocity.normalized;
        vel.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, vel, Time.deltaTime * rotationSpeed);
        animator.SetFloat("Speed",body.velocity.sqrMagnitude);
    }
}
