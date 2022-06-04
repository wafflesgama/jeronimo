using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class BigPlayerMovController : MonoBehaviour
{
    public float deac;
    public float springStrength = 1;
    public float dampenStrength = 1;
    public float floatHeight = 1;
    public float minGroundHeight;

    public float baseStepSpeed = 30;
    public float maxSpeed = 80;
    public int maxStepCountBonus = 16;
    public LayerMask mask;
    public float castDist = 1f;

    public PlayerInputManager inputManager1;
    public PlayerInputManager inputManager2;

    public float secondsToResetStep = 2f;

    public float goalVelocity;

    public Vector3 vel;
    public Vector3 horizontalVel;
    public float horizontalVelMag;


    public bool isFloatGrounded { get; private set; }
    public bool isGrounded { get; private set; }


    public UEvent<bool> OnStep = new UEvent<bool>();
    public UEvent OnReset = new UEvent();

    public Rigidbody rb { get; private set; }
    RaycastHit groundHit;

    UEventHandler eventHandler = new UEventHandler();

    public bool? stepDirection;
    public int stepCounter;

    public DateTime? lastStep;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        inputManager1.input_jump.Onpressed.Subscribe(eventHandler, () => Step(true));
        inputManager2.input_jump.Onpressed.Subscribe(eventHandler, () => Step(false));
        ResetMovement();
    }
    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }


    private void FixedUpdate()
    {
        CheckGround();

        Float();
        //Move();

        AirbourneDecel();

        CheckStepTime();
    }

    private void CheckStepTime()
    {
        if (!lastStep.HasValue) return;

        var timeDif = DateTime.Now - lastStep.Value;
        if (timeDif.TotalSeconds < secondsToResetStep) return;

        lastStep = null;
        ResetMovement();
    }

    private void CheckGround()
    {
        isFloatGrounded = Physics.Raycast(transform.position, -transform.up, out groundHit, castDist, mask, QueryTriggerInteraction.Ignore);
        isGrounded = isFloatGrounded ? groundHit.distance <= minGroundHeight : false;
    }


    public void ResetMovement()
    {
        stepDirection = null;
        ResetInertia();
    }
    public void ResetInertia()
    {
        Debug.Log("Reset Intertia");
        OnReset.TryInvoke();
        stepCounter = 1;
    }
    private void Step(bool right)
    {

        if (!isGrounded) return;

        if (stepDirection != null && stepDirection == !right)
        {
            ResetMovement();
            return;
            //stepDirection = !stepDirection; //Invert once to maintain same side

        }

        lastStep = DateTime.Now;
        OnStep.TryInvoke(right);

        if (stepDirection == null)
            stepDirection = right;

        Debug.Log("Step right- " + right);

        Vector3 force;

        // Get Players Directions
        var move1 = inputManager1.input_move.value;
        var moveRet1 = new Vector3(move1.x, 0, move1.y);
        var transformedMove1 = Camera.main.transform.TransformDirection(moveRet1);
        transformedMove1.y = 0;

        var move2 = inputManager2.input_move.value;
        var moveRet2 = new Vector3(move2.x, 0, move2.y);
        var transformedMove2 = Camera.main.transform.TransformDirection(moveRet2);
        transformedMove2.y = 0;

        //Make combined direction
        var combinedDir = transformedMove1 + transformedMove2;
        combinedDir.Normalize();

        Debug.DrawRay(transform.position, combinedDir, Color.red, 3);

        force = combinedDir * (baseStepSpeed + baseStepSpeed * stepCounter);
        force.y = 0;
        rb.AddForce(force);

        stepCounter++;
        stepCounter = Mathf.Clamp(stepCounter, 0, maxStepCountBonus);
        stepDirection = !stepDirection;
    }


    private void Float()
    {
        if (!isFloatGrounded) return;

        Vector3 outsideVel = Vector3.zero;

        if (isFloatGrounded && groundHit.rigidbody != null)
            outsideVel = groundHit.rigidbody.velocity;

        float dirOwnVel = Vector3.Dot(-transform.up, rb.velocity);
        float dirOutsideVel = Vector3.Dot(-transform.up, outsideVel);



        float dirRelation = dirOwnVel - dirOutsideVel;
        float x = groundHit.distance - floatHeight;

        float spring = (x * springStrength) - (dirRelation * dampenStrength);

        rb.AddForce(-transform.up * rb.mass * spring);

        if (outsideVel != Vector3.zero)
        {
            groundHit.rigidbody.AddForceAtPosition(-transform.up * -springStrength, groundHit.point);
        }

    }


    private void AirbourneDecel()
    {
        if (isFloatGrounded || rb.velocity == Vector3.zero) return;
        var force = -rb.velocity * rb.mass * deac;
        force.y = 0f;
        rb.AddForce(force);
    }

    private void LateUpdate()
    {
        vel = rb.velocity;

        horizontalVel = vel;
        horizontalVel.y = 0f;

        horizontalVelMag = horizontalVel.magnitude;
    }
}
