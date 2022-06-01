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
    public float maxAccel = 10;
    public LayerMask mask;
    public float castDist = 1f;

    public PlayerInputManager inputManager1;
    public PlayerInputManager inputManager2;

    private bool playerTurn = false;

    public float goalVelocity;

    public UEvent OnJump = new UEvent();

    public float jumpDownForce = 2f;

    public UEvent OnLand = new UEvent();

    public Vector3 vel;
    public Vector3 horizontalVel;
    public float horizontalVelMag;

    public bool isFloatGrounded { get; private set; }
    public bool isGrounded { get; private set; }

    bool isJumping;
    int jumpCounter;
    int jumpStartCounter;

    bool isLanding;

    Rigidbody rb;
    RaycastHit groundHit;

    UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        //inputManager.input_jump.Onpressed.Subscribe(eventHandler, () => jumpCounter = jumpFrames);
    }


    private void FixedUpdate()
    {
        CheckGround();

        Float();
        Move();

        AirbourneDecel();

        if (jumpCounter > 0)
            jumpCounter--;
    }

    private void CheckGround()
    {
        isFloatGrounded = Physics.Raycast(transform.position, -transform.up, out groundHit, castDist, mask, QueryTriggerInteraction.Ignore);
        isGrounded = isFloatGrounded ? groundHit.distance <= minGroundHeight : false;
    }

    private void Move()
    {
        if (!isGrounded) return;

        Vector3 force;

        if (!playerTurn)
        {
            var move = inputManager1.input_move.value;

            var moveRet = new Vector3(move.x, 0, move.y);
            var transformedMove = inputManager1.playerCamera.transform.TransformDirection(moveRet);
            transformedMove.y = 0;
            
            var aceleration = transformedMove * goalVelocity - rb.velocity;
            aceleration = Vector3.ClampMagnitude(aceleration, maxAccel);
            force = rb.mass * (aceleration / Time.fixedDeltaTime);
            force.y = 0;
            playerTurn = true;
        }
        else
        {
            var move = inputManager2.input_move.value;

            var moveRet = new Vector3(move.x, 0, move.y);
            var transformedMove = inputManager2.playerCamera.transform.TransformDirection(moveRet);
            transformedMove.y = 0;
            
            var aceleration = transformedMove * goalVelocity - rb.velocity;
            aceleration = Vector3.ClampMagnitude(aceleration, maxAccel);
            force = rb.mass * (aceleration / Time.fixedDeltaTime);
            force.y = 0;
            playerTurn = false;
        }

        
        rb.AddForce(force);
    }

    private void Float()
    {
        if (!isFloatGrounded || isJumping) return;

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
